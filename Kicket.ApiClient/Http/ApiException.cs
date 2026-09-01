using System.Net;
using Kicket.Contracts.Common;

namespace Kicket.ApiClient.Http
{
    /// <summary>
    /// Excepcion unica que lanza la capa cliente. La capa de escritorio solo necesita
    /// atrapar esta y mostrar MensajeCompleto(): ya viene traducido a algo legible.
    /// </summary>
    public class ApiException : Exception
    {
        public HttpStatusCode StatusCode { get; }

        /// <summary>Errores de validacion por campo, si la API los devolvio.</summary>
        public IReadOnlyDictionary<string, string[]> Errores { get; }

        /// <summary>True cuando la API no respondio (servicio caido, sin red, timeout).</summary>
        public bool EsFalloDeConexion { get; }

        public ApiException(
            HttpStatusCode statusCode,
            string mensaje,
            IReadOnlyDictionary<string, string[]>? errores = null,
            bool esFalloDeConexion = false,
            Exception? inner = null)
            : base(mensaje, inner)
        {
            StatusCode = statusCode;
            Errores = errores ?? new Dictionary<string, string[]>();
            EsFalloDeConexion = esFalloDeConexion;
        }

        public bool NoEncontrado => StatusCode == HttpStatusCode.NotFound;
        public bool NoAutenticado => StatusCode == HttpStatusCode.Unauthorized;
        public bool SinPermisos => StatusCode == HttpStatusCode.Forbidden;
        public bool HayErroresDeValidacion => Errores.Count > 0;

        /// <summary>Mensaje + detalle de cada campo invalido, listo para un MessageBox.</summary>
        public string MensajeCompleto()
        {
            if (!HayErroresDeValidacion) return Message;

            var detalle = Errores.SelectMany(e => e.Value).Distinct();
            return Message + Environment.NewLine + string.Join(Environment.NewLine, detalle.Select(d => "- " + d));
        }

        internal static ApiException DesdeRespuesta(HttpStatusCode status, ApiError? error, string? cuerpoCrudo)
        {
            var mensaje = error?.Detail
                          ?? error?.Title
                          ?? (string.IsNullOrWhiteSpace(cuerpoCrudo) ? MensajePorDefecto(status) : cuerpoCrudo!.Trim('"'));

            return new ApiException(status, mensaje, error?.Errors);
        }

        private static string MensajePorDefecto(HttpStatusCode status) => status switch
        {
            HttpStatusCode.Unauthorized => "Tu sesion no es valida o expiro. Volve a iniciar sesion.",
            HttpStatusCode.Forbidden => "No tenes permisos para realizar esta operacion.",
            HttpStatusCode.NotFound => "No se encontro el recurso solicitado.",
            HttpStatusCode.BadRequest => "Los datos enviados no son validos.",
            _ => $"La API respondio con el estado {(int)status} ({status})."
        };
    }
}
