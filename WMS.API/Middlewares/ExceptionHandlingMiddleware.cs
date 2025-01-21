using System.Net;
using System.Text.Json;
using WMS.Core.Exceptions;
using WMS.Models.Common;
using WMS.Models.Enums;

namespace WMS.Api.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    private readonly IWebHostEnvironment _hostEnvironment;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger, IWebHostEnvironment hostEnvironment)
    {
        _next = next;
        _logger = logger;
        _hostEnvironment = hostEnvironment;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (WMSException ex)
        {
            await HandleExceptionAsync(context, ex);
        }
        catch (Exception ex)
        {
            if (_hostEnvironment.IsDevelopment())
            {
                await HandleExceptionAsync(context, ex);
                return;
            }

            _logger.LogError(ex, "Unhandled Error");
            await UnhandledException(context);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var response = new ResponseModel<dynamic>()
        {
            Errors = new List<string> { exception.Message }
        };

        if (exception is WMSException tvmException)
        {
            response = new ResponseModel<dynamic>()
            {
                Errors = new List<string> { tvmException.Message }
            };
            response.StatusCode = (int)tvmException.StatusCode;
            context.Response.StatusCode = (int)tvmException.StatusCode;
        }
        else
        {
            response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        }

        var result = JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(result);
    }

    private static async Task UnhandledException(HttpContext context)
    {
        var value = new WMSException("Internal Server Error", ExceptionType.ServerError, HttpStatusCode.InternalServerError);
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsJsonAsync(value);
    }
}