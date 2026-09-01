using Kicket.Contracts.Auth;

namespace Kicket.ApiClient.Abstracciones
{
    /// <summary>Login y logout vistos desde la capa de escritorio.</summary>
    public interface IAuthApiClient
    {
        /// <summary>
        /// Valida las credenciales contra la API y, si son correctas, deja la sesion abierta.
        /// Lanza ApiException con NoAutenticado = true si el email o la contrasena no coinciden.
        /// </summary>
        Task<LoginResponse> LoginAsync(LoginRequest request, CancellationToken ct = default);

        /// <summary>Cierra la sesion local. El token JWT es sin estado, no hay nada que avisarle a la API.</summary>
        void Logout();
    }
}
