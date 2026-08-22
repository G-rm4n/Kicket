namespace WebApi.EndPoints
{
    public static class EventoEndPoints
    {
        public static void MapEventoEndPoints(this WebApplication app)
        {
            app.MapGet("/eventos", async (/*IEventoService eventoService*/) =>
            {
                /*
                 * var eventos=await eventoService.Getalleventos();
                 */
            })
            .WithName("GetAllEventos");
            /*.Produces<List<EventoDTO>>(Statuscode.Status200OK)*/;

            app.MapGet("/eventos/{id}", async (/*int id,IEventoService eventoService*/) =>
            {
                /*
                 * EventoDTO evento=await EventoService.getEventoById(id);
                 * 
                 * if(evento ==null){
                 *      return Results.NotFound()
                 * };
                 * 
                 * return evento;
                 *
                 */
            })
            .WithName("GetEvento")
            /*.Produces<EventoDTO>(Statuscode.Status200OK)
             */.Produces(StatusCodes.Status404NotFound)
            ;

            app.MapPost("/eventos", async (/*EventoDTO dto,IEventoService eventoService*/) =>
            {
                /*
                EventoDTO evento = await eventoService.AddAsync(dto);
                return Results.Created($"/eventos/{evento.Id}", evento);
                */

            })
            .WithName("AddEvento")
            /*Produces<eventoDTO>(StatusCodes.Status201Created)*/
            .Produces(StatusCodes.Status400BadRequest);

            app.MapPut("/eventos",(/*EventoDTO dto,IEventoService eventoService*/) =>
            {   
                /*
                var found = await eventoService.Update(dto);

                if (!found)
                {
                    return Results.NotFound();
                }

                return Results.NoContent();
                */
            })
            .WithName("UpdateEvento")
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            ;

            app.MapDelete("/eventos/{id}", (/*int id,IEventoService eventoService*/) =>
            {
                /*
                var deleted = await eventoService.Delete(id);

                if (!deleted)
                {
                    return Results.NotFound();
                }

                return Results.NoContent();
                */
            })
            .WithName("DeleteEvento")
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            ;
        }
    }
}
