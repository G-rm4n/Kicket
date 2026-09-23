using Core.Interfaces;
using Domain.Entities;
using Kicket.Contracts.Common;
using Kicket.Contracts.Compras;
using Kicket.Contracts.Entradas;

namespace WebApi.EndPoints
{
    public static class CompraEndPoints
    {
        public static void MapCompraEndPoints(this WebApplication app)
        {
            app.MapGet("/compras", async (ICompraService compraService) =>
            {
                var compras = await compraService.ObtenerTodosAsync();

                IEnumerable<CompraDto> dtos = compras.Select(MapearACompraDto).ToList();

                return Results.Ok(dtos);
            })
            .WithName("GetAllCompras")
            .Produces<IEnumerable<CompraDto>>(StatusCodes.Status200OK);

            app.MapGet("/compras/{id}", async (int id, ICompraService compraService) =>
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

            // No hay PUT /compras: una compra no se modifica una vez generada
            // (ver la nota en CompraRequest, Fase 1). Ante un error, se elimina.
            app.MapPost("/compras", async (CompraRequest compraReq, ICompraService compraService) =>
            {
                try
                {
                    Compra nuevaCompra = await compraService.GenerarCompraAsync(compraReq.UsuarioId, compraReq.EventoId, compraReq.SectorId, compraReq.Cantidad);

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

            app.MapDelete("/compras/{id}", async (int id, ICompraService compraService) =>
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

        private static CompraDto MapearACompraDto(Compra compra)
        {
            return new CompraDto
            {
                CompraId = compra.CompraId,
                UsuarioId = compra.UsuarioId,
                FechaCompra = compra.FechaCompra,
                Cantidad = compra.Cantidad,
                MontoTotal = compra.MontoTotal,
                Entradas = compra.Entradas.Select(e => new EntradaDto
                {
                    EntradaId = e.EntradaId,
                    CompraId = e.CompraId,
                    EventoId = e.EventoId,
                    SectorId = e.SectorId,
                    FilaAsiento = e.FilaAsiento
                }).ToList()
            };
        }
    }
}