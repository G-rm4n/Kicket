using Kicket.ApiClient.Abstracciones;
using Kicket.ApiClient.Http;
using Kicket.Contracts.Auth;
using Kicket.Contracts.Usuarios;

namespace Kicket.ApiClient.Clientes
{
    /// <summary>
    /// Unico punto donde se abre y se cierra la sesion. Guarda el resultado del login
    /// en ISesionUsuario; a partir de ahi AuthTokenHandler se encarga del resto.
    /// </summary>
    public class AuthApiClient : ApiClientBase, IAuthApiClient
    {
        // Prefijo de las rutas de autenticación en la API.
        private const string Ruta = "auth";

        private readonly ISesionUsuario _sesion;

        // Recibe el HttpClient (ya configurado) y la sesión por inyección de dependencias.
        public AuthApiClient(HttpClient http, ISesionUsuario sesion) : base(http)
        {
            _sesion = sesion;
        }

        // Hace POST /auth/login y, si sale bien, guarda token y usuario en la sesión.
        public async Task<LoginResponse> LoginAsync(LoginRequest request, CancellationToken ct = default)
        {
            // Si habia una sesion previa, se descarta antes de pedir la nueva.
            _sesion.Cerrar();

            var respuesta = await PostAsync<LoginResponse>($"{Ruta}/login", request, ct);
            _sesion.Iniciar(respuesta);

            return respuesta;
        }

        // Hace POST /auth/register y devuelve el mensaje de la API.
        public async Task<RegisterResponse> RegistAsync(UsuarioRequest request, CancellationToken ct = default)
        {
            var respuesta= await PostAsync<RegisterResponse>($"{Ruta}/register", request, ct);
            return respuesta;
        }

        // Cierra la sesión local (borra el token).
        public void Logout() => _sesion.Cerrar();
    }
}
