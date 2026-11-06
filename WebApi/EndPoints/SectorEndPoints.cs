using Core.Interfaces;
using Domain.Entities;
using Kicket.Contracts.Common;
using Kicket.Contracts.Sectores;
using Microsoft.EntityFrameworkCore;

namespace WebApi.EndPoints
{
    // Endpoints REST de sectores: consultas públicas; alta, edición y baja solo para Admin.
    public static class SectorEndPoints
    {
        // Registra todas las rutas /sectores.
        public static void MapSectorEndPoints(this WebApplication app)
        {
            // GET /sectores: lista todos los sectores como DTO.
            app.MapGet("/sectores", async (ISectorService sectorService) =>
            {
                var sectores = await sectorService.ObtenerTodosAsync();

                IEnumerable<SectorDto> dtos = sectores.Select(s => s.ADto()).ToList();

                return Results.Ok(dtos);
            })
            .WithName("GetAllSectores")
            .Produces<IEnumerable<SectorDto>>(StatusCodes.Status200OK);

            // GET /sectores/paginado: página de sectores con filtros (estadio, nombre, tipo Platea/Popular, capacidad mínima).
            app.MapGet("/sectores/paginado", async ([AsParameters] SectorFiltro filtro, ISectorService sectorService, CancellationToken ct) =>
                Results.Ok(await sectorService.ObtenerPaginadoAsync(filtro, ct)))
            .WithName("GetSectoresPaginado")
            .Produces<PagedResult<SectorDto>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);

            // GET /sectores/{id}: devuelve un sector o 404.
            app.MapGet("/sectores/{id}", async (int id, ISectorService sectorService) =>
            {
                Sector? sector = await sectorService.ObtenerPorIdAsync(id);

                if (sector is null)
                {
                    return Results.NotFound();
                }

                SectorDto dto = sector.ADto();

                return Results.Ok(dto);
            })
            .WithName("GetSector")
            .Produces<SectorDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

            // GET /sectores/estadio/{id}: lista los sectores de un estadio.
            app.MapGet("/sectores/estadio/{estadioId}", async (int estadioId, ISectorService sectorService) =>
            {
                var sectores = await sectorService.ObtenerPorEstadioAsync(estadioId);

                IEnumerable<SectorDto> dtos = sectores.Select(s => s.ADto()).ToList();

                return Results.Ok(dtos);
            })
            .WithName("GetSectoresPorEstadio")
            .Produces<IEnumerable<SectorDto>>(StatusCodes.Status200OK);

            // POST /sectores: crea un sector (solo Admin). 201 o 400.
            app.MapPost("/sectores", async (SectorRequest sectorReq, ISectorService sectorService) =>
            {
                Sector sector = new()
                {
                    EstadioId = sectorReq.EstadioId,
                    Nombre = sectorReq.Nombre,
                    CapacidadMaxima = sectorReq.CapacidadMaxima,
                    PrecioBase = sectorReq.PrecioBase
                };

                try
                {
                    Sector nuevoSector = await sectorService.CrearSectorAsync(sector);

                    SectorDto dto = nuevoSector.ADto();

                    return Results.Created($"/sectores/{dto.SectorId}", dto);
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(new ApiError { Status = StatusCodes.Status400BadRequest, Title = "Datos inválidos", Detail = ex.Message });
                }
            })
            .WithName("AddSector")
            .Produces<SectorDto>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .RequireAuthorization("Solo admin");

            // PUT /sectores: modifica un sector (solo Admin). 204, 404 o 400.
            app.MapPut("/sectores", async (SectorUpdateRequest sectorReq, ISectorService sectorService) =>
            {
                Sector sector = new()
                {
                    SectorId = sectorReq.SectorId,
                    EstadioId = sectorReq.EstadioId,
                    Nombre = sectorReq.Nombre,
                    CapacidadMaxima = sectorReq.CapacidadMaxima,
                    PrecioBase = sectorReq.PrecioBase
                };

                try
                {
                    var found = await sectorService.ActualizarSectorAsync(sector);

                    if (!found)
                    {
                        return Results.NotFound();
                    }

                    return Results.NoContent();
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(new ApiError { Status = StatusCodes.Status400BadRequest, Title = "Datos inválidos", Detail = ex.Message });
                }
            })
            .WithName("UpdateSector")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .RequireAuthorization("Solo admin");

            // DELETE /sectores/{id}: borra un sector (solo Admin). 204, 404, o 409 si tiene entradas vendidas.
            app.MapDelete("/sectores/{id}", async (int id, ISectorService sectorService) =>
            {
                try
                {
                    var deleted = await sectorService.EliminarSectorAsync(id);

                    if (!deleted)
                    {
                        return Results.NotFound();
                    }

                    return Results.NoContent();
                }
                // La FK Restrict de Entrada→Sector hace fallar el borrado: se responde 409 Conflict.
                catch (DbUpdateException)
                {
                    return Results.Conflict(new ApiError { Status = StatusCodes.Status409Conflict, Title = "No se puede eliminar el sector", Detail = "El sector tiene entradas vendidas asociadas." });
                }
            })
            .WithName("DeleteSector")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status409Conflict)
            .RequireAuthorization("Solo admin");
        }
    }
}