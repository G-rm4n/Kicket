using System.Collections.Generic;
using System.Net;
using Core.Interfaces;
using Domain.Entities;
using Kicket.Contracts.Clubes;

namespace WebApi.EndPoints
{
    // Endpoints REST de clubes: listar y consultar son públicos; crear, modificar y borrar requieren rol Admin.
    public static class ClubEndPoints
    {
        // Registra todas las rutas /clubes.
        public static void MapClubEndPoints(this WebApplication app)
        {
            // Agrupa todas las rutas bajo el prefijo /clubes.
            var clubGroup = app.MapGroup("/clubes");

            // GET /clubes: lista todos los clubes convertidos a DTO.
            clubGroup.MapGet("/", async (IClubService clubService) =>
            {
                var clubes = await clubService.ObtenerTodosAsync();
                IEnumerable<ClubDto> clubDtos = clubes.Select(c => new ClubDto
                {
                    ClubId = c.ClubId,
                    Nombre = c.Nombre,
                    Descripcion = c.Descripcion,
                    Abreviatura = c.Abreviatura
                }).ToList();

                return Results.Ok(clubDtos);
            })
            .WithName("GetAllClubes")
            .Produces<IEnumerable<ClubDto>>(StatusCodes.Status200OK);

            // GET /clubes/{id}: devuelve un club o 404 si no existe.
            clubGroup.MapGet("/{id}", static async (int id, IClubService clubService) =>
            {
                Club? club = await clubService.ObtenerPorIdAsync(id);

                if (club == null)
                {
                    return Results.Problem(detail: "Club no encontrado", statusCode: (int)HttpStatusCode.NotFound);
                }

                ClubDto clubDto = new()
                {
                    ClubId = club.ClubId,
                    Abreviatura = club.Abreviatura,
                    Descripcion = club.Descripcion,
                    Nombre = club.Nombre
                };

                return Results.Ok(clubDto);
            })
            .WithName("GetClub")
            .Produces<ClubDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

            // POST /clubes: crea un club (solo Admin) y responde 201 con el club creado.
            clubGroup.MapPost("/", async (ClubRequest clubReq, IClubService clubService) =>
            {
                // Convierte el request en la entidad.
                Club club = new()
                {
                    Abreviatura = clubReq.Abreviatura,
                    Descripcion = clubReq.Descripcion,
                    Nombre = clubReq.Nombre
                };

                // El servicio valida y guarda; si los datos son inválidos tira ArgumentException (400).
                Club newClub = await clubService.CrearClubAsync(club);

                // Convierte la entidad guardada (ya con id) a DTO.
                ClubDto clubDto = new()
                {
                    Abreviatura = newClub.Abreviatura,
                    Descripcion = newClub.Descripcion,
                    Nombre = newClub.Nombre,
                    ClubId = newClub.ClubId
                };

                return Results.Created($"/clubes/{clubDto.ClubId}", clubDto);
            })
            .WithName("AddClub")
            .Produces<ClubDto>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .RequireAuthorization("Solo admin");

            // PUT /clubes: modifica un club (solo Admin); el id viene en el cuerpo. 204 si sale bien, 404 si no existe.
            clubGroup.MapPut("/", async (ClubUpdateRequest clubReq, IClubService clubService) =>
            {
                Club club = new()
                {
                    Abreviatura = clubReq.Abreviatura,
                    Descripcion = clubReq.Descripcion,
                    Nombre = clubReq.Nombre,
                    ClubId = clubReq.IdClub
                };

                bool res = await clubService.ActualizarClubAsync(club);

                if (res)
                {
                    return Results.NoContent();
                }
                return Results.Problem(detail: "El club no existe", statusCode: (int)HttpStatusCode.NotFound);
            })
            .WithName("UpdateClub")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .RequireAuthorization("Solo admin");

            // DELETE /clubes/{id}: borra un club (solo Admin). 204 si sale bien, 404 si no existe.
            // Si el club tiene eventos, la FK Restrict hace fallar el borrado y el handler global responde 500.
            clubGroup.MapDelete("/{id}", async (int id, IClubService clubService) =>
            {
                var deleted = await clubService.EliminarClubAsync(id);
                if (!deleted)
                {
                    return Results.Problem(detail: "El club no existe", statusCode: (int)HttpStatusCode.NotFound);
                }

                return Results.NoContent();
            })
            .WithName("DeleteClub")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .RequireAuthorization("Solo admin");
        }
    }
}
