using Kicket.ApiClient.Abstracciones;
using Kicket.Contracts.Auth;
using Kicket.Contracts.Usuarios;

namespace Kicket.ApiClient.Sesion
{
    /// <summary>
    /// Implementacion en memoria de la sesion. Se registra como singleton: hay una sola
    /// por proceso, igual que hay un solo usuario sentado frente a la aplicacion.
    /// </summary>
    public class SesionUsuario : ISesionUsuario
    {
        private readonly object _lock = new();

        public string? Token { get; private set; }
        public UsuarioDto? Usuario { get; private set; }
        public DateTime? ExpiraEn { get; private set; }

        public string? Rol => Usuario?.Rol;

        public bool EstaAutenticado =>
            !string.IsNullOrWhiteSpace(Token) && ExpiraEn > DateTime.UtcNow;

        public event EventHandler? SesionCambiada;

        public void Iniciar(LoginResponse respuesta)
        {
            ArgumentNullException.ThrowIfNull(respuesta);

            lock (_lock)
            {
                Token = respuesta.Token;
                Usuario = respuesta.Usuario;
                ExpiraEn = respuesta.ExpiraEn;
            }

            SesionCambiada?.Invoke(this, EventArgs.Empty);
        }

        public void Cerrar()
        {
            lock (_lock)
            {
                Token = null;
                Usuario = null;
                ExpiraEn = null;
            }

            SesionCambiada?.Invoke(this, EventArgs.Empty);
        }

        public bool TieneRol(string rol) =>
            EstaAutenticado && string.Equals(Rol, rol, StringComparison.OrdinalIgnoreCase);
    }
}
