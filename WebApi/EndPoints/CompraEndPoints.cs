using System.Security.Claims;
using Core.Interfaces;
using Domain.Entities;
using Kicket.Contracts.Common;
using Kicket.Contracts.Compras;
using Kicket.Contracts.Entradas;
using WebApi.Seguridad;

namespace WebApi.EndPoints
{
    // Endpoints REST de compras. Todo el grupo exige estar logueado; además cada ruta controla
    // que el usuario solo vea o genere SUS compras (el Admin puede ver todas).
    public static class CompraEndPoints
    {
        // Registra todas las rutas /compras.
        public static void MapCompraEndPoints(this WebApplication app)
        {
            // Prefijo /compras y autorización obligatoria para todo el grupo.
            var comprasGroup = app.MapGroup("/compras")
                                  .RequireAuthorization();

            // GET /compras: lista todas las compras. Solo Admin (antes cualquier logueado veía las de todos).
            comprasGroup.MapGet("/", async (ICompraService compraService) =>
            {
                var compras = await compraService.ObtenerTodosAsync();

                IEnumerable<CompraDto> dtos = compras.Select(MapearACompraDto).ToList();

                return Results.Ok(dtos);
            })
            .WithName("GetAllCompras")
            .Produces<IEnumerable<CompraDto>>(StatusCodes.Status200OK)
            .RequireAuthorization("Solo admin");

            // GET /compras/paginado: página de compras con filtros (evento, fechas, estado, usuario). Solo Admin.
            comprasGroup.MapGet("/paginado", async ([AsParameters] CompraFiltro filtro, ICompraService compraService, CancellationToken ct) =>
                Results.Ok(await compraService.ObtenerPaginadoAsync(filtro, ct)))
            .WithName("GetComprasPaginado")
            .Produces<PagedResult<CompraDto>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .RequireAuthorization("Solo admin");

            // GET /compras/mis-entradas: partidos del usuario logueado con todas sus entradas, paginados por partido.
            // El usuario sale del token, no de la URL: no hay forma de pedir las entradas de otro.
            comprasGroup.MapGet("/mis-entradas", async ([AsParameters] MisEntradasFiltro filtro, ClaimsPrincipal user,
                ICompraService compraService, CancellationToken ct) =>
            {
                int? usuarioId = user.ObtenerId();
                if (usuarioId is null)
                {
                    return Results.Unauthorized();
                }
                return Results.Ok(await compraService.ObtenerMisEntradasAsync(usuarioId.Value, filtro, ct));
            })
            .WithName("GetMisEntradas")
            .Produces<PagedResult<PartidoConEntradasDto>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);

            // GET /compras/usuario/{id}: compras de un usuario. Solo el propio usuario o un Admin (si no, 403).
            comprasGroup.MapGet("/usuario/{usuarioId:int}", async (int usuarioId, ClaimsPrincipal user, ICompraService compraService) =>
            {
                if (!user.PuedeVer(usuarioId))
                {
                    return Results.Forbid();
                }

                var compras = await compraService.ObtenerPorUsuarioAsync(usuarioId);
                IEnumerable<CompraDto> dtos = compras.Select(MapearACompraDto).ToList();

                return Results.Ok(dtos);
            })
            .WithName("GetComprasPorUsuario")
            .Produces<IEnumerable<CompraDto>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status403Forbidden);

            // GET /compras/{id}: devuelve una compra o 404. Solo el dueño o un Admin.
            comprasGroup.MapGet("/{id:int}", async (int id, ClaimsPrincipal user, ICompraService compraService) =>
            {
                Compra? compra = await compraService.ObtenerPorIdAsync(id);

                if (compra is null)
                {
                    return Results.NotFound();
                }
                if (!user.PuedeVer(compra.UsuarioId))
                {
                    return Results.Forbid();
                }

                return Results.Ok(MapearACompraDto(compra));
            })
            .WithName("GetCompra")
            .Produces<CompraDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status404NotFound);

            // No hay PUT /compras: una compra no se modifica una vez generada.
            // Ante un error, se elimina.
            // POST /compras: genera una compra con sus entradas; si una validación falla responde 400.
            // Un usuario común siempre compra a su nombre (se ignora el UsuarioId del cuerpo); un Admin puede indicar otro.
            comprasGroup.MapPost("/", async (CompraRequest compraReq, ClaimsPrincipal user, ICompraService compraService) =>
            {
                // Ojo: los usuarios de prueba tienen ids negativos, por eso se controla null y no "<= 0".
                int? usuarioId = user.EsAdmin() ? compraReq.UsuarioId : user.ObtenerId();
                if (usuarioId is null)
                {
                    return Results.Unauthorized();
                }

                try
                {
                    Compra nuevaCompra = await compraService.GenerarCompraAsync(
                        usuarioId.Value,
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

            // DELETE /compras/{id}: anula la compra y sus entradas. Solo Admin (antes lo podía hacer cualquier logueado). 204 o 404.
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
            .Produces(StatusCodes.Status404NotFound)
            .RequireAuthorization("Solo admin");
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
