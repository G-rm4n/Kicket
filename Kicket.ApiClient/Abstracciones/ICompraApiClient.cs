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
