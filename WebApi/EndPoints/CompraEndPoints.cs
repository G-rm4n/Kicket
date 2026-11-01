using Core.Interfaces;
using Domain.Entities;
using Kicket.Contracts.Common;
using Kicket.Contracts.Compras;
using Kicket.Contracts.Entradas;

namespace WebApi.EndPoints
{
    // Endpoints REST de compras. Todo el grupo exige estar logueado (cualquier rol).
    public static class CompraEndPoints
    {
        // Registra todas las rutas /compras.
        public static void MapCompraEndPoints(this WebApplication app)
        {
            // Prefijo /compras y autorización obligatoria para todo el grupo.
            var comprasGroup = app.MapGroup("/compras")
                                  .RequireAuthorization();

            // GET /compras: lista todas las compras.
            comprasGroup.MapGet("/", async (ICompraService compraService) =>
            {
                var compras = await compraService.ObtenerTodosAsync();

                IEnumerable<CompraDto> dtos = compras.Select(MapearACompraDto).ToList();

                return Results.Ok(dtos);
            })
            .WithName("GetAllCompras")
            .Produces<IEnumerable<CompraDto>>(StatusCodes.Status200OK);

            // GET /compras/usuario/{id}: trae todas las compras y filtra en memoria las del usuario.
            comprasGroup.MapGet("/usuario/{usuarioId:int}", async (int usuarioId, ICompraService compraService) =>
            {
                var compras = await compraService.ObtenerTodosAsync();

                IEnumerable<CompraDto> dtos = compras
                    .Where(c => c.UsuarioId == usuarioId)
                    .Select(MapearACompraDto)
                    .ToList();

                return Results.Ok(dtos);
            })
            .WithName("GetComprasPorUsuario")
            .Produces<IEnumerable<CompraDto>>(StatusCodes.Status200OK);

            // GET /compras/{id}: devuelve una compra o 404.
            comprasGroup.MapGet("/{id:int}", async (int id, ICompraService compraService) =>
            {
                Compra? compra = await compraService.ObtenerPorIdAsync(id);

                if (compra is null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(MapearACompraDto(compra));
            })
            .WithName("GetCompra")
            .Produces<CompraDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

            // No hay PUT /compras: una compra no se modifica una vez generada.
            // Ante un error, se elimina.
            // POST /compras: genera una compra con sus entradas; si una validación falla responde 400.
            comprasGroup.MapPost("/", async (CompraRequest compraReq, ICompraService compraService) =>
            {
                try
                {
                    Compra nuevaCompra = await compraService.GenerarCompraAsync(
                        compraReq.UsuarioId,
                        compraReq.EventoId,
                        compraReq.SectorId,
                        compraReq.Cantidad,
                        compraReq.MetodoPago,
                        compraReq.PaymentId,
                        compraReq.EstadoPago);

                    CompraDto dto = MapearACompraDto(nuevaCompra);

                    return Results.Created($"/compras/{dto.CompraId}", dto);
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(new ApiError { Status = StatusCodes.Status400BadRequest, Title = "No se pudo generar la compra", Detail = ex.Message });
                }
            })
            .WithName("AddCompra")
            .Produces<CompraDto>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest);

            // DELETE /compras/{id}: borra la compra y sus entradas. 204 o 404.
            comprasGroup.MapDelete("/{id:int}", async (int id, ICompraService compraService) =>
            {
                var deleted = await compraService.EliminarCompraAsync(id);

                if (!deleted)
                {
                    return Results.NotFound();
                }

                return Results.NoContent();
            })
            .WithName("DeleteCompra")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);
        }

        // Convierte una Compra (con sus entradas) a CompraDto. La usa también PagoEndPoints.
        public static CompraDto MapearACompraDto(Compra compra)
        {
            return new CompraDto
            {
                CompraId = compra.CompraId,
                UsuarioId = compra.UsuarioId,
                FechaCompra = compra.FechaCompra,
                Cantidad = compra.Cantidad,
                MontoTotal = compra.MontoTotal,
                MetodoPago = compra.MetodoPago ?? "MercadoPago",
                PaymentId = compra.PaymentId,
                EstadoPago = compra.EstadoPago ?? "Aprobado",
                Entradas = compra.Entradas.Select(e => new EntradaDto
                {
                    EntradaId = e.EntradaId,
                    CompraId = e.CompraId,
                    EventoId = e.EventoId,
                    SectorId = e.SectorId,
                    FilaAsiento = e.FilaAsiento,
                    CodigoQr = e.CodigoQr
                }).ToList()
            };
        }
    }
}
