using System.Collections.Generic;
using Core.Interfaces;
using Domain.Entities;
using Kicket.Contracts.Clubes;
using Kicket.Contracts.Estadios;

namespace WebApi.EndPoints
{
    public static class ClubEndPoints
    {
        public static void MapClubEndPoints(this WebApplication app)
        {
            var clubGroup = app.MapGroup("/clubes")
                                .RequireAuthorization();

            clubGroup.MapGet("/", async (IClubService clubService) =>
            {

                var clubes = await clubService.ObtenerTodosAsync();
                IEnumerable<ClubDto> clubDtos= clubes.Select(c => new ClubDto
                {
                    ClubId = c.ClubId,
                    Nombre = c.Nombre,
                    Descripcion=c.Descripcion,
                    Abreviatura = c.Abreviatura
                }).ToList();

                return Results.Ok(clubDtos);
            })
            .WithName("GetAllClubes")
            .Produces<IEnumerable<ClubDto>>(StatusCodes.Status200OK);

            clubGroup.MapGet("/{id}", static async (int id, IClubService clubService) =>
            {
                
                  Club? club = await clubService.ObtenerPorIdAsync(id);
                  
                  if(club == null){
                      return Results.NotFound();
                  }

                  ClubDto clubDto = new()
                  {
                        ClubId=club.ClubId,
                        Abreviatura=club.Abreviatura,
                        Descripcion=club.Descripcion,
                        Nombre=club.Nombre
                    
                  };
    
                  return Results.Ok(clubDto);
                 
            })
            .WithName("GetClub")
            .Produces<ClubDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

            clubGroup.MapPost("/", async (ClubRequest clubReq, IClubService clubService) =>
            {

                Club club = new()
                {
                    Abreviatura = clubReq.Abreviatura,
                    Descripcion = clubReq.Descripcion,
                    Nombre = clubReq.Nombre
                };

                Club newClub = await clubService.CrearClubAsync(club);

                ClubDto clubDto = new()
                {
                    Abreviatura = newClub.Abreviatura,
                    Descripcion = newClub.Descripcion,
                    Nombre = newClub.Nombre,
                    ClubId=newClub.ClubId
                };

                return Results.Created($"/clubes/{clubDto.ClubId}", clubDto);
                 
            })
            .WithName("AddClub")
            .Produces<ClubDto>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest);

            clubGroup.MapPut("/", async (ClubUpdateRequest clubReq, IClubService clubService) =>
            {
                Club club = new()
                {
                    Abreviatura = clubReq.Abreviatura,
                    Descripcion = clubReq.Descripcion,
                    Nombre = clubReq.Nombre,
                    ClubId=clubReq.IdClub
                };

                bool res=await clubService.ActualizarClubAsync(club);

                if (res)
                {
                    return Results.NoContent();
                }
                return Results.NotFound();
            })
            .WithName("UpdateClub")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest);

            clubGroup.MapDelete("/clubes/{id}",async (int id, IClubService clubService) =>
            {
                
                 var deleted = await clubService.EliminarClubAsync(id);
                 if (!deleted)
                 {
                     return Results.NotFound();
                 }
                 return Results.NoContent();
                 
            })
            .WithName("DeleteClub")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest);
        }
    }
}
