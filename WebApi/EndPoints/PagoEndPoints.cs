using Core.Interfaces;
using Domain.Entities;
using Kicket.Contracts.Common;
using Kicket.Contracts.Compras;
using Kicket.Contracts.Pagos;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace WebApi.EndPoints
{
    public static class PagoEndPoints
    {
        public static void MapPagoEndPoints(this WebApplication app)
        {
            var pagosGroup = app.MapGroup("/pagos")
                                .RequireAuthorization();

            pagosGroup.MapPost("/crear-preferencia", async (
                PreferenciaPagoRequest request,
                IMercadoPagoService mpService,
                HttpContext httpContext,
                IConfiguration config) =>
            {
                try
                {
                    // Detecta la URL base del frontend que invoca
                    string? referer = httpContext.Request.Headers["Referer"].ToString();
                    string baseUrlRetorno;

                    if (!string.IsNullOrWhiteSpace(referer) && Uri.TryCreate(referer, UriKind.Absolute, out var uriReferer))
                    {
                        baseUrlRetorno = $"{uriReferer.Scheme}://{uriReferer.Authority}";
                    }
                    else
                    {
                        baseUrlRetorno = config["BlazorBaseUrl"] ?? "http://localhost:5268";
                    }

                    var response = await mpService.CrearPreferenciaAsync(request, baseUrlRetorno);
                    return Results.Ok(response);
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(new ApiError
                    {
                        Status = StatusCodes.Status400BadRequest,
                        Title = "No se pudo generar la preferencia de pago",
                        Detail = ex.Message
                    });
                }
                catch (Exception ex)
                {
                    return Results.Problem(
                        detail: ex.Message,
                        title: "Error inesperado al conectar con Mercado Pago",
                        statusCode: StatusCodes.Status500InternalServerError);
                }
            })
            .WithName("CrearPreferenciaPago")
            .Produces<PreferenciaPagoResponse>(StatusCodes.Status200OK)
            .Produces<ApiError>(StatusCodes.Status400BadRequest);

            pagosGroup.MapPost("/confirmar", async (
                ConfirmarPagoRequest request,
                IMercadoPagoService mpService,
                ICompraService compraService) =>
            {
                if (!string.Equals(request.Status, "approved", StringComparison.OrdinalIgnoreCase))
                {
                    return Results.BadRequest(new ApiError
                    {
                        Status = StatusCodes.Status400BadRequest,
                        Title = "Pago no aprobado",
                        Detail = $"El estado del pago es '{request.Status}'. No se puede emitir la compra."
                    });
                }

                bool pagoValido = await mpService.ValidarPagoAprobadoAsync(request.PaymentId);
                if (!pagoValido)
                {
                    return Results.BadRequest(new ApiError
                    {
                        Status = StatusCodes.Status400BadRequest,
                        Title = "Pago inválido",
                        Detail = "No se pudo comprobar la validez de la transacción en Mercado Pago."
                    });
                }

                try
                {
                    Compra nuevaCompra = await compraService.GenerarCompraAsync(
                        usuarioId: request.UsuarioId,
                        eventoId: request.EventoId,
                        sectorId: request.SectorId,
                        cantidad: request.Cantidad,
                        metodoPago: string.IsNullOrWhiteSpace(request.MetodoPago) ? "MercadoPago" : request.MetodoPago,
                        paymentId: request.PaymentId,
                        estadoPago: "Aprobado");

                    CompraDto dto = CompraEndPoints.MapearACompraDto(nuevaCompra);
                    return Results.Created($"/compras/{dto.CompraId}", dto);
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(new ApiError
                    {
                        Status = StatusCodes.Status400BadRequest,
                        Title = "No se pudo emitir la compra",
                        Detail = ex.Message
                    });
                }
            })
            .WithName("ConfirmarPago")
            .Produces<CompraDto>(StatusCodes.Status201Created)
            .Produces<ApiError>(StatusCodes.Status400BadRequest);
        }
    }
}
