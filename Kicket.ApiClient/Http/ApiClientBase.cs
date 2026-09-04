using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Kicket.Contracts.Common;

namespace Kicket.ApiClient.Http
{
    /// <summary>
    /// Base de todos los clientes concretos. Concentra en un solo lugar el serializado,
    /// la lectura de la respuesta y la traduccion de cualquier fallo a ApiException.
    /// Los clientes que heredan quedan reducidos a declarar rutas.
    /// </summary>
    public abstract class ApiClientBase
    {
        protected readonly HttpClient Http;

        protected static readonly JsonSerializerOptions JsonOpciones = new()
        {
            PropertyNameCaseInsensitive = true
        };

        protected ApiClientBase(HttpClient http)
        {
            Http = http;
        }

        /// <summary>GET que espera un cuerpo JSON de vuelta.</summary>
        protected Task<T> GetAsync<T>(string ruta, CancellationToken ct = default) =>
            LeerCuerpoAsync<T>(() => new HttpRequestMessage(HttpMethod.Get, ruta), ct);

        /// <summary>POST que devuelve el recurso creado (201 con cuerpo).</summary>
        protected Task<T> PostAsync<T>(string ruta, object cuerpo, CancellationToken ct = default) =>
            LeerCuerpoAsync<T>(() => Crear(HttpMethod.Post, ruta, cuerpo), ct);

        /// <summary>
        /// PUT sin cuerpo de respuesta: los endpoints del equipo contestan 204 en el update.
        /// El id del recurso viaja dentro del cuerpo, no en la ruta.
        /// </summary>
        protected Task SinCuerpoAsync(string ruta, object cuerpo, CancellationToken ct = default) =>
            SinCuerpoAsync(() => Crear(HttpMethod.Put, ruta, cuerpo), ct);

        /// <summary>DELETE: tambien contesta 204.</summary>
        protected Task DeleteAsync(string ruta, CancellationToken ct = default) =>
            SinCuerpoAsync(() => new HttpRequestMessage(HttpMethod.Delete, ruta), ct);

        private static HttpRequestMessage Crear(HttpMethod metodo, string ruta, object cuerpo) =>
            new(metodo, ruta) { Content = JsonContent.Create(cuerpo, options: JsonOpciones) };

        private async Task<T> LeerCuerpoAsync<T>(Func<HttpRequestMessage> fabrica, CancellationToken ct)
        {
            using var respuesta = await EjecutarAsync(fabrica, ct);
            await GarantizarExitoAsync(respuesta, ct);

            var resultado = await respuesta.Content.ReadFromJsonAsync<T>(JsonOpciones, ct);

            if (resultado is null)
                throw new ApiException(
                    respuesta.StatusCode,
                    "La API respondio correctamente pero sin datos. Revisa que el endpoint este implementado.");

            return resultado;
        }

        private async Task SinCuerpoAsync(Func<HttpRequestMessage> fabrica, CancellationToken ct)
        {
            using var respuesta = await EjecutarAsync(fabrica, ct);
            await GarantizarExitoAsync(respuesta, ct);
        }

        /// <summary>Manda la request y convierte los fallos de red en ApiException.</summary>
        private async Task<HttpResponseMessage> EjecutarAsync(
            Func<HttpRequestMessage> fabrica, CancellationToken ct)
        {
            try
            {
                using var request = fabrica();
                return await Http.SendAsync(request, ct);
            }
            catch (TaskCanceledException ex) when (!ct.IsCancellationRequested)
            {
                throw new ApiException(
                    HttpStatusCode.RequestTimeout,
                    "La API tardo demasiado en responder. Puede estar detenida o la URL ser incorrecta.",
                    esFalloDeConexion: true,
                    inner: ex);
            }
            catch (HttpRequestException ex)
            {
                throw new ApiException(
                    HttpStatusCode.ServiceUnavailable,
                    "No se pudo conectar con la API. Verifica que este iniciada y que la URL sea correcta.",
                    esFalloDeConexion: true,
                    inner: ex);
            }
        }

        /// <summary>Si el estado no es 2xx, arma la ApiException con el detalle que mando la API.</summary>
        private static async Task GarantizarExitoAsync(HttpResponseMessage respuesta, CancellationToken ct)
        {
            if (respuesta.IsSuccessStatusCode) return;

            var cuerpo = await respuesta.Content.ReadAsStringAsync(ct);
            ApiError? error = null;

            if (!string.IsNullOrWhiteSpace(cuerpo))
            {
                // La API puede responder ApiError, un ProblemDetails de ASP.NET o un string suelto.
                try { error = JsonSerializer.Deserialize<ApiError>(cuerpo, JsonOpciones); }
                catch (JsonException) { /* se usa el cuerpo crudo como mensaje */ }
            }

            throw ApiException.DesdeRespuesta(respuesta.StatusCode, error, cuerpo);
        }
    }
}
