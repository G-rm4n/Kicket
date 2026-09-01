using Kicket.ApiClient.Abstracciones;
using Kicket.ApiClient.Http;
using Kicket.Contracts.Usuarios;

namespace Kicket.ApiClient.Clientes
{
    public class UsuarioApiClient : ApiClientBase, IUsuarioApiClient
    {
        private const string Ruta = "usuarios";

        public UsuarioApiClient(HttpClient http) : base(http) { }

        public async Task<IReadOnlyList<UsuarioDto>> GetAllAsync(CancellationToken ct = default) =>
            await GetAsync<List<UsuarioDto>>(Ruta, ct);

        public Task<UsuarioDto> GetOneAsync(int id, CancellationToken ct = default) =>
            GetAsync<UsuarioDto>($"{Ruta}/{id}", ct);

        public Task<UsuarioDto> CreateAsync(UsuarioRequest request, CancellationToken ct = default) =>
            PostAsync<UsuarioDto>(Ruta, request, ct);

        public Task UpdateAsync(UsuarioUpdateRequest request, CancellationToken ct = default) =>
            SinCuerpoAsync(Ruta, request, ct);

        public Task DeleteAsync(int id, CancellationToken ct = default) =>
            DeleteAsync($"{Ruta}/{id}", ct);
    }
}
