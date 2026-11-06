using Kicket.Contracts.Common;
using Kicket.Contracts.Usuarios;

namespace Kicket.ApiClient.Abstracciones
{
    /// <summary>Operaciones de Usuario expuestas a la capa de escritorio.</summary>
    public interface IUsuarioApiClient
    {
        // GET /usuarios: lista todos los usuarios.
        Task<IReadOnlyList<UsuarioDto>> GetAllAsync(CancellationToken ct = default);
        // GET .../paginado: una pagina con filtros y orden resueltos en el servidor.
        Task<PagedResult<UsuarioDto>> GetPaginadoAsync(UsuarioFiltro filtro, CancellationToken ct = default);
        // GET /usuarios/{id}: trae un usuario.
        Task<UsuarioDto> GetOneAsync(int id, CancellationToken ct = default);
        // POST /usuarios: crea un usuario.
        Task<UsuarioDto> CreateAsync(UsuarioRequest request, CancellationToken ct = default);
        // PUT /usuarios: modifica un usuario (el id va en el cuerpo).
        Task UpdateAsync(UsuarioUpdateRequest request, CancellationToken ct = default);
        // DELETE /usuarios/{id}: borra un usuario.
        Task DeleteAsync(int id, CancellationToken ct = default);
    }
}
