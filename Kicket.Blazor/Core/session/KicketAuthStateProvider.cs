using Kicket.ApiClient.Abstracciones;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace Kicket.Blazor.Core.session
{
    public class KicketAuthStateProvider:AuthenticationStateProvider
    {
        private readonly ISesionUsuario _session;

        public KicketAuthStateProvider(ISesionUsuario session)
        {
            _session = session;

            _session.SesionCambiada += (obj, arg) => NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                
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

            if (!_session.EstaAutenticado)
            {
                var anonimous = new ClaimsPrincipal(new ClaimsIdentity());
                return new AuthenticationState(anonimous);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name,_session.Usuario?.Nombre ?? "Usuario")
            };

            if (!string.IsNullOrEmpty(_session.Rol))
            {
                claims.Add(new Claim(ClaimTypes.Role, _session.Rol));
            }

            var identity = new ClaimsIdentity(claims, "KicketAuth");
            var user = new ClaimsPrincipal(identity);

            return new AuthenticationState(user);
        }

        public void NotificarCambioDeEstado()
        {
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }
}
