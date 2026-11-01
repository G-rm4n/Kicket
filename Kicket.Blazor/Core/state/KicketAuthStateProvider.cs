using Kicket.ApiClient.Abstracciones;
using Kicket.Blazor.Core.session;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace Kicket.Blazor.Core.state
{
    // Proveedor de autenticación propio: le dice a Blazor quién está logueado y con qué rol, a partir de la sesión.
    public class KicketAuthStateProvider:AuthenticationStateProvider
    {
        // Sesión del usuario (inyectada).
        private readonly ISesionUsuario _session;

        // Recibe la sesión y se suscribe a su evento para avisar a Blazor cada vez que cambia.
        public KicketAuthStateProvider(ISesionUsuario session)
        {
            _session = session;

            _session.SesionCambiada += (obj, arg) => NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        // Devuelve el usuario actual como ClaimsPrincipal (anónimo si no hay sesión).
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                
                // Si no hay sesión en memoria, intenta recuperarla del storage del navegador (por ejemplo, al recargar la página).
                if (!_session.EstaAutenticado)
                {
                    if (_session is BlazorSesionUsuario blazorSesion)
                    {
                        await blazorSesion.CargarSession();
                    }
                }
            }
            catch
            {
            }

            // Sin sesión: usuario anónimo.
            if (!_session.EstaAutenticado)
            {
                var anonimous = new ClaimsPrincipal(new ClaimsIdentity());
                return new AuthenticationState(anonimous);
            }

            // Con sesión: arma los claims con el nombre y el rol del usuario.
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name,_session.Usuario?.Nombre ?? "Usuario")
            };

            if (!string.IsNullOrEmpty(_session.Rol))
            {
                claims.Add(new Claim(ClaimTypes.Role, _session.Rol));
            }

            // Identidad autenticada con el tipo "KicketAuth".
            var identity = new ClaimsIdentity(claims, "KicketAuth");
            var user = new ClaimsPrincipal(identity);

            return new AuthenticationState(user);
        }

        // Fuerza a Blazor a volver a leer el estado de autenticación (después de login o logout).
        public void NotificarCambioDeEstado()
        {
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }
}
