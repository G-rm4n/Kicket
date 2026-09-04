using Core.Interfaces;
using Domain.Entities;
using Kicket.Contracts.Usuarios;

namespace WebApi.EndPoints
{
    public static class UsuarioEndPoints
    {
        public static void MapUsuarioEndPoints(this WebApplication app)
        {
            app.MapGet("/usuarios", async (IUsuarioService usuarioService) =>
            {
                
                var usuarios = await usuarioService.ObtenerTodosAsync();

                IEnumerable<UsuarioDto> dtos = usuarios.Select(u => new UsuarioDto()
                {
                    Email = u.Email,
                    Apellido = u.Apellido,
                    Nombre = u.Nombre,
                    IdUsuario = u.IdUsuario,
                    Rol = u.Rol
                }).ToList();

                return Results.Ok(dtos);
                 
            })
            .WithName("GetAllUsuarios")
            .Produces<IEnumerable<UsuarioDto>>(StatusCodes.Status200OK);

            app.MapGet("/usuarios/{id}", async (int id, IUsuarioService usuarioService) =>
            {
                
                Usuario? usuario = await usuarioService.ObtenerPorIdAsync(id);
                  
                if(usuario == null){
                    return Results.NotFound();
                }

                UsuarioDto dto = new()
                {
                    Email = usuario.Email,
                    Apellido = usuario.Apellido,
                    Nombre = usuario.Nombre,
                    IdUsuario = usuario.IdUsuario,
                    Rol = usuario.Rol
                };

                return Results.Ok(dto);
                 
            })
            .WithName("GetUsuario")
            .Produces<UsuarioDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

            app.MapPost("/usuarios", async (UsuarioRequest usuarioReq, IUsuarioService usuarioService) =>
            {

                Usuario usuario = new()
                {
                    Apellido = usuarioReq.Apellido,
                    Email = usuarioReq.Email,
                    Nombre = usuarioReq.Nombre,
                    Password = usuarioReq.Pass
                };
                Usuario newUsuario = await usuarioService.RegistrarUsuarioAsync(usuario);

                UsuarioDto dto = new()
                {
                    Email = newUsuario.Email,
                    Apellido = newUsuario.Apellido,
                    Nombre =newUsuario.Nombre,
                    IdUsuario = newUsuario.IdUsuario,
                    Rol = newUsuario.Rol
                };
                return Results.Created($"/usuarios/{dto.IdUsuario}", dto);
                
            })
            .WithName("AddUsuario")
            .Produces<UsuarioDto>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest);

            app.MapPut("/usuarios", async (UsuarioUpdateRequest usuarioReq, IUsuarioService usuarioService) =>
            {

                Usuario usuario = new()
                {
                    Apellido = usuarioReq.Apellido,
                    Email = usuarioReq.Email,
                    Nombre = usuarioReq.Nombre,
                    IdUsuario=usuarioReq.IdUsuario,
                    Password=usuarioReq.Pass,
                    Rol=usuarioReq.Rol

                };

                var found = await usuarioService.ActualizarUsuarioAsync(usuario);
                if (!found)
                {
                    return Results.NotFound();
                }
                return Results.NoContent();

            })
            .WithName("UpdateUsuario")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest);

            app.MapDelete("/usuarios/{id}",async (int id, IUsuarioService usuarioService) =>
            {
                
               var deleted = await usuarioService.EliminarUsuarioAsync(id);
               if (!deleted)
               {
                   return Results.NotFound();
               }
               return Results.NoContent();
               
            })
            .WithName("DeleteUsuario")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest);
        }
    }
}
