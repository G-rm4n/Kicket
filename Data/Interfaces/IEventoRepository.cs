using Domain.Entities;
using Kicket.Contracts.Common;
using Kicket.Contracts.Eventos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    // Contrato del repositorio de eventos.
    public interface IEventoRepository
    {
        // Busca un evento por id.
        Task<Evento?> GetOneById(int eventoId);
        // Agrega un evento nuevo.
        Task AddAsync(Evento evento);
        // Borra un evento por id; false si no existe.
        Task<bool> DeleteAsync(int id);
        // Devuelve todos los eventos.
        Task<IEnumerable<Evento>> GetAllAsync();
        // Actualiza un evento; false si no existe.
        Task<bool> UpdateAsync(Evento evento);

        // Pagina de eventos con los nombres de clubes y estadio ya resueltos.
        Task<PagedResult<EventoDto>> ObtenerPaginadoAsync(EventoFiltro filtro, CancellationToken ct = default);
    }
}
