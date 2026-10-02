using Kicket.Contracts.Common;
using Kicket.Contracts.Eventos;

namespace Kicket.ApiClient.Abstracciones
{
    /// <summary>Operaciones de Evento expuestas a la capa de escritorio.</summary>
    public interface IEventoApiClient
    {
        Task<IReadOnlyList<EventoDto>> GetAllAsync(CancellationToken ct = default);

        /// <summary>
        /// Historial de eventos para el admin. Se acumula una fila por partido jugado,
        /// asi que la grilla pagina. Conviene mandar OrdenarPor = "Fecha" y Descendente:
        /// lo que interesa primero es el evento mas proximo.
        /// </summary>
        Task<PagedResult<EventoDto>> GetPaginadoAsync(PageRequest? pagina = null, CancellationToken ct = default);

        Task<EventoDto> GetOneAsync(int id, CancellationToken ct = default);
        Task<EventoDto> CreateAsync(EventoRequest request, CancellationToken ct = default);
        Task UpdateAsync(EventoUpdateRequest request, CancellationToken ct = default);
        Task DeleteAsync(int id, CancellationToken ct = default);
    }
}
