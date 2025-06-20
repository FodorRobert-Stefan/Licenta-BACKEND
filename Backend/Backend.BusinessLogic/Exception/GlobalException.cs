using System.Net;
using System.Text.Json;
using Backend.BusinessLogic;
using Backend.BusinessLogic.Exception;
using Backend.BusinessLogic.Response;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace Backend.Middleware
{
  public class GlobalExceptionMiddleware
  {
    private readonly RequestDelegate _next;

    public GlobalExceptionMiddleware(RequestDelegate next)
    {
      _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
      try
      {
        await _next(context);
      }
      catch (AbstractValidatorException ex)
      {
        Log.Warning(ex, "Validation failed: {Message}", ex.Message);
        await HandleExceptionAsync(context, ex);
      }
      catch (BaseException ex)
      {
        Log.Warning(ex, "Handled business exception: {Message}", ex.Message);
        await HandleExceptionAsync(context, ex);
      }
      catch (Exception ex)
      {
        Log.Error(ex, "Unhandled exception occurred.");
        await HandleExceptionAsync(context, HttpStatusCode.InternalServerError, "An unexpected error occurred.");
      }
    }

    private static async Task HandleExceptionAsync(HttpContext context, BaseException ex)
    {
      context.Response.ContentType = "application/json";
      context.Response.StatusCode = (int)ex.StatusCode;

      var response = ex.GetResponse();
      var json = JsonSerializer.Serialize(response);
      await context.Response.WriteAsync(json);
    }

    private static async Task HandleExceptionAsync(HttpContext context, HttpStatusCode statusCode, string message)
    {
      context.Response.ContentType = "application/json";
      context.Response.StatusCode = (int)statusCode;

      var response = new Response(message, statusCode);
      var json = JsonSerializer.Serialize(response);
      await context.Response.WriteAsync(json);
    }
  }
}
