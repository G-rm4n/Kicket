using Kicket.Contracts.Estadios;

namespace Kicket.ApiClient.Abstracciones
{
    /// <summary>Operaciones de Estadio expuestas a la capa de escritorio.</summary>
    public interface IEstadioApiClient
    {
        Task<IReadOnlyList<EstadioDto>> GetAllAsync(CancellationToken ct = default);
        Task<EstadioDto> GetOneAsync(int id, CancellationToken ct = default);
        Task<EstadioDto> CreateAsync(EstadioRequest request, CancellationToken ct = default);
        Task UpdateAsync(EstadioUpdateRequest request, CancellationToken ct = default);
        Task DeleteAsync(int id, CancellationToken ct = default);
    }
}
