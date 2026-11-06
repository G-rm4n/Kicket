using Domain.Entities;
using Kicket.Contracts.Common;
using Kicket.Contracts.Sectores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    // Contrato del repositorio de sectores.
    public interface ISectorRepository
    {
        // Busca un sector por id.
        Task<Sector?> ObtenerSectorPorIdAsync(int sectorId);
        // Agrega un sector nuevo.
        Task AddSectorAsync(Sector sector);

        // Actualiza un sector; false si no existe.
        Task<bool> UpdateSectorAsync(Sector sector);

        // Borra un sector por id; false si no existe.
        Task<bool> DeleteSectorAsync(int sectorId);
        // Devuelve todos los sectores.
        Task<IEnumerable<Sector>> GetAll();
        // Devuelve los sectores de un estadio.
        Task<IEnumerable<Sector>> ObtenerPorEstadioAsync(int estadioId);
        // Pagina de sectores (con el nombre del estadio).
        Task<PagedResult<SectorDto>> ObtenerPaginadoAsync(SectorFiltro filtro, CancellationToken ct = default);
    }
}
