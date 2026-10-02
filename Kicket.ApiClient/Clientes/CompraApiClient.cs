using Kicket.ApiClient.Abstracciones;
using Kicket.ApiClient.Http;
using Kicket.Contracts.Common;
using Kicket.Contracts.Compras;

namespace Kicket.ApiClient.Clientes
{
    public class CompraApiClient : ApiClientBase, ICompraApiClient
    {
        private const string Ruta = "compras";

        public CompraApiClient(HttpClient http) : base(http) { }

        public async Task<IReadOnlyList<CompraDto>> GetAllAsync(CancellationToken ct = default) =>
            await GetAsync<List<CompraDto>>(Ruta, ct);

        public Task<PagedResult<CompraDto>> GetPaginadoAsync(
            PageRequest? pagina = null, CancellationToken ct = default) =>
            GetPaginadoAsync<CompraDto>($"{Ruta}/paginado", pagina, ct);

        public Task<PagedResult<CompraDto>> GetMisComprasPaginadoAsync(
            PageRequest? pagina = null, CancellationToken ct = default) =>
            GetPaginadoAsync<CompraDto>($"{Ruta}/mias/paginado", pagina, ct);

        public Task<PagedResult<CompraDto>> GetPorUsuarioPaginadoAsync(
            int usuarioId, PageRequest? pagina = null, CancellationToken ct = default) =>
            GetPaginadoAsync<CompraDto>($"{Ruta}/usuario/{usuarioId}/paginado", pagina, ct);

        public Task<CompraDto> GetOneAsync(int id, CancellationToken ct = default) =>
            GetAsync<CompraDto>($"{Ruta}/{id}", ct);

        public Task<CompraDto> CreateAsync(CompraRequest request, CancellationToken ct = default) =>
            PostAsync<CompraDto>(Ruta, request, ct);

        public Task DeleteAsync(int id, CancellationToken ct = default) =>
            DeleteAsync($"{Ruta}/{id}", ct);
    }
}
