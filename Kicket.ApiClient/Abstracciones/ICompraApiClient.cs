using Kicket.Contracts.Common;
using Kicket.Contracts.Compras;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Kicket.ApiClient.Abstracciones
{
    /// <summary>
    /// Operaciones de Compra expuestas a las capas clientes. Sin UpdateAsync a proposito:
    /// la API no expone PUT /compras (ver la nota en CompraEndPoints).
    /// </summary>
    public interface ICompraApiClient
    {
        // GET /compras: lista todas las compras.
        Task<IReadOnlyList<CompraDto>> GetAllAsync(CancellationToken ct = default);
        // GET .../paginado: una pagina con filtros y orden resueltos en el servidor.
        Task<PagedResult<CompraDto>> GetPaginadoAsync(CompraFiltro filtro, CancellationToken ct = default);
        // GET /compras/mis-entradas: partidos del usuario logueado (la API lo saca del token) con sus entradas.
        Task<PagedResult<PartidoConEntradasDto>> GetMisEntradasAsync(MisEntradasFiltro filtro, CancellationToken ct = default);
        // GET /compras/usuario/{id}: compras de un usuario.
        Task<IReadOnlyList<CompraDto>> GetByUsuarioIdAsync(int usuarioId, CancellationToken ct = default);
        // GET /compras/{id}: trae una compra.
        Task<CompraDto> GetOneAsync(int id, CancellationToken ct = default);
        // POST /compras: genera una compra.
        Task<CompraDto> CreateAsync(CompraRequest request, CancellationToken ct = default);
        // DELETE /compras/{id}: borra una compra.
        Task DeleteAsync(int id, CancellationToken ct = default);
    }
}
