using Kicket.ApiClient.Abstracciones;
using Kicket.ApiClient.Http;
using Kicket.Contracts.Estadios;

namespace Kicket.ApiClient.Clientes
{
    public class EstadioApiClient : ApiClientBase, IEstadioApiClient
    {
        private const string Ruta = "estadios";

        public EstadioApiClient(HttpClient http) : base(http) { }

        public async Task<IReadOnlyList<EstadioDto>> GetAllAsync(CancellationToken ct = default) =>
            await GetAsync<List<EstadioDto>>(Ruta, ct);

        public Task<EstadioDto> GetOneAsync(int id, CancellationToken ct = default) =>
            GetAsync<EstadioDto>($"{Ruta}/{id}", ct);

        public Task<EstadioDto> CreateAsync(EstadioRequest request, CancellationToken ct = default) =>
            PostAsync<EstadioDto>(Ruta, request, ct);

        public Task UpdateAsync(EstadioUpdateRequest request, CancellationToken ct = default) =>
            SinCuerpoAsync(Ruta, request, ct);

        public Task DeleteAsync(int id, CancellationToken ct = default) =>
            DeleteAsync($"{Ruta}/{id}", ct);
    }
}
