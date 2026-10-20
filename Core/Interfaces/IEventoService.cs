using Domain.Entities;
using Kicket.Contracts.Eventos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IEventoService
    {
        Task<Evento> CrearEventoAsync(Evento evento);
        Task<Evento?> ObtenerPorIdAsync(int id);
        Task<IEnumerable<Evento>> ObtenerTodosAsync();
        Task<IReadOnlyList<SectorDisponibilidadDto>?> ObtenerDisponibilidadAsync(int eventoId);
        Task<bool> ActualizarEventoAsync(Evento evento);
        Task<bool> CancelarEventoAsync(int id);
        Task<bool> EliminarEventoAsync(int id);
    }
}
