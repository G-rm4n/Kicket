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
        // Objeto para bloquear y que dos hilos no modifiquen la sesión a la vez.
        private readonly object _lock = new();

        // Datos de la sesión actual (solo se cambian desde esta clase).
        public string? Token { get; private set; }
        public UsuarioDto? Usuario { get; private set; }
        public DateTime? ExpiraEn { get; private set; }

        // Rol del usuario logueado (null si no hay sesión).
        public string? Rol => Usuario?.Rol;

        // Hay sesión si hay token y todavía no venció.
        public bool EstaAutenticado =>
            !string.IsNullOrWhiteSpace(Token) && ExpiraEn > DateTime.UtcNow;

        // Evento que avisa a los formularios cuando se inicia o cierra la sesión.
        public event EventHandler? SesionCambiada;

        // Inicia la sesión guardando token, usuario y vencimiento, y avisa con el evento.
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

        // Cierra la sesión borrando todos los datos, y avisa con el evento.
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

        // Compara el rol del usuario logueado con el pedido (sin importar mayúsculas).
        public bool TieneRol(string rol) =>
            EstaAutenticado && string.Equals(Rol, rol, StringComparison.OrdinalIgnoreCase);
    }
}
