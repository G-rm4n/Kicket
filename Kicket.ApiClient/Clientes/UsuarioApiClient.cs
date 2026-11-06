using Kicket.Contracts.Common;
using Kicket.ApiClient.Abstracciones;
using Kicket.ApiClient.Http;
using Kicket.Contracts.Usuarios;

namespace Kicket.ApiClient.Clientes
{
    // Cliente HTTP tipado para los endpoints de usuarios.
    public class UsuarioApiClient : ApiClientBase, IUsuarioApiClient
    {
        // Prefijo de las rutas de usuarios en la API.
        private const string Ruta = "usuarios";

        // Recibe el HttpClient configurado.
        public UsuarioApiClient(HttpClient http) : base(http) { }

        // Lista todos los usuarios.
        public async Task<IReadOnlyList<UsuarioDto>> GetAllAsync(CancellationToken ct = default) =>
            await GetAsync<List<UsuarioDto>>(Ruta, ct);

        // Pide una pagina filtrada (los filtros viajan en la query string).
        public Task<PagedResult<UsuarioDto>> GetPaginadoAsync(UsuarioFiltro filtro, CancellationToken ct = default) =>
            GetAsync<PagedResult<UsuarioDto>>(ConQuery($"{Ruta}/paginado", filtro), ct);
        // Trae un usuario por id.
        public Task<UsuarioDto> GetOneAsync(int id, CancellationToken ct = default) =>
            GetAsync<UsuarioDto>($"{Ruta}/{id}", ct);

        // Crea un usuario.
        public Task<UsuarioDto> CreateAsync(UsuarioRequest request, CancellationToken ct = default) =>
            PostAsync<UsuarioDto>(Ruta, request, ct);

        // Modifica un usuario (PUT sin id en la ruta).
        public Task UpdateAsync(UsuarioUpdateRequest request, CancellationToken ct = default) =>
            SinCuerpoAsync(Ruta, request, ct);

        // Borra un usuario por id.
        public Task DeleteAsync(int id, CancellationToken ct = default) =>
            DeleteAsync($"{Ruta}/{id}", ct);
    }
}
