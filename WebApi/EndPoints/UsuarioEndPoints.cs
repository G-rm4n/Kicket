namespace WebApi.EndPoints
{
    public static class UsuarioEndPoints
    {
        public static void MapUsuarioEndPoints(this WebApplication app)
        {
            app.MapGet("/usuarios", async (/*IUsuarioService usuarioService*/) =>
            {
                /*
                 * var usuarios = await usuarioService.GetAllUsuarios();
                 */
            })
            .WithName("GetAllUsuarios");

            app.MapGet("/usuarios/{id}", async (/*int id, IUsuarioService usuarioService*/) =>
            {
                /*
                 * UsuarioDTO usuario = await usuarioService.GetUsuarioById(id);
                 * 
                 * if(usuario == null){
                 *     return Results.NotFound();
                 * }
                 * 
                 * return usuario;
                 */
            })
            .WithName("GetUsuario")
            .Produces(StatusCodes.Status404NotFound);

            app.MapPost("/usuarios", async (/*UsuarioDTO dto, IUsuarioService usuarioService*/) =>
            {
                /*
                 * UsuarioDTO usuario = await usuarioService.AddAsync(dto);
                 * return Results.Created($"/usuarios/{usuario.Id}", usuario);
                 */
            })
            .WithName("AddUsuario")
            .Produces(StatusCodes.Status400BadRequest);

            app.MapPut("/usuarios", (/*UsuarioDTO dto, IUsuarioService usuarioService*/) =>
            {
                /*
                 * var found = await usuarioService.Update(dto);
                 * if (!found)
                 * {
                 *     return Results.NotFound();
                 * }
                 * return Results.NoContent();
                 */
            })
            .WithName("UpdateUsuario")
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest);

            app.MapDelete("/usuarios/{id}", (/*int id, IUsuarioService usuarioService*/) =>
            {
                /*
                 * var deleted = await usuarioService.Delete(id);
                 * if (!deleted)
                 * {
                 *     return Results.NotFound();
                 * }
                 * return Results.NoContent();
                 */
            })
            .WithName("DeleteUsuario")
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest);
        }
    }
}
