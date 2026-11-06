using Kicket.Contracts.Common;
using Kicket.Contracts.Sectores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kicket.ApiClient.Abstracciones
{
    // Operaciones de Sector expuestas a las capas clientes.
    public interface ISectorApiClient
    {
        // GET /sectores: lista todos los sectores.
        Task<IReadOnlyList<SectorDto>> GetAllAsync(CancellationToken ct = default);
        // GET .../paginado: una pagina con filtros y orden resueltos en el servidor.
        Task<PagedResult<SectorDto>> GetPaginadoAsync(SectorFiltro filtro, CancellationToken ct = default);
        // GET /sectores/{id}: trae un sector.
        Task<SectorDto> GetOneAsync(int id, CancellationToken ct = default);
        // GET /sectores/estadio/{id}: sectores de un estadio.
        Task<IReadOnlyList<SectorDto>> GetByEstadioIdAsync(int estadioId, CancellationToken ct = default);
        // POST /sectores: crea un sector.
        Task<SectorDto> CreateAsync(SectorRequest request, CancellationToken ct = default);
        // PUT /sectores: modifica un sector (el id va en el cuerpo).
        Task UpdateAsync(SectorUpdateRequest request, CancellationToken ct = default);
        // DELETE /sectores/{id}: borra un sector.
        Task DeleteAsync(int id, CancellationToken ct = default);
    }
}
