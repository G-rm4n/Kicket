using Core.Interfaces;
using Core.Services;
using Kicket.Contracts.Auth;
using Kicket.Contracts.Usuarios;

namespace WebApi.EndPoints
{
    public static class AuthEndPoints
    {
        public static void MapAuthEndPoints(this WebApplication app)
        {
            app.MapPost("auth/login", async (LoginRequest loginReq, IAuthService authService) =>
            {
                var result = await authService.Login(loginReq.Email, loginReq.Pass);

                if (result is null) return Results.Unauthorized();

                var (token, fechaExpiracion, usuario) = result.Value;

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
        }
    }
}