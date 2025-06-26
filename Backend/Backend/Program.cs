using Backend.BusinessLogic.Generic.Create;
using Backend.CommonDomain;
using Backend.CommonDomainu;
using Backend.DataAbstraction;
using Backend.DataAbstraction.IAzure;
using Backend.Database;
using Backend.Domain.UserDomain;
using Backend.Extensions;
using Backend.Middleware;
using Backend.Service;
using Backend.Services;
using FluentValidation;
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

    builder.Services.AddUserServices();

    builder.Services.AddFluentValidation(cfg =>
    {
      cfg.RegisterValidatorsFromAssemblies(assemblies);
    });
    builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

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

    builder.Services.Configure<AzureBlobStorageSettings>(builder.Configuration.GetSection("AzureBlobStorage"));
    builder.Services.AddSingleton<IAzureBlobBackupService, AzureBlobBackupService>();
    builder.Services.AddHostedService<BackupService>();

    builder.Services.Configure<CryptoSettings>(
    builder.Configuration.GetSection("CryptoSettings"));

    builder.Services.AddScoped<IHybridCryptoService>(provider =>
    {
      var cryptoSettings = provider.GetRequiredService<IOptions<CryptoSettings>>().Value;
      return new HybridCryptoService(cryptoSettings.RsaPrivateKeyBase64);
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
    app.UseMiddleware<GlobalExceptionMiddleware>();

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
