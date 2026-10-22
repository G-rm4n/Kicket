using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Kiketpropio.Dominio.DTOs;

namespace Kiketpropio.Web.Servicios
{
    public class ApiClient
    {
        private static readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };

        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApiClient(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        private void AgregarToken()
        {
            var token = _httpContextAccessor.HttpContext?.User.FindFirst("Token")?.Value;
            if (string.IsNullOrWhiteSpace(token)) return;

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public async Task<LoginResponse?> Login(LoginRequest loginInput)
        {
            var respuesta = await _httpClient.PostAsJsonAsync("api/Auth/login", loginInput);
            if (!respuesta.IsSuccessStatusCode) return null;

            return await respuesta.Content.ReadFromJsonAsync<LoginResponse>(_jsonOptions);
        }

        public async Task<List<T>> GetAll<T>(string recurso)
        {
            AgregarToken();
            var respuesta = await _httpClient.GetAsync($"api/{recurso}");
            if (!respuesta.IsSuccessStatusCode) return new List<T>();

            var resultado = await respuesta.Content.ReadFromJsonAsync<List<T>>(_jsonOptions);
            return resultado ?? new List<T>();
        }

        public async Task<T?> GetOne<T>(string recurso, int id)
        {
            AgregarToken();
            var respuesta = await _httpClient.GetAsync($"api/{recurso}/{id}");
            if (!respuesta.IsSuccessStatusCode) return default;

            return await respuesta.Content.ReadFromJsonAsync<T>(_jsonOptions);
        }

        public async Task<bool> Create<T>(string recurso, T entidadInput)
        {
            AgregarToken();
            var respuesta = await _httpClient.PostAsJsonAsync($"api/{recurso}", entidadInput);
            return respuesta.IsSuccessStatusCode;
        }

        public async Task<bool> Update<T>(string recurso, int id, T entidadInput)
        {
            AgregarToken();
            var respuesta = await _httpClient.PutAsJsonAsync($"api/{recurso}/{id}", entidadInput);
            return respuesta.IsSuccessStatusCode;
        }

        public async Task<bool> Delete(string recurso, int id)
        {
            AgregarToken();
            var respuesta = await _httpClient.DeleteAsync($"api/{recurso}/{id}");
            return respuesta.IsSuccessStatusCode;
        }
    }
}
