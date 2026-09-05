using Core.Interfaces;
using Core.Services;
using Kicket.Contracts.Auth;

namespace WebApi.EndPoints
{
    public static class AuthEndPoints
    {
        public static void MapAuthEndPoints(this WebApplication app)
        {
            app.MapPost("auth/login", async (LoginRequest loginReq,IAuthService authService) =>
            {
                var result = await authService.Login(loginReq.Email, loginReq.Pass);
              
                if (result is null) return Results.Unauthorized();

                var (token, FechaExpiracion) = result.Value;

                return Results.Ok(new LoginResponse() { ExpiraEn = FechaExpiracion, Token = token });
            });
        }
    }
}
