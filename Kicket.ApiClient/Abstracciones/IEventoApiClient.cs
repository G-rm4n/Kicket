using Kicket.Contracts.Eventos;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Kicket.ApiClient.Abstracciones
{
    // Operaciones de Evento expuestas a las capas clientes (Blazor y WinForms).
    public interface IEventoApiClient
    {
        // GET /eventos: lista todos los eventos.
        Task<IReadOnlyList<EventoDto>> GetAllAsync(CancellationToken ct = default);
        // GET /eventos/{id}: trae un evento.
        Task<EventoDto> GetOneAsync(int id, CancellationToken ct = default);
        // GET /eventos/{id}/disponibilidad: stock de cada sector para ese evento.
        Task<IReadOnlyList<SectorDisponibilidadDto>> GetDisponibilidadAsync(int eventoId, CancellationToken ct = default);
        // POST /eventos: crea un evento.
        Task<EventoDto> CreateAsync(EventoRequest request, CancellationToken ct = default);
        // PUT /eventos: modifica un evento (el id va en el cuerpo).
        Task UpdateAsync(EventoUpdateRequest request, CancellationToken ct = default);
        // DELETE /eventos/{id}: borra un evento.
        Task DeleteAsync(int id, CancellationToken ct = default);
    }
}
