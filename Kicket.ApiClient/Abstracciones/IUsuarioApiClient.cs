using Kicket.Contracts.Usuarios;

namespace Kicket.ApiClient.Abstracciones
{
    /// <summary>Operaciones de Usuario expuestas a la capa de escritorio.</summary>
    public interface IUsuarioApiClient
    {
        Task<IReadOnlyList<UsuarioDto>> GetAllAsync(CancellationToken ct = default);
        Task<UsuarioDto> GetOneAsync(int id, CancellationToken ct = default);
        Task<UsuarioDto> CreateAsync(UsuarioRequest request, CancellationToken ct = default);
        Task UpdateAsync(UsuarioUpdateRequest request, CancellationToken ct = default);
        Task DeleteAsync(int id, CancellationToken ct = default);
    }
}
