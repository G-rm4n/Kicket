using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Middlewares;

// Manejador global de excepciones: atrapa cualquier excepción no controlada de los endpoints
// y la convierte en una respuesta JSON ProblemDetails con el código HTTP adecuado.
public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    // Recibe el logger por inyección de dependencias.
    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    // Se ejecuta cuando un endpoint tira una excepción que nadie atrapó.
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        // Registra el error en el log.
        _logger.LogError(exception, "Excepción no controlada interceptada.");

        
        // Elige el código HTTP según el tipo de excepción.
        var statusCode = exception switch
        {
            ArgumentException => StatusCodes.Status400BadRequest,
            UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
            InvalidOperationException => StatusCodes.Status409Conflict,
            _ => StatusCodes.Status500InternalServerError
        };

        // Arma el cuerpo de la respuesta con el título y el mensaje de la excepción.
        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = "Ocurrió un error al procesar la solicitud",
            Detail = exception.Message 
        };

        // Escribe el código y el JSON en la respuesta.
        httpContext.Response.StatusCode = statusCode;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        // true = la excepción quedó manejada.
        return true;
    }
}