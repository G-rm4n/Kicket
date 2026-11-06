using Kicket.Contracts.Common;

namespace Kicket.Contracts.Estadios
{
    /// <summary>Filtros de GET /estadios/paginado. Busqueda: nombre o direccion.</summary>
    public class EstadioFiltro : PagedRequest
    {
        // Ciudad del estadio (busqueda parcial).
        public string? Ciudad { get; set; }
        // Provincia del estadio (busqueda parcial).
        public string? Provincia { get; set; }

        public override IReadOnlyCollection<string> OrdenesPermitidos => new[] { "id", "nombre", "ciudad", "provincia" };
    }
}
