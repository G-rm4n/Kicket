using Kicket.Contracts.Common;
using Microsoft.EntityFrameworkCore;

namespace Data.Consultas
{
    /// <summary>
    /// Paginacion del lado del servidor. Recibe una consulta YA filtrada y ordenada (IQueryable, todavia
    /// sin ejecutar) y la resuelve con dos consultas SQL: un COUNT para el total y un
    /// OFFSET/FETCH (Skip/Take) para traer solo los registros de la pagina.
    /// </summary>
    public static class Paginacion
    {
        public static async Task<PagedResult<T>> PaginarAsync<T>(
            this IQueryable<T> consultaOrdenada, PagedRequest pedido, CancellationToken ct = default)
        {
            int tamano = pedido.Tamano;

            // 1) Total de registros que cumplen los filtros (SELECT COUNT(*) ...).
            int total = await consultaOrdenada.CountAsync(ct);

            // 2) Si los filtros dejaron menos paginas que la pedida, se devuelve la ultima pagina valida
            //    en vez de una pagina vacia.
            int totalPaginas = Math.Max(1, (int)Math.Ceiling(total / (double)tamano));
            int pagina = Math.Min(pedido.PaginaActual, totalPaginas);

            // 3) Solo la pagina pedida (ORDER BY ... OFFSET x ROWS FETCH NEXT y ROWS ONLY).
            var items = await consultaOrdenada
                .Skip((pagina - 1) * tamano)
                .Take(tamano)
                .ToListAsync(ct);

            return new PagedResult<T>
            {
                Items = items,
                Pagina = pagina,
                TamanoPagina = tamano,
                TotalRegistros = total
            };
        }

        /// <summary>Atajo para aplicar OrderBy u OrderByDescending segun un bool.</summary>
        public static IOrderedQueryable<T> OrdenarPor<T, TKey>(
            this IQueryable<T> consulta, System.Linq.Expressions.Expression<Func<T, TKey>> clave, bool descendente) =>
            descendente ? consulta.OrderByDescending(clave) : consulta.OrderBy(clave);

        /// <summary>Atajo para el criterio secundario (ThenBy / ThenByDescending).</summary>
        public static IOrderedQueryable<T> LuegoPor<T, TKey>(
            this IOrderedQueryable<T> consulta, System.Linq.Expressions.Expression<Func<T, TKey>> clave, bool descendente) =>
            descendente ? consulta.ThenByDescending(clave) : consulta.ThenBy(clave);
    }
}
