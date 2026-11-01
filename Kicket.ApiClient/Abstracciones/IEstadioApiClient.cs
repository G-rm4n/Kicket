using Kicket.Contracts.Estadios;

namespace Kicket.ApiClient.Abstracciones
{
    /// <summary>Operaciones de Estadio expuestas a la capa de escritorio.</summary>
    public interface IEstadioApiClient
    {
        // GET /estadios: lista todos los estadios.
        Task<IReadOnlyList<EstadioDto>> GetAllAsync(CancellationToken ct = default);
        // GET /estadios/{id}: trae un estadio.
        Task<EstadioDto> GetOneAsync(int id, CancellationToken ct = default);
        // POST /estadios: crea un estadio.
        Task<EstadioDto> CreateAsync(EstadioRequest request, CancellationToken ct = default);
        // PUT /estadios: modifica un estadio (el id va en el cuerpo).
        Task UpdateAsync(EstadioUpdateRequest request, CancellationToken ct = default);
        // DELETE /estadios/{id}: borra un estadio.
        Task DeleteAsync(int id, CancellationToken ct = default);
    }
}
