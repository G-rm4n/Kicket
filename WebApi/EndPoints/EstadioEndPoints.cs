namespace WebApi.EndPoints
{
    public static class EstadioEndPoints
    {
        public static void MapEstadioEndPoints(this WebApplication app)
        {
            app.MapGet("/estadios", async (/*IEstadioService estadioService*/) =>
            {
                /*
                 * var estadios=await estadioService.GetallEstadios();
                 */
            })
            .WithName("GetAllEstadios");
            /*.Produces<List<EstadioDTO>>(Statuscode.Status200OK)*/
            ;

            app.MapGet("/estadios/{id}", async (/*int id,IEstadioService estadioService*/) =>
            {
                /*
                 * EstadioDTO estadio=await EstadioService.getEstadioById(id);
                 * 
                 * if(estadio ==null){
                 *      return Results.NotFound()
                 * };
                 * 
                 * return estadio;
                 *
                 */
            })
            .WithName("GetEstadio")
            /*.Produces<EstadioDTO>(Statuscode.Status200OK)
             */.Produces(StatusCodes.Status404NotFound)
            ;

            app.MapPost("/estadios", async (/*EstadioDTO dto,IEstadioService estadioService*/) =>
            {
                /*
                EstadioDTO estadio = await estadioService.AddAsync(dto);
                return Results.Created($"/estadios/{estadio.Id}", estadio);
                */

            })
            .WithName("AddEstadio")
            /*Produces<EstadioDTO>(StatusCodes.Status201Created)*/
            .Produces(StatusCodes.Status400BadRequest);

            app.MapPut("/estadios", (/*EstadioDTO dto,IEstadioService estadioService*/) =>
            {
                /*
                var found = await estadioService.Update(dto);

                if (!found)
                {
                    return Results.NotFound();
                }

                return Results.NoContent();
                */
            })
            .WithName("UpdateEstadio")
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            ;

            app.MapDelete("/estadios/{id}", (/*int id,IEstadioService estadioService*/) =>
            {
                /*
                var deleted = await estadioService.Delete(id);

                if (!deleted)
                {
                    return Results.NotFound();
                }

                return Results.NoContent();
                */
            })
            .WithName("DeleteEstadio")
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            ;
        }
    }
}
