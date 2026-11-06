using Kicket.Contracts.Common;

namespace Kicket.Contracts.Clubes
{
    /// <summary>Filtros de GET /clubes/paginado. Busqueda: nombre, abreviatura o descripcion.</summary>
    public class ClubFiltro : PagedRequest
    {
        // Ciudad exacta o parcial del club.
        public string? Ciudad { get; set; }

        public override IReadOnlyCollection<string> OrdenesPermitidos => new[] { "id", "nombre", "abreviatura", "ciudad", "fundacion" };
    }
}
