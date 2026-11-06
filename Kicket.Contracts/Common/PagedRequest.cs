namespace Kicket.Contracts.Common
{
    /// <summary>
    /// Parametros comunes a todos los listados paginados: pagina, tamaño, texto de busqueda y orden.
    /// Viaja por query string (GET /clubes/paginado?pagina=2&amp;tamanoPagina=25&amp;busqueda=rosario).
    /// Cada listado hereda de esta clase y agrega sus propios filtros.
    /// </summary>
    public class PagedRequest
    {
        // Tamaños de pagina que ofrecen las pantallas.
        public static readonly int[] TamanosSugeridos = { 10, 25, 50 };
        // Tope para que nadie pida la tabla entera de una vez.
        public const int TamanoMaximo = 100;

        // Los tres son anulables para que sean OPCIONALES en la query string (Minimal APIs exige los no anulables).
        // Normalizar() completa los valores por defecto.
        // Numero de pagina, empezando en 1.
        public int? Pagina { get; set; } = 1;
        // Cantidad de registros por pagina.
        public int? TamanoPagina { get; set; } = 10;
        // Texto libre; cada listado decide sobre que columnas busca.
        public string? Busqueda { get; set; }
        // Columna de orden; cada listado tiene su lista blanca de valores validos.
        public string? OrdenarPor { get; set; }
        // true = orden descendente.
        public bool? Descendente { get; set; }

        // Valores ya resueltos (solo lectura, no viajan en la query).
        public int PaginaActual => Pagina is > 0 ? Pagina.Value : 1;
        public int Tamano => TamanoPagina is > 0 ? Math.Min(TamanoPagina.Value, TamanoMaximo) : 10;
        public bool EsDescendente => Descendente ?? false;

        /// <summary>Valores aceptados en OrdenarPor. Vacio = el listado no permite elegir orden.</summary>
        public virtual IReadOnlyCollection<string> OrdenesPermitidos => Array.Empty<string>();

        /// <summary>Devuelve los errores de los parametros. Lista vacia = parametros validos.</summary>
        public virtual List<string> Validar()
        {
            var errores = new List<string>();
            if (Pagina < 1)
                errores.Add("La pagina debe ser mayor o igual a 1.");
            if (TamanoPagina < 1 || TamanoPagina > TamanoMaximo)
                errores.Add($"El tamaño de pagina debe estar entre 1 y {TamanoMaximo}.");
            if (!string.IsNullOrWhiteSpace(OrdenarPor)
                && !OrdenesPermitidos.Contains(OrdenarPor, StringComparer.OrdinalIgnoreCase))
                errores.Add($"No se puede ordenar por '{OrdenarPor}'. Valores validos: {string.Join(", ", OrdenesPermitidos)}.");
            if (Busqueda is { Length: > 100 })
                errores.Add("La busqueda no puede superar los 100 caracteres.");
            return errores;
        }

        /// <summary>Deja la busqueda sin espacios sobrantes (o null si quedo vacia) y el orden en minusculas.</summary>
        public void Normalizar()
        {
            Pagina ??= 1;
            TamanoPagina ??= 10;
            Descendente ??= false;
            Busqueda = string.IsNullOrWhiteSpace(Busqueda) ? null : Busqueda.Trim();
            OrdenarPor = string.IsNullOrWhiteSpace(OrdenarPor) ? null : OrdenarPor.Trim().ToLowerInvariant();
        }

        // Ayuda para que las clases hijas validen un rango de fechas.
        protected static void ValidarRango(List<string> errores, DateTime? desde, DateTime? hasta, string nombre)
        {
            if (desde.HasValue && hasta.HasValue && desde.Value.Date > hasta.Value.Date)
                errores.Add($"La fecha 'desde' de {nombre} no puede ser posterior a la fecha 'hasta'.");
        }
    }
}
