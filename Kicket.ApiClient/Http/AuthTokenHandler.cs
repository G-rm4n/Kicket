using System.Net;
using System.Net.Http.Headers;
using Kicket.ApiClient.Abstracciones;

namespace Kicket.ApiClient.Http
{
    /// <summary>
    /// Se engancha en el pipeline de HttpClient y le pega el token a cada request.
    /// Gracias a esto ningun cliente concreto tiene que acordarse de mandar el header.
    /// Si la API contesta 401, cierra la sesion local para que la UI reaccione.
    /// </summary>
    public class AuthTokenHandler : DelegatingHandler
    {
        // Sesión de donde se saca el token.
        private readonly ISesionUsuario _sesion;

        // Recibe la sesión por inyección de dependencias.
        public AuthTokenHandler(ISesionUsuario sesion)
        {
            _sesion = sesion;
        }

        // Se ejecuta en cada request antes de salir hacia la API.
        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Si hay sesión activa, agrega el header "Authorization: Bearer {token}".
            if (_sesion.EstaAutenticado)
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _sesion.Token);

            // Deja pasar la request al siguiente handler (el que realmente la envía).
            var respuesta = await base.SendAsync(request, cancellationToken);

            // El token vencio o fue revocado: no tiene sentido seguir con una sesion muerta.
            if (respuesta.StatusCode == HttpStatusCode.Unauthorized && _sesion.EstaAutenticado)
                _sesion.Cerrar();

            return respuesta;
        }
    }
}
