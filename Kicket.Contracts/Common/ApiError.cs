namespace Kicket.Contracts.Common
{
    /// <summary>
    /// Forma unica en la que la API devuelve un error. La capa cliente la deserializa
    /// para poder mostrarle al usuario un mensaje entendible en vez de un codigo HTTP.
    /// </summary>
    public class ApiError
    {
        public int Status { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Detail { get; set; }

        /// <summary>Errores de validacion por campo. Clave = nombre de la propiedad.</summary>
        public Dictionary<string, string[]>? Errors { get; set; }
    }
}
