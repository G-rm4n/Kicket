using Kicket.Contracts.Common;

namespace Kicket.Contracts.Sectores
{
    /// <summary>Filtros de GET /sectores/paginado. Busqueda: nombre del sector.</summary>
    public class SectorFiltro : PagedRequest
    {
        // Tipos de sector. No es una columna: se deduce del nombre (si contiene "Platea" es numerado).
        public const string TipoPlatea = "Platea";
        public const string TipoPopular = "Popular";

        // Solo los sectores de este estadio.
        public int? EstadioId { get; set; }
        // "Platea" o "Popular".
        public string? Tipo { get; set; }
        // Capacidad minima del sector.
        public int? CapacidadMinima { get; set; }

        public override IReadOnlyCollection<string> OrdenesPermitidos => new[] { "id", "nombre", "capacidad", "precio" };

        public override List<string> Validar()
        {
            var errores = base.Validar();
            if (!string.IsNullOrWhiteSpace(Tipo)
                && !string.Equals(Tipo, TipoPlatea, StringComparison.OrdinalIgnoreCase)
                && !string.Equals(Tipo, TipoPopular, StringComparison.OrdinalIgnoreCase))
                errores.Add("El tipo de sector debe ser 'Platea' o 'Popular'.");
            if (CapacidadMinima < 0)
                errores.Add("La capacidad minima no puede ser negativa.");
            return errores;
        }
    }
}
