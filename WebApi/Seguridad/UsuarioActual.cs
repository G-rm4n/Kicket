using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace WebApi.Seguridad
{
    /// <summary>
    /// Lee del token JWT quien hace la request. La API usa esto en lugar del UsuarioId que manda
    /// el navegador: un id en el cuerpo o en la URL se puede cambiar a mano, la firma del token no.
    /// </summary>
    public static class UsuarioActual
    {
        // Id del usuario logueado (claim "sub"), o null si no hay token valido.
        public static int? ObtenerId(this ClaimsPrincipal usuario)
        {
            var valor = usuario.FindFirst(ClaimTypes.NameIdentifier)?.Value
                        ?? usuario.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
            return int.TryParse(valor, out var id) ? id : null;
        }

        // true si el token tiene rol Admin.
        public static bool EsAdmin(this ClaimsPrincipal usuario) => usuario.IsInRole("Admin");

        // true si el usuario logueado es el dueño del recurso o es Admin.
        public static bool PuedeVer(this ClaimsPrincipal usuario, int usuarioIdDelRecurso) =>
            usuario.EsAdmin() || usuario.ObtenerId() == usuarioIdDelRecurso;
    }
}
