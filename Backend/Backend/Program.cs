using Backend.BusinessLogic.Generic.Create;
using Backend.BusinessLogic.Generic.Get;
using Backend.CommonDomain;
using Backend.CommonDomain.UserCommon;
using Backend.DataAbstraction;
using Backend.Database;
using Backend.Domain.UserDomain;
using Backend.Services;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.Extensions.DependencyModel;
using Microsoft.Extensions.Options;
using Serilog;
using System.Reflection;

internal class Program
{
  private static Assembly[]? assemblies;

  private static void Main(string[] args)
  {
    var builder = WebApplication.CreateBuilder(args);
    assemblies = LoadApplicationDependencies();

    builder.Host.UseSerilog((ctx, lc) =>
      lc.ReadFrom.Configuration(ctx.Configuration));

    Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Information()
        .WriteTo.File("logs/app-log.txt", rollingInterval: RollingInterval.Day)
        .CreateLogger();

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.AddAutoMapper(assemblies);

    builder.Services.Configure<DatabaseConfiguration>(builder.Configuration.GetSection("DatabaseConfiguration"));
    builder.Services.Configure<AzureConfig>(builder.Configuration.GetSection("AzureAd"));
    builder.Services.Configure<IEmailConfig>(builder.Configuration.GetSection("Email"));
    builder.Services.Configure<JwtConfig>(builder.Configuration.GetSection("JwtConfig"));

    builder.Services.AddSingleton<IDatabaseConfiguration>(sp => sp.GetRequiredService<IOptions<DatabaseConfiguration>>().Value);
    builder.Services.AddSingleton<IDatabase, MongoDatabase>();

    builder.Services.AddSingleton<IAzureConfig>(sp => sp.GetRequiredService<IOptions<AzureConfig>>().Value);
    builder.Services.AddScoped<IEmailConfig>(sp => sp.GetRequiredService<IOptions<IEmailConfig>>().Value);
    builder.Services.AddScoped<IJwtConfig>(sp => sp.GetRequiredService<IOptions<JwtConfig>>().Value);

    builder.Services.AddScoped<IHashingGenerator, HashingGenerator>();
    builder.Services.AddScoped<IAzureService, AzureService>();

    builder.Services.AddScoped(typeof(IGenericCrudRepository<>), typeof(MongoGenericRepository<>));

    builder.Services.AddMediatR(cfg =>
    {
      cfg.RegisterServicesFromAssemblies(assemblies);
    });
    builder.Services.AddTransient<IRequestHandler<GenericCreateRequest<CreateUserDto, User>, GenericCreateResponse>, GenericCreateHandler<CreateUserDto, User>>();
    builder.Services.AddTransient<IRequestHandler<GenericGetByIdRequest<CreateUserDto, User>, GenericGetByIdResponse<CreateUserDto>>, GenericGetByIdHandler<User, CreateUserDto>>();
    builder.Services.AddTransient<IRequestHandler<GenericGetByFilterRequest<GetUserDto, User>, GenericGetByFilterResponse<GetUserDto>>, GenericGetByFilterHandler<GetUserDto, User>>();

    builder.Services.AddFluentValidation(cfg =>
    {
      cfg.RegisterValidatorsFromAssemblies(assemblies);
    });

    builder.Services.AddScoped(typeof(IRequestHandler<,>), typeof(GenericCreateHandler<,>));

    builder.Services.Scan(scan => scan
      .FromAssemblyOf<IRepository>()
      .AddClasses(classes => classes.AssignableTo<IRepository>())
      .AsImplementedInterfaces()
      .WithScopedLifetime());

    builder.Services.AddCors(options =>
    {
      options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
    });

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
      app.UseSwagger();
      app.UseSwaggerUI();
    }

    MongoClassMapRegistration.RegisterAll();

    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.UseCors();
    app.MapControllers();

    app.Run();
  }

  private static Assembly[] LoadApplicationDependencies()
  {
    var context = DependencyContext.Default;

    return context.RuntimeLibraries
        .SelectMany(lib => lib.GetDefaultAssemblyNames(context))
        .Where(assembly => assembly.FullName.Contains("Backend"))
        .Select(Assembly.Load)
        .ToArray();
  }
}
