using Core.Interfaces;
using Domain.Entities;
using Kicket.Contracts.Estadios;

namespace WebApi.EndPoints
{
    // Endpoints REST de estadios: listar y consultar son públicos; el resto requiere rol Admin.
    public static class EstadioEndPoints
    {
        // Registra todas las rutas /estadios.
        public static void MapEstadioEndPoints(this WebApplication app)
        {
            // Agrupa las rutas bajo /estadios.
            var estadiosGroup = app.MapGroup("/estadios");

            // GET /estadios: lista todos los estadios como DTO.
            estadiosGroup.MapGet("/", async (IEstadioService estadioService) =>
            {
                var estadios = await estadioService.ObtenerTodosAsync();

                IEnumerable<EstadioDto> dtos = estadios.Select(e => new EstadioDto()
                {
                    IdEstadio = e.EstadioId,
                    Direccion = e.Direccion,
                    Ciudad = e.Ciudad,
                    Nombre = e.Nombre,
                }).ToList();

                return Results.Ok(dtos);
            })
            .WithName("GetAllEstadios")
            .Produces<IEnumerable<EstadioDto>>(StatusCodes.Status200OK);

            // GET /estadios/{id}: devuelve un estadio o 404.
            estadiosGroup.MapGet("/{id}", async (int id, IEstadioService estadioService) =>
            {
                Estadio? estadio = await estadioService.ObtenerPorIdAsync(id);

                if (estadio == null)
                {
                    return Results.NotFound();
                }

                EstadioDto estadioDto = new()
                {
                    Ciudad = estadio.Ciudad,
                    Direccion = estadio.Direccion,
                    Nombre = estadio.Nombre,
                    IdEstadio = estadio.EstadioId
                };

                return Results.Ok(estadioDto);
            })
            .WithName("GetEstadio")
            .Produces<EstadioDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

            // POST /estadios: crea un estadio (solo Admin) y responde 201.
            estadiosGroup.MapPost("/", async (EstadioRequest estadioReq, IEstadioService estadioService) =>
            {
                Estadio estadio = new()
                {
                    Ciudad = estadioReq.Ciudad,
                    Direccion = estadioReq.Direccion,
                    Nombre = estadioReq.Nombre
                };

                Estadio estadioNew = await estadioService.CrearEstadioAsync(estadio);

                EstadioDto estadioDto = new()
                {
                    Ciudad = estadioNew.Ciudad,
                    Nombre = estadioNew.Nombre,
                    Direccion = estadioNew.Direccion,
                    IdEstadio = estadioNew.EstadioId
                };

                return Results.Created($"/estadios/{estadioDto.IdEstadio}", estadioDto);
            })
            .WithName("AddEstadio")
            .Produces<EstadioDto>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .RequireAuthorization("Solo admin");

            // PUT /estadios: modifica un estadio (solo Admin); el id va en el cuerpo. 204 o 404.
            estadiosGroup.MapPut("/", async (EstadioUpdateRequest estadioReq, IEstadioService estadioService) =>
            {
                Estadio estadio = new()
                {
                    Ciudad = estadioReq.Ciudad,
                    Direccion = estadioReq.Direccion,
                    Nombre = estadioReq.Nombre,
                    EstadioId = estadioReq.IdEstadio
                };

                var found = await estadioService.ActualizarEstadioAsync(estadio);

                if (!found)
                {
                    return Results.NotFound();
                }

                return Results.NoContent();
            })
            .WithName("UpdateEstadio")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .RequireAuthorization("Solo admin");

            // DELETE /estadios/{id}: borra un estadio (solo Admin). 204 o 404.
            estadiosGroup.MapDelete("/{id}", async (int id, IEstadioService estadioService) =>
            {
                var deleted = await estadioService.EliminarEstadioAsync(id);

                if (!deleted)
                {
                    return Results.NotFound();
                }

                return Results.NoContent();
            })
            .WithName("DeleteEstadio")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .RequireAuthorization("Solo admin");
        }
    }
}
