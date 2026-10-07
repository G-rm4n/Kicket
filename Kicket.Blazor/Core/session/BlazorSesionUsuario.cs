using Kicket.ApiClient.Abstracciones;
using Kicket.Contracts.Auth;
using Kicket.Contracts.Usuarios;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Reflection.Metadata;

namespace Kicket.Blazor.Core.session
{
    public class BlazorSesionUsuario : ISesionUsuario
    {
        private readonly ProtectedSessionStorage _sesionStorage;
        private readonly object _lock = new();
        private readonly string StoreKey = "Kicket_session";

        public string? Token { get; private set; }
        public UsuarioDto? Usuario { get; private set; }
        public DateTime? ExpiraEn { get; private set; }

        public string? Rol => Usuario?.Rol;

        public bool EstaAutenticado => !string.IsNullOrWhiteSpace(Token) && ExpiraEn > DateTime.Now;

        public event EventHandler? SesionCambiada;

        public BlazorSesionUsuario(ProtectedSessionStorage sessionStorage)
        {
            _sesionStorage = sessionStorage;
        }

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

        private async Task GuardarEnStorageAsync(LoginResponse response)
        {
            try
            {
                await _sesionStorage.SetAsync(StoreKey, response);
            }
            catch { }
        }

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

        public bool TieneRol(string rol) => EstaAutenticado && string.Equals(Usuario?.Rol, rol);

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
