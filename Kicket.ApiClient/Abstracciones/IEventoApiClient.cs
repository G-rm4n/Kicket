using Kicket.Contracts.Eventos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kicket.ApiClient.Abstracciones
{
    public interface IEventoApiClient
    {
        Task<IReadOnlyList<EventoDto>> GetAllAsync(CancellationToken ct = default);
        Task<EventoDto> GetOneAsync(int id, CancellationToken ct = default);
        Task<EventoDto> CreateAsync(EventoRequest request, CancellationToken ct = default);
        Task UpdateAsync(EventoUpdateRequest request, CancellationToken ct = default);
        Task DeleteAsync(int id, CancellationToken ct = default);
    }
}
