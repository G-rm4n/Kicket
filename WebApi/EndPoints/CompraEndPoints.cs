namespace WebApi.EndPoints
{
    public static class CompraEndPoints
    {
        public static void MapCompraEndPoints(this WebApplication app)
        {
            app.MapGet("/compras", async (/*ICompraService compraService*/) =>
            {
                /*
                 * var compras = await compraService.GetAllCompras();
                 */
            })
            .WithName("GetAllCompras");

            app.MapGet("/compras/{id}", async (/*int id, ICompraService compraService*/) =>
            {
                /*
                 * CompraDTO compra = await compraService.GetCompraById(id);
                 * 
                 * if(compra == null){
                 *     return Results.NotFound();
                 * }
                 * 
                 * return compra;
                 */
            })
            .WithName("GetCompra")
            .Produces(StatusCodes.Status404NotFound);

            app.MapPost("/compras", async (/*CompraDTO dto, ICompraService compraService*/) =>
            {
                /*
                 * CompraDTO compra = await compraService.AddAsync(dto);
                 * return Results.Created($"/compras/{compra.Id}", compra);
                 */
            })
            .WithName("AddCompra")
            .Produces(StatusCodes.Status400BadRequest);

            app.MapPut("/compras", (/*CompraDTO dto, ICompraService compraService*/) =>
            {
                /*
                 * var found = await compraService.Update(dto);
                 * if (!found)
                 * {
                 *     return Results.NotFound();
                 * }
                 * return Results.NoContent();
                 */
            })
            .WithName("UpdateCompra")
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest);

            app.MapDelete("/compras/{id}", (/*int id, ICompraService compraService*/) =>
            {
                /*
                 * var deleted = await compraService.Delete(id);
                 * if (!deleted)
                 * {
                 *     return Results.NotFound();
                 * }
                 * return Results.NoContent();
                 */
            })
            .WithName("DeleteCompra")
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest);
        }
    }
}
