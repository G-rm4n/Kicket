using Kicket.ApiClient.Abstracciones;
using Kicket.ApiClient.Http;
using Kicket.Contracts.Auth;

namespace Kicket.ApiClient.Clientes
{
    /// <summary>
    /// Unico punto donde se abre y se cierra la sesion. Guarda el resultado del login
    /// en ISesionUsuario; a partir de ahi AuthTokenHandler se encarga del resto.
    /// </summary>
    public class AuthApiClient : ApiClientBase, IAuthApiClient
    {
        private const string Ruta = "auth";

        private readonly ISesionUsuario _sesion;

        public AuthApiClient(HttpClient http, ISesionUsuario sesion) : base(http)
        {
            _sesion = sesion;
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest request, CancellationToken ct = default)
        {
            // Si habia una sesion previa, se descarta antes de pedir la nueva.
            _sesion.Cerrar();

            var respuesta = await PostAsync<LoginResponse>($"{Ruta}/login", request, ct);
            _sesion.Iniciar(respuesta);

            return respuesta;
        }

        public void Logout() => _sesion.Cerrar();
    }
}
