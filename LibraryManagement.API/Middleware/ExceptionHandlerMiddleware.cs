using System.Net;
using System.Text.Json;
using LibraryManagement.Domain.Exceptions;
using FluentValidation;

namespace LibraryManagement.API.Middleware;

public class ExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlerMiddleware> _logger;

    public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "An unhandled exception occurred.");
            await HandleExceptionAsync(context, exception);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (statusCode, message, errors) = exception switch
        {
            NotFoundException notFoundException => ((int)HttpStatusCode.NotFound, notFoundException.Message, (string[]?)null),
            BusinessRuleException businessRuleException => ((int)HttpStatusCode.BadRequest, businessRuleException.Message, (string[]?)null),
            ValidationException validationException => ((int)HttpStatusCode.BadRequest, "Validation failed.", validationException.Errors.Select(e => e.ErrorMessage).ToArray()),
            _ => ((int)HttpStatusCode.InternalServerError, "An unexpected error occurred.", (string[]?)null)
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        var response = new
        {
            StatusCode = statusCode,
            Message = message,
            Errors = errors
        };

        return context.Response.WriteAsync(JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        }));
    }
}
