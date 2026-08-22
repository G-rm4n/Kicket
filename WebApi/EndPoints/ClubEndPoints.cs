namespace WebApi.EndPoints
{
    public static class ClubEndPoints
    {
        public static void MapClubEndPoints(this WebApplication app)
        {
            app.MapGet("/clubes", async (/*IClubService clubService*/) =>
            {
                /*
                 * var clubes = await clubService.GetAllClubes();
                 */
            })
            .WithName("GetAllClubes");

            app.MapGet("/clubes/{id}", async (/*int id, IClubService clubService*/) =>
            {
                /*
                 * ClubDTO club = await clubService.GetClubById(id);
                 * 
                 * if(club == null){
                 *     return Results.NotFound();
                 * }
                 * 
                 * return club;
                 */
            })
            .WithName("GetClub")
            .Produces(StatusCodes.Status404NotFound);

            app.MapPost("/clubes", async (/*ClubDTO dto, IClubService clubService*/) =>
            {
                /*
                 * ClubDTO club = await clubService.AddAsync(dto);
                 * return Results.Created($"/clubes/{club.Id}", club);
                 */
            })
            .WithName("AddClub")
            .Produces(StatusCodes.Status400BadRequest);

            app.MapPut("/clubes", (/*ClubDTO dto, IClubService clubService*/) =>
            {
                /*
                 * var found = await clubService.Update(dto);
                 * if (!found)
                 * {
                 *     return Results.NotFound();
                 * }
                 * return Results.NoContent();
                 */
            })
            .WithName("UpdateClub")
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest);

            app.MapDelete("/clubes/{id}", (/*int id, IClubService clubService*/) =>
            {
                /*
                 * var deleted = await clubService.Delete(id);
                 * if (!deleted)
                 * {
                 *     return Results.NotFound();
                 * }
                 * return Results.NoContent();
                 */
            })
            .WithName("DeleteClub")
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest);
        }
    }
}
