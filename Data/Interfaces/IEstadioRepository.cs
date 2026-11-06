using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Kicket.Contracts.Common;
using Kicket.Contracts.Estadios;

namespace Data.Interfaces
{
    // Contrato del repositorio de estadios: operaciones CRUD contra la tabla Estadios.
    public interface IEstadioRepository
    {
        // Agrega un estadio nuevo.
        Task AddAsync(Estadio estadio);
        // Borra un estadio por id; false si no existe.
        Task<bool> DeleteAsync(int id);
        // Actualiza un estadio; false si no existe.
        Task<bool> UpdateAsync(Estadio estadio);
        // Busca un estadio por id.
        Task<Estadio?> GetByIdAsync(int id);
        // Devuelve todos los estadios.
        Task<IEnumerable<Estadio>> GetAllAsync();
        // Pagina de estadios con la capacidad total de sus sectores.
        Task<PagedResult<EstadioDto>> ObtenerPaginadoAsync(EstadioFiltro filtro, CancellationToken ct = default);
    }
}
