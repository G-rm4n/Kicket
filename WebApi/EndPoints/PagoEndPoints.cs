using Core.Interfaces;
using Domain.Entities;
using Kicket.Contracts.Common;
using Kicket.Contracts.Compras;
using Kicket.Contracts.Pagos;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace WebApi.EndPoints
{
    // Endpoints de pago con Mercado Pago. Todo el grupo exige estar logueado.
    public static class PagoEndPoints
    {
        // Registra las rutas /pagos.
        public static void MapPagoEndPoints(this WebApplication app)
        {
            // Prefijo /pagos y autorización obligatoria.
            var pagosGroup = app.MapGroup("/pagos")
                                .RequireAuthorization();

            // POST /pagos/crear-preferencia: crea la preferencia de pago y devuelve la URL a la que ir a pagar.
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

                    // Si vino un Referer válido usa su esquema y host; si no, la URL configurada o un valor fijo.
                    if (!string.IsNullOrWhiteSpace(referer) && Uri.TryCreate(referer, UriKind.Absolute, out var uriReferer))
                    {
                        baseUrlRetorno = $"{uriReferer.Scheme}://{uriReferer.Authority}";
                    }
                    else
                    {
                        baseUrlRetorno = config["BlazorBaseUrl"] ?? "http://localhost:5268";
                    }

                    // Pide al servicio la preferencia (real o simulada) y la devuelve.
                    var response = await mpService.CrearPreferenciaAsync(request, baseUrlRetorno);
                    return Results.Ok(response);
                }
                // Error de validación (evento, sector, stock...) → 400.
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(new ApiError
                    {
                        Status = StatusCodes.Status400BadRequest,
                        Title = "No se pudo generar la preferencia de pago",
                        Detail = ex.Message
                    });
                }
                // Cualquier otro error → 500.
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

            // POST /pagos/confirmar: con el pago aprobado, genera la compra y sus entradas.
            pagosGroup.MapPost("/confirmar", async (
                ConfirmarPagoRequest request,
                IMercadoPagoService mpService,
                ICompraService compraService) =>
            {
                // 1) El estado informado tiene que ser "approved".
                if (!string.Equals(request.Status, "approved", StringComparison.OrdinalIgnoreCase))
                {
                    return Results.BadRequest(new ApiError
                    {
                        Status = StatusCodes.Status400BadRequest,
                        Title = "Pago no aprobado",
                        Detail = $"El estado del pago es '{request.Status}'. No se puede emitir la compra."
                    });
                }

                // 2) El servicio de Mercado Pago tiene que dar el pago por válido.
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

                // 3) Genera la compra con estado "Aprobado" y responde 201 con la compra creada.
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
                // Si la compra no se puede generar (sin stock, evento no disponible...) → 400.
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
