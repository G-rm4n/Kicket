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
        bool EstaAutenticado { get; }
        string? Token { get; }
        UsuarioDto? Usuario { get; }
        string? Rol { get; }
        DateTime? ExpiraEn { get; }

        /// <summary>Se dispara al iniciar y al cerrar sesion, para que los formularios se refresquen.</summary>
        event EventHandler? SesionCambiada;

        void Iniciar(LoginResponse respuesta);
        void Cerrar();
        bool TieneRol(string rol);
    }
}
