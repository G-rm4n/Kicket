using Kicket.Contracts.Usuarios;

namespace Kicket.Contracts.Auth
{
    /// <summary>Lo que devuelve la API cuando el login es correcto.</summary>
    public class LoginResponse
    {
        /// <summary>Token JWT que el cliente manda en el header Authorization.</summary>
        public string Token { get; set; } = string.Empty;

        /// <summary>Momento (UTC) en el que el token deja de servir.</summary>
        public DateTime ExpiraEn { get; set; }

        /// <summary>Datos del usuario logueado, para mostrarlos en la pantalla principal.</summary>
        public UsuarioDto Usuario { get; set; } = new();
    }
}
