using Kicket.Contracts.Compras;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kicket.ApiClient.Abstracciones
/// <summary>
/// Operaciones de Compra expuestas a la capa de escritorio. Sin UpdateAsync a proposito:
/// la API no expone PUT /compras (ver la nota en CompraEndPoints).
/// </summary>
{
    public interface ICompraApiClient
    {
        Task<IReadOnlyList<CompraDto>> GetAllAsync(CancellationToken ct = default);
        Task<CompraDto> GetOneAsync(int id, CancellationToken ct = default);
        Task<CompraDto> CreateAsync(CompraRequest request, CancellationToken ct = default);
        Task DeleteAsync(int id, CancellationToken ct = default);
    }
}