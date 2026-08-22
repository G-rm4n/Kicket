namespace WebApi.EndPoints
{
    public static class EntradaEndPoints
    {
        public static void MapEntradaEndPoints(this WebApplication app)
        {
            app.MapGet("/entradas", async (/*IEntradaService entradaService*/) =>
            {
                /*
                 * var entradas = await entradaService.GetAllEntradas();
                 */
            })
            .WithName("GetAllEntradas");

            app.MapGet("/entradas/{id}", async (/*int id, IEntradaService entradaService*/) =>
            {
                /*
                 * EntradaDTO entrada = await entradaService.GetEntradaById(id);
                 * 
                 * if(entrada == null){
                 *     return Results.NotFound();
                 * }
                 * 
                 * return entrada;
                 */
            })
            .WithName("GetEntrada")
            .Produces(StatusCodes.Status404NotFound);

            app.MapPost("/entradas", async (/*EntradaDTO dto, IEntradaService entradaService*/) =>
            {
                /*
                 * EntradaDTO entrada = await entradaService.AddAsync(dto);
                 * return Results.Created($"/entradas/{entrada.Id}", entrada);
                 */
            })
            .WithName("AddEntrada")
            .Produces(StatusCodes.Status400BadRequest);

            app.MapPut("/entradas", (/*EntradaDTO dto, IEntradaService entradaService*/) =>
            {
                /*
                 * var found = await entradaService.Update(dto);
                 * if (!found)
                 * {
                 *     return Results.NotFound();
                 * }
                 * return Results.NoContent();
                 */
            })
            .WithName("UpdateEntrada")
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest);

            app.MapDelete("/entradas/{id}", (/*int id, IEntradaService entradaService*/) =>
            {
                /*
                 * var deleted = await entradaService.Delete(id);
                 * if (!deleted)
                 * {
                 *     return Results.NotFound();
                 * }
                 * return Results.NoContent();
                 */
            })
            .WithName("DeleteEntrada")
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest);
        }
    }
}
