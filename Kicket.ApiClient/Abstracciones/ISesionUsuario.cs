using Kicket.Contracts.Auth;
using Kicket.Contracts.Usuarios;

namespace Kicket.ApiClient.Abstracciones
{
    /// <summary>
    /// Guarda quien esta logueado durante la vida de la aplicacion de escritorio.
    /// Es el estado que hace que "logout" signifique algo: cerrarla borra el token,
    /// asi que las requests siguientes salen sin credenciales y la API las rechaza.
    /// </summary>
    public interface ISesionUsuario
    {
        // true si hay token y todavía no venció.
        bool EstaAutenticado { get; }
        // Token JWT de la sesión actual.
        string? Token { get; }
        // Datos del usuario logueado.
        UsuarioDto? Usuario { get; }
        // Rol del usuario logueado.
        string? Rol { get; }
        // Fecha de vencimiento del token.
        DateTime? ExpiraEn { get; }

        /// <summary>Se dispara al iniciar y al cerrar sesion, para que los formularios se refresquen.</summary>
        event EventHandler? SesionCambiada;

        // Guarda los datos del login (abre la sesión).
        void Iniciar(LoginResponse respuesta);
        // Borra los datos de la sesión (logout).
        void Cerrar();
        // Indica si el usuario logueado tiene el rol indicado.
        bool TieneRol(string rol);
    }
}
