using Kicket.ApiClient.Abstracciones;
using Kicket.ApiClient.Http;
using Kicket.Contracts.Compras;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Kicket.ApiClient.Clientes
{
    public class CompraApiClient : ApiClientBase, ICompraApiClient
    {
        private const string Ruta = "compras";

        public CompraApiClient(HttpClient http) : base(http) { }

        public async Task<IReadOnlyList<CompraDto>> GetAllAsync(CancellationToken ct = default) =>
            await GetAsync<List<CompraDto>>(Ruta, ct);

        public async Task<IReadOnlyList<CompraDto>> GetByUsuarioIdAsync(int usuarioId, CancellationToken ct = default) =>
            await GetAsync<List<CompraDto>>($"{Ruta}/usuario/{usuarioId}", ct);

        public Task<CompraDto> GetOneAsync(int id, CancellationToken ct = default) =>
            GetAsync<CompraDto>($"{Ruta}/{id}", ct);

        public Task<CompraDto> CreateAsync(CompraRequest request, CancellationToken ct = default) =>
            PostAsync<CompraDto>(Ruta, request, ct);

        public Task DeleteAsync(int id, CancellationToken ct = default) =>
            DeleteAsync($"{Ruta}/{id}", ct);
    }
}
