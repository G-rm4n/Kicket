using Core.Interfaces;
using Domain.Entities;
using Kicket.Contracts.Common;
using Kicket.Contracts.Eventos;

namespace WebApi.EndPoints
{
    // Endpoints REST de eventos: consultas públicas; alta, edición y baja solo para Admin.
    public static class EventoEndPoints
    {
        // Registra todas las rutas /eventos.
        public static void MapEventoEndPoints(this WebApplication app)
        {
            // GET /eventos: lista todos los eventos como DTO.
            app.MapGet("/eventos", async (IEventoService eventoService) =>
            {
                var eventos = await eventoService.ObtenerTodosAsync();

                IEnumerable<EventoDto> dtos = eventos.Select(e => e.ADto()).ToList();

                return Results.Ok(dtos);
            })
            .WithName("GetAllEventos")
            .Produces<IEnumerable<EventoDto>>(StatusCodes.Status200OK);

            // GET /eventos/paginado: página de eventos con filtros (club, estadio, rango de fechas, estado)
            // y los nombres de clubes y estadio ya resueltos. Público, igual que GET /eventos.
            app.MapGet("/eventos/paginado", async ([AsParameters] EventoFiltro filtro, IEventoService eventoService, CancellationToken ct) =>
                Results.Ok(await eventoService.ObtenerPaginadoAsync(filtro, ct)))
            .WithName("GetEventosPaginado")
            .Produces<PagedResult<EventoDto>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);

            // GET /eventos/{id}: devuelve un evento o 404.
            app.MapGet("/eventos/{id}", async (int id, IEventoService eventoService) =>
            {
                Evento? evento = await eventoService.ObtenerPorIdAsync(id);

                if (evento is null)
                {
                    return Results.NotFound();
                }

                EventoDto dto = evento.ADto();

                return Results.Ok(dto);
            })
            .WithName("GetEvento")
            .Produces<EventoDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

            // GET /eventos/{id}/disponibilidad: stock y precio de cada sector para el evento (404 si no existe).
            app.MapGet("/eventos/{id}/disponibilidad", async (int id, IEventoService eventoService) =>
            {
                var disponibilidad = await eventoService.ObtenerDisponibilidadAsync(id);

                if (disponibilidad is null)
                {
                    return Results.NotFound(new ApiError
                    {
                        Status = StatusCodes.Status404NotFound,
                        Title = "Evento no encontrado",
                        Detail = $"No existe un evento con ID {id}."
                    });
                }

                return Results.Ok(disponibilidad);
            })
            .WithName("GetEventoDisponibilidad")
            .Produces<IReadOnlyList<SectorDisponibilidadDto>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

            // POST /eventos: crea un evento (solo Admin). 201 si sale bien, 400 si falla una validación.
            app.MapPost("/eventos", async (EventoRequest eventoReq, IEventoService eventoService) =>
            {
                // Convierte el request en la entidad Evento.
                Evento evento = new()
                {
                    Nombre = eventoReq.Nombre,
                    Fecha = eventoReq.Fecha,
                    EstadioId = eventoReq.EstadioId,
                    ClubLocalId = eventoReq.ClubLocalId,
                    ClubVisitanteId = eventoReq.ClubVisitanteId,
                    Descripcion = eventoReq.Descripcion
                };

                try
                {
                    // El servicio valida (fecha futura, clubes distintos, estadio y clubes existentes) y guarda.
                    Evento nuevoEvento = await eventoService.CrearEventoAsync(evento);

                    EventoDto dto = nuevoEvento.ADto();

                    return Results.Created($"/eventos/{dto.IdEvento}", dto);
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(new ApiError { Status = StatusCodes.Status400BadRequest, Title = "Datos inválidos", Detail = ex.Message });
                }
            })
            .WithName("AddEvento")
            .Produces<EventoDto>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .RequireAuthorization("Solo admin");

            // PUT /eventos: modifica un evento (solo Admin), incluido si está cancelado. 204, 404 o 400.
            app.MapPut("/eventos", async (EventoUpdateRequest eventoReq, IEventoService eventoService) =>
            {
                Evento evento = new()
                {
                    IdEvento = eventoReq.IdEvento,
                    Nombre = eventoReq.Nombre,
                    Fecha = eventoReq.Fecha,
                    EstadioId = eventoReq.EstadioId,
                    ClubLocalId = eventoReq.ClubLocalId,
                    ClubVisitanteId = eventoReq.ClubVisitanteId,
                    EstaCancelado = eventoReq.EstaCancelado,
                    Descripcion = eventoReq.Descripcion
                };

                try
                {
                    var found = await eventoService.ActualizarEventoAsync(evento);

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
            .WithName("UpdateEvento")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .RequireAuthorization("Solo admin");

            // DELETE /eventos/{id}: borra un evento (solo Admin). 204 o 404.
            app.MapDelete("/eventos/{id}", async (int id, IEventoService eventoService) =>
            {
                var deleted = await eventoService.EliminarEventoAsync(id);

                if (!deleted)
                {
                    return Results.NotFound();
                }

                return Results.NoContent();
            })
            .WithName("DeleteEvento")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .RequireAuthorization("Solo admin");
        }
    }
}
