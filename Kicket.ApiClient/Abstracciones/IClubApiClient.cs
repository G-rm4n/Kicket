using Kicket.Contracts.Clubes;

namespace Kicket.ApiClient.Abstracciones
{
    /// <summary>Operaciones de Club expuestas a la capa de escritorio.</summary>
    public interface IClubApiClient
    {
        // GET /clubes: lista todos los clubes.
        Task<IReadOnlyList<ClubDto>> GetAllAsync(CancellationToken ct = default);
        // GET /clubes/{id}: trae un club.
        Task<ClubDto> GetOneAsync(int id, CancellationToken ct = default);
        // POST /clubes: crea un club y devuelve el creado.
        Task<ClubDto> CreateAsync(ClubRequest request, CancellationToken ct = default);
        // PUT /clubes: modifica un club (el id va en el cuerpo).
        Task UpdateAsync(ClubUpdateRequest request, CancellationToken ct = default);
        // DELETE /clubes/{id}: borra un club.
        Task DeleteAsync(int id, CancellationToken ct = default);
    }
}
