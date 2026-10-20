using Kicket.Contracts.Eventos;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Kicket.ApiClient.Abstracciones
{
    public interface IEventoApiClient
    {
        Task<IReadOnlyList<EventoDto>> GetAllAsync(CancellationToken ct = default);
        Task<EventoDto> GetOneAsync(int id, CancellationToken ct = default);
        Task<IReadOnlyList<SectorDisponibilidadDto>> GetDisponibilidadAsync(int eventoId, CancellationToken ct = default);
        Task<EventoDto> CreateAsync(EventoRequest request, CancellationToken ct = default);
        Task UpdateAsync(EventoUpdateRequest request, CancellationToken ct = default);
        Task DeleteAsync(int id, CancellationToken ct = default);
    }
}
