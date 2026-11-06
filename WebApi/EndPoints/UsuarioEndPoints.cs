using Core.Interfaces;
using Domain.Entities;
using Kicket.Contracts.Common;
using Kicket.Contracts.Usuarios;

namespace WebApi.EndPoints
{
    // Endpoints REST de usuarios. El grupo exige login y cada ruta además exige rol Admin.
    public static class UsuarioEndPoints
    {
        // Registra todas las rutas /usuarios.
        public static void MapUsuarioEndPoints(this WebApplication app)
        {
            // Prefijo /usuarios y autorización obligatoria.
            var usuariosGroup = app.MapGroup("/usuarios")
                                   .RequireAuthorization();

            // GET /usuarios: lista todos los usuarios como DTO (sin contraseña).
            usuariosGroup.MapGet("/", async (IUsuarioService usuarioService) =>
            {
                var usuarios = await usuarioService.ObtenerTodosAsync();

                IEnumerable<UsuarioDto> dtos = usuarios.Select(u => u.ADto()).ToList();

                return Results.Ok(dtos);
            })
            .WithName("GetAllUsuarios")
            .Produces<IEnumerable<UsuarioDto>>(StatusCodes.Status200OK)
            .RequireAuthorization("Solo admin");

            // GET /usuarios/paginado: página de usuarios con filtros (nombre, apellido, email, DNI, rol). Solo Admin:
            // un usuario común no puede ver DNI ajenos.
            usuariosGroup.MapGet("/paginado", async ([AsParameters] UsuarioFiltro filtro, IUsuarioService usuarioService, CancellationToken ct) =>
                Results.Ok(await usuarioService.ObtenerPaginadoAsync(filtro, ct)))
            .WithName("GetUsuariosPaginado")
            .Produces<PagedResult<UsuarioDto>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .RequireAuthorization("Solo admin");

            // GET /usuarios/{id}: devuelve un usuario o 404.
            usuariosGroup.MapGet("/{id}", async (int id, IUsuarioService usuarioService) =>
            {
                Usuario? usuario = await usuarioService.ObtenerPorIdAsync(id);

                if (usuario == null)
                {
                    return Results.NotFound();
                }

                UsuarioDto dto = usuario.ADto();

                return Results.Ok(dto);
            })
            .WithName("GetUsuario")
            .Produces<UsuarioDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .RequireAuthorization("Solo admin");

            // POST /usuarios: crea un usuario (solo Admin); si no viene rol usa "Usuario". Responde 201.
            usuariosGroup.MapPost("/", async (UsuarioRequest usuarioReq, IUsuarioService usuarioService) =>
            {
                Usuario usuario = new()
                {
                    Apellido = usuarioReq.Apellido,
                    Email = usuarioReq.Email,
                    Nombre = usuarioReq.Nombre,
                    Password = usuarioReq.Pass,
                    Rol = string.IsNullOrWhiteSpace(usuarioReq.Rol) ? Roles.Usuario : usuarioReq.Rol,
                    Dni = usuarioReq.Dni,
                    Telefono = usuarioReq.Telefono,
                    FechaNacimiento = usuarioReq.FechaNacimiento
                };
                // El servicio valida, controla email repetido (409) y hashea la contraseña.
                Usuario newUsuario = await usuarioService.RegistrarUsuarioAsync(usuario);

                UsuarioDto dto = newUsuario.ADto();
                return Results.Created($"/usuarios/{dto.IdUsuario}", dto);
            })
            .WithName("AddUsuario")
            .Produces<UsuarioDto>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .RequireAuthorization("Solo admin");

            // PUT /usuarios: modifica un usuario (solo Admin). Si la contraseña o el rol vienen vacíos, conserva los actuales.
            usuariosGroup.MapPut("/", async (UsuarioUpdateRequest usuarioReq, IUsuarioService usuarioService) =>
            {
                // Busca el usuario actual para poder conservar contraseña y rol.
                var existente = await usuarioService.ObtenerPorIdAsync(usuarioReq.IdUsuario);
                if (existente is null)
                {
                    return Results.NotFound();
                }

                Usuario usuario = new()
                {
                    Apellido = usuarioReq.Apellido,
                    Email = usuarioReq.Email,
                    Nombre = usuarioReq.Nombre,
                    IdUsuario = usuarioReq.IdUsuario,
                    Password = string.IsNullOrWhiteSpace(usuarioReq.Pass) ? existente.Password : usuarioReq.Pass,
                    Rol = string.IsNullOrWhiteSpace(usuarioReq.Rol) ? existente.Rol : usuarioReq.Rol,
                    // DNI vacío = se conserva el actual (no se borra ni se inventa).
                    Dni = string.IsNullOrWhiteSpace(usuarioReq.Dni) ? existente.Dni : usuarioReq.Dni,
                    Telefono = usuarioReq.Telefono,
                    FechaNacimiento = usuarioReq.FechaNacimiento
                };

                // Actualiza; 204 si sale bien, 404 si no existe.
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
            .Produces(StatusCodes.Status400BadRequest)
            .RequireAuthorization("Solo admin");

            // DELETE /usuarios/{id}: borra un usuario (solo Admin). 204 o 404.
            usuariosGroup.MapDelete("/{id}", async (int id, IUsuarioService usuarioService) =>
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
            .Produces(StatusCodes.Status400BadRequest)
            .RequireAuthorization("Solo admin");
        }
    }
}
