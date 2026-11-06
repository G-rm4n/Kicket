namespace Kicket.Contracts.Common
{
    /// <summary>
    /// Una pagina de resultados mas los datos para dibujar el paginador
    /// (pagina actual, tamaño, total de registros y total de paginas).
    /// </summary>
    public class PagedResult<T>
    {
        // Registros de la pagina pedida.
        public List<T> Items { get; set; } = new();
        // Pagina devuelta (puede ser menor a la pedida si los filtros dejaron menos paginas).
        public int Pagina { get; set; } = 1;
        // Registros por pagina.
        public int TamanoPagina { get; set; } = 10;
        // Total de registros que cumplen los filtros (no solo los de esta pagina).
        public int TotalRegistros { get; set; }

        // Paginas totales; al menos 1 para que el paginador siempre muestre "Pagina 1 de 1".
        public int TotalPaginas => TamanoPagina <= 0 || TotalRegistros == 0
            ? 1
            : (int)Math.Ceiling(TotalRegistros / (double)TamanoPagina);
        public bool TieneAnterior => Pagina > 1;
        public bool TieneSiguiente => Pagina < TotalPaginas;

        /// <summary>Convierte los items (por ejemplo de entidad a DTO) conservando los datos de paginacion.</summary>
        public PagedResult<TOut> Map<TOut>(Func<T, TOut> convertir) => new()
        {
            Items = Items.Select(convertir).ToList(),
            Pagina = Pagina,
            TamanoPagina = TamanoPagina,
            TotalRegistros = TotalRegistros
        };
    }
}
