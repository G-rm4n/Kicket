using System.Net.Http.Headers;
using Kicket.ApiClient.Abstracciones;

namespace Kicket.Blazor.Core.Interceptors
{
    public class TokenInsertor:DelegatingHandler
    {
        private readonly ISesionUsuario _sesion;

        public TokenInsertor(ISesionUsuario sesion)
        {
            _sesion = sesion;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {

            if (!string.IsNullOrEmpty(_sesion.Token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _sesion.Token);
            }

            var response = await base.SendAsync(request, cancellationToken);

            return response;
        }
    }
}
