using Core.Interfaces;
using Domain.Entities;
using Kicket.Contracts.Common;
using Kicket.Contracts.Eventos;

namespace WebApi.EndPoints
{
    public static class EventoEndPoints
    {
        public static void MapEventoEndPoints(this WebApplication app)
        {
            app.MapGet("/eventos", async (IEventoService eventoService) =>
            {
                var eventos = await eventoService.ObtenerTodosAsync();

                IEnumerable<EventoDto> dtos = eventos.Select(e => new EventoDto
                {
                    IdEvento = e.IdEvento,
                    Nombre = e.Nombre,
                    Fecha = e.Fecha,
                    IdEstadio = e.EstadioId,
                    IdEquipoLocal = e.ClubLocalId,
                    IdEquipoVisitante = e.ClubVisitanteId,
                    EstaCancelado = e.EstaCancelado
                }).ToList();

                return Results.Ok(dtos);
            })
            .WithName("GetAllEventos")
            .Produces<IEnumerable<EventoDto>>(StatusCodes.Status200OK);

            app.MapGet("/eventos/{id}", async (int id, IEventoService eventoService) =>
            {
                Evento? evento = await eventoService.ObtenerPorIdAsync(id);

                if (evento is null)
                {
                    return Results.NotFound();
                }

                EventoDto dto = new()
                {
                    IdEvento = evento.IdEvento,
                    Nombre = evento.Nombre,
                    Fecha = evento.Fecha,
                    IdEstadio = evento.EstadioId,
                    IdEquipoLocal = evento.ClubLocalId,
                    IdEquipoVisitante = evento.ClubVisitanteId,
                    EstaCancelado = evento.EstaCancelado
                };

                return Results.Ok(dto);
            })
            .WithName("GetEvento")
            .Produces<EventoDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

            app.MapPost("/eventos", async (EventoRequest eventoReq, IEventoService eventoService) =>
            {
                Evento evento = new()
                {
                    Nombre = eventoReq.Nombre,
                    Fecha = eventoReq.Fecha,
                    EstadioId = eventoReq.EstadioId,
                    ClubLocalId = eventoReq.ClubLocalId,
                    ClubVisitanteId = eventoReq.ClubVisitanteId
                };

                try
                {
                    Evento nuevoEvento = await eventoService.CrearEventoAsync(evento);

                    EventoDto dto = new()
                    {
                        IdEvento = nuevoEvento.IdEvento,
                        Nombre = nuevoEvento.Nombre,
                        Fecha = nuevoEvento.Fecha,
                        IdEstadio = nuevoEvento.EstadioId,
                        IdEquipoLocal = nuevoEvento.ClubLocalId,
                        IdEquipoVisitante = nuevoEvento.ClubVisitanteId,
                        EstaCancelado = nuevoEvento.EstaCancelado
                    };

                    return Results.Created($"/eventos/{dto.IdEvento}", dto);
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(new ApiError { Status = StatusCodes.Status400BadRequest, Title = "Datos inválidos", Detail = ex.Message });
                }
            })
            .WithName("AddEvento")
            .Produces<EventoDto>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest);

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
                    EstaCancelado = eventoReq.EstaCancelado
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
            .Produces(StatusCodes.Status400BadRequest);

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
            .Produces(StatusCodes.Status404NotFound);
        }
    }
}