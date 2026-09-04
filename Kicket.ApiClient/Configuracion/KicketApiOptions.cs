namespace Kicket.ApiClient.Configuracion
{
    /// <summary>Configuracion de la capa cliente. Se completa desde appsettings del proyecto de escritorio.</summary>
    public class KicketApiOptions
    {
        /// <summary>Nombre de la seccion en appsettings.json.</summary>
        public const string SeccionConfig = "KicketApi";

        /// <summary>URL base de la API. Debe terminar en "/".</summary>
        public string BaseUrl { get; set; } = "https://localhost:7000/";

        /// <summary>Segundos de espera antes de cortar una request.</summary>
        public int TimeoutSegundos { get; set; } = 30;
    }
}
