using Kicket.Contracts.Clubes;

namespace Kicket.ApiClient.Abstracciones
{
    /// <summary>Operaciones de Club expuestas a la capa de escritorio.</summary>
    public interface IClubApiClient
    {
        Task<IReadOnlyList<ClubDto>> GetAllAsync(CancellationToken ct = default);
        Task<ClubDto> GetOneAsync(int id, CancellationToken ct = default);
        Task<ClubDto> CreateAsync(ClubRequest request, CancellationToken ct = default);
        Task UpdateAsync(ClubUpdateRequest request, CancellationToken ct = default);
        Task DeleteAsync(int id, CancellationToken ct = default);
    }
}
