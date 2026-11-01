using Core.Interfaces;
using Core.Services;
using Domain.Entities;
using Kicket.Contracts.Auth;
using Kicket.Contracts.Usuarios;
using System.Net;

namespace WebApi.EndPoints
{
    // Endpoints de autenticación (login y registro). Son públicos: no piden token.
    public static class AuthEndPoints
    {
        // Método de extensión que registra las rutas /auth en la aplicación.
        public static void MapAuthEndPoints(this WebApplication app)
        {
            // POST /auth/login: valida credenciales y devuelve token JWT + datos del usuario.
            app.MapPost("auth/login", async (LoginRequest loginReq, IAuthService authService) =>
            {
                // Le pide al servicio que valide el email y la contraseña.
                var result = await authService.Login(loginReq.Email, loginReq.Pass);

                // Si las credenciales no coinciden responde 401.
                if (result is null)
                    return Results.Problem(detail: "Credenciales incorrectas", statusCode: (int)HttpStatusCode.Unauthorized);

                // Desarma la tupla devuelta por el servicio.
                var (token, fechaExpiracion, usuario) = result.Value;

                // Responde 200 con el token, su vencimiento y el usuario como DTO (sin contraseña).
                return Results.Ok(new LoginResponse()
                {
                    Token = token,
                    ExpiraEn = fechaExpiracion,
                    Usuario = new UsuarioDto
                    {
                        IdUsuario = usuario.IdUsuario,
                        Nombre = usuario.Nombre,
                        Apellido = usuario.Apellido,
                        Email = usuario.Email,
                        Rol = usuario.Rol
                    }
                });
            });

            // POST /auth/register: registra un usuario nuevo desde la pantalla pública de registro.
            app.MapPost("auth/register", async (UsuarioRequest RegReq, IAuthService authService) =>
            {
                // Convierte el request en la entidad Usuario.
                var usuario = new Usuario() { Email = RegReq.Email, Apellido = RegReq.Apellido, Nombre = RegReq.Nombre, Password = RegReq.Pass };

                // El servicio valida email repetido, hashea la contraseña y lo guarda.
                await authService.RegistrarUsuario(usuario);

                // Responde 200 con un mensaje de confirmación.
                return Results.Ok(new { Mensaje = "Usuario creado correctamente" });
            });
        }
    }
}