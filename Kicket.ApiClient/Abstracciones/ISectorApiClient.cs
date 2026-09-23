using Kicket.Contracts.Sectores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kicket.ApiClient.Abstracciones
{
    public interface ISectorApiClient
    {
        Task<IReadOnlyList<SectorDto>> GetAllAsync(CancellationToken ct = default);
        Task<SectorDto> GetOneAsync(int id, CancellationToken ct = default);
        Task<SectorDto> CreateAsync(SectorRequest request, CancellationToken ct = default);
        Task UpdateAsync(SectorUpdateRequest request, CancellationToken ct = default);
        Task DeleteAsync(int id, CancellationToken ct = default);
    }
}
