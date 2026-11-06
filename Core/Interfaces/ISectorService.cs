using Domain.Entities;
using Kicket.Contracts.Common;
using Kicket.Contracts.Sectores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    // Contrato de la lógica de negocio de sectores.
    public interface ISectorService
    {
        // Valida y crea un sector.
        Task<Sector> CrearSectorAsync(Sector sector);
        // Devuelve un sector por id.
        Task<Sector?> ObtenerPorIdAsync(int id);
        // Devuelve todos los sectores.
        Task<IEnumerable<Sector>> ObtenerTodosAsync();
        // Valida y actualiza un sector; false si no existe.
        Task<bool> ActualizarSectorAsync(Sector sector);
        // Elimina un sector; false si no existe.
        Task<bool> EliminarSectorAsync(int id);
        // Devuelve los sectores de un estadio.
        Task<IEnumerable<Sector>> ObtenerPorEstadioAsync(int estadioId);
        // Pagina de sectores con filtros.
        Task<PagedResult<SectorDto>> ObtenerPaginadoAsync(SectorFiltro filtro, CancellationToken ct = default);
    }
}
