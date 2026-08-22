namespace WebApi.EndPoints
{
    public static class SectorEndPoints
    {
        public static void MapSectorEndPoints(this WebApplication app)
        {
            app.MapGet("/sectores", async (/*ISectorService sectorService*/) =>
            {
                /*
                 * var sectores = await sectorService.GetAllSectores();
                 */
            })
            .WithName("GetAllSectores");

            app.MapGet("/sectores/{id}", async (/*int id, ISectorService sectorService*/) =>
            {
                /*
                 * SectorDTO sector = await sectorService.GetSectorById(id);
                 * 
                 * if(sector == null){
                 *     return Results.NotFound();
                 * }
                 * 
                 * return sector;
                 */
            })
            .WithName("GetSector")
            .Produces(StatusCodes.Status404NotFound);

            app.MapPost("/sectores", async (/*SectorDTO dto, ISectorService sectorService*/) =>
            {
                /*
                 * SectorDTO sector = await sectorService.AddAsync(dto);
                 * return Results.Created($"/sectores/{sector.Id}", sector);
                 */
            })
            .WithName("AddSector")
            .Produces(StatusCodes.Status400BadRequest);

            app.MapPut("/sectores", (/*SectorDTO dto, ISectorService sectorService*/) =>
            {
                /*
                 * var found = await sectorService.Update(dto);
                 * if (!found)
                 * {
                 *     return Results.NotFound();
                 * }
                 * return Results.NoContent();
                 */
            })
            .WithName("UpdateSector")
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest);

            app.MapDelete("/sectores/{id}", (/*int id, ISectorService sectorService*/) =>
            {
                /*
                 * var deleted = await sectorService.Delete(id);
                 * if (!deleted)
                 * {
                 *     return Results.NotFound();
                 * }
                 * return Results.NoContent();
                 */
            })
            .WithName("DeleteSector")
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest);
        }
    }
}
