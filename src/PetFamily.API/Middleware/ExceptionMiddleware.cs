using System.Net;
using System.Text.Json;
using PetFamily.Domain.Exceptions;

namespace PetFamily.API.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
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
        catch (NotFoundException ex)
        {
            _logger.LogWarning(ex, "Recurso não encontrado.");
            await WriteResponseAsync(context, HttpStatusCode.NotFound, ex.Message);
        }
        catch (BusinessException ex)
        {
            _logger.LogWarning(ex, "Regra de negócio violada.");
            await WriteResponseAsync(context, HttpStatusCode.BadRequest, ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro interno não tratado.");
            await WriteResponseAsync(context, HttpStatusCode.InternalServerError, "Ocorreu um erro interno. Tente novamente mais tarde.");
        }
    }

    private static async Task WriteResponseAsync(HttpContext context, HttpStatusCode statusCode, string message)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var response = new { statusCode = (int)statusCode, message };
        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}
