using Kicket.Contracts.Common;
using Kicket.Contracts.Usuarios;

namespace Kicket.ApiClient.Abstracciones
{
    /// <summary>Operaciones de Usuario expuestas a la capa de escritorio.</summary>
    public interface IUsuarioApiClient
    {
        Task<IReadOnlyList<UsuarioDto>> GetAllAsync(CancellationToken ct = default);

        /// <summary>
        /// Listado paginado para la pantalla de clientes del admin. GetAllAsync se queda
        /// para los combos (traer todo de una), esto es para la grilla, que puede crecer
        /// sin techo. Busqueda filtra por nombre, apellido o email.
        /// </summary>
        Task<PagedResult<UsuarioDto>> GetPaginadoAsync(PageRequest? pagina = null, CancellationToken ct = default);

        Task<UsuarioDto> GetOneAsync(int id, CancellationToken ct = default);
        Task<UsuarioDto> CreateAsync(UsuarioRequest request, CancellationToken ct = default);
        Task UpdateAsync(UsuarioUpdateRequest request, CancellationToken ct = default);
        Task DeleteAsync(int id, CancellationToken ct = default);
    }
}
