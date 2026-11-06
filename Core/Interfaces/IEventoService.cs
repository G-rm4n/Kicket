using Domain.Entities;
using Kicket.Contracts.Common;
using Kicket.Contracts.Eventos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    // Contrato de la lógica de negocio de eventos.
    public interface IEventoService
    {
        // Valida y crea un evento.
        Task<Evento> CrearEventoAsync(Evento evento);
        // Devuelve un evento por id.
        Task<Evento?> ObtenerPorIdAsync(int id);
        // Devuelve todos los eventos.
        Task<IEnumerable<Evento>> ObtenerTodosAsync();
        // Pagina de eventos con filtros (club, estadio, fechas, estado).
        Task<PagedResult<EventoDto>> ObtenerPaginadoAsync(EventoFiltro filtro, CancellationToken ct = default);
        // Calcula, por cada sector del estadio, cuántas entradas se vendieron y cuántas quedan (null si el evento no existe).
        Task<IReadOnlyList<SectorDisponibilidadDto>?> ObtenerDisponibilidadAsync(int eventoId);
        // Valida y actualiza un evento; false si no existe.
        Task<bool> ActualizarEventoAsync(Evento evento);
        // Marca el evento como cancelado; false si no existe.
        Task<bool> CancelarEventoAsync(int id);
        // Elimina un evento; false si no existe.
        Task<bool> EliminarEventoAsync(int id);
    }
}
