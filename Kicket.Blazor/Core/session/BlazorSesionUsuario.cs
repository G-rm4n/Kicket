using Kicket.ApiClient.Abstracciones;
using Kicket.Contracts.Auth;
using Kicket.Contracts.Usuarios;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Reflection.Metadata;

namespace Kicket.Blazor.Core.session
{
    // Sesión del usuario para Blazor: la guarda en memoria y también en el ProtectedSessionStorage del navegador
    // (cifrado), para que sobreviva a una recarga de página.
    public class BlazorSesionUsuario : ISesionUsuario
    {
        // Storage protegido del navegador, un objeto para bloquear y la clave donde se guarda la sesión.
        private readonly ProtectedSessionStorage _sesionStorage;
        private readonly object _lock = new();
        private readonly string StoreKey = "Kicket_session";

        // Datos de la sesión actual.
        public string? Token { get; private set; }
        public UsuarioDto? Usuario { get; private set; }
        public DateTime? ExpiraEn { get; private set; }

        // Rol del usuario logueado.
        public string? Rol => Usuario?.Rol;

        // Hay sesión si hay token y todavía no venció.
        public bool EstaAutenticado => !string.IsNullOrWhiteSpace(Token) && ExpiraEn > DateTime.Now;

        // Evento que avisa cuando la sesión cambia.
        public event EventHandler? SesionCambiada;

        // Recibe el storage protegido por inyección de dependencias.
        public BlazorSesionUsuario(ProtectedSessionStorage sessionStorage)
        {
            _sesionStorage = sessionStorage;
        }

        // Inicia la sesión: guarda los datos en memoria, los persiste en el storage y avisa del cambio.
        public void Iniciar(LoginResponse response)
        {
            ArgumentNullException.ThrowIfNull(response);

            lock (_lock)
            {
                this.Token = response.Token;
                this.Usuario = response.Usuario;
                this.ExpiraEn = response.ExpiraEn;
            }

            _ = GuardarEnStorageAsync(response);

            SesionCambiada?.Invoke(this, EventArgs.Empty);
        }

        // Guarda la respuesta del login en el storage del navegador (si falla, lo ignora).
        private async Task GuardarEnStorageAsync(LoginResponse response)
        {
            try
            {
                await _sesionStorage.SetAsync(StoreKey, response);
            }
            catch { }
        }

        // Recupera la sesión desde el storage del navegador; si algo falla, borra lo guardado.
        public async Task CargarSession()
        {
            try
            {
                var resultado = await _sesionStorage.GetAsync<LoginResponse>(StoreKey);
                if (resultado.Success && resultado.Value is not null)
                {
                    lock (_lock)
                    {
                        this.Token = resultado.Value.Token;
                        this.Usuario = resultado.Value.Usuario;
                        this.ExpiraEn = resultado.Value.ExpiraEn;
                    }

                    this.SesionCambiada?.Invoke(this, EventArgs.Empty);
                }
            }
            catch {
               await _sesionStorage.DeleteAsync(StoreKey);
            }
        }

        // Indica si el usuario logueado tiene ese rol.
        public bool TieneRol(string rol) => EstaAutenticado && string.Equals(Usuario?.Rol, rol);

        // Cierra la sesión: borra memoria y storage, y avisa del cambio.
        public void Cerrar()
        {
            lock (_lock)
            {
                this.Usuario = null;
                this.ExpiraEn = null;
                this.Token = null;
            }

            _ = _sesionStorage.DeleteAsync(StoreKey);

            this.SesionCambiada?.Invoke(this, EventArgs.Empty);
        }
    }
}
