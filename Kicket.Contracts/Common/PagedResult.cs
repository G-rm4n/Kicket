namespace Kicket.Contracts.Common
{
    /// <summary>
    /// Una pagina de resultados. Las listas que crecen sin techo (clientes, compras,
    /// eventos) viajan asi en vez de como lista suelta: ademas de los items trae el
    /// total de registros, que es lo que necesita la UI para dibujar el paginador.
    /// Sin TotalRegistros no se puede saber si hay una pagina siguiente.
    /// </summary>
    public class PagedResult<T>
    {
        /// <summary>Los registros de esta pagina. Nunca es null: si no hay datos, viene vacia.</summary>
        public List<T> Items { get; set; } = new();

        /// <summary>Numero de pagina devuelta. Arranca en 1, igual que PageRequest.</summary>
        public int Pagina { get; set; } = 1;

        /// <summary>Cantidad de registros por pagina que se pidio.</summary>
        public int TamanoPagina { get; set; } = PageRequest.TamanoPorDefecto;

        /// <summary>Total de registros que cumplen el filtro, contando todas las paginas.</summary>
        public int TotalRegistros { get; set; }

        /// <summary>Cantidad de paginas disponibles. Si no hay registros, es 0.</summary>
        public int TotalPaginas =>
            TamanoPagina <= 0 ? 0 : (int)Math.Ceiling(TotalRegistros / (double)TamanoPagina);

        public bool HayPaginaAnterior => Pagina > 1;

        public bool HayPaginaSiguiente => Pagina < TotalPaginas;

        /// <summary>Pagina vacia, para cuando la UI todavia no cargo nada.</summary>
        public static PagedResult<T> Vacia(PageRequest? pagina = null) => new()
        {
            Pagina = pagina?.Pagina ?? 1,
            TamanoPagina = pagina?.TamanoPagina ?? PageRequest.TamanoPorDefecto
        };
    }
}
