using Kicket.ApiClient.Abstracciones;
using Kicket.ApiClient.Http;
using Kicket.Contracts.Common;
using Kicket.Contracts.Eventos;

namespace Kicket.ApiClient.Clientes
{
    public class EventoApiClient : ApiClientBase, IEventoApiClient
    {
        private const string Ruta = "eventos";

        public EventoApiClient(HttpClient http) : base(http) { }

        public async Task<IReadOnlyList<EventoDto>> GetAllAsync(CancellationToken ct = default) =>
            await GetAsync<List<EventoDto>>(Ruta, ct);

        public Task<PagedResult<EventoDto>> GetPaginadoAsync(
            PageRequest? pagina = null, CancellationToken ct = default) =>
            GetPaginadoAsync<EventoDto>($"{Ruta}/paginado", pagina, ct);

        public Task<EventoDto> GetOneAsync(int id, CancellationToken ct = default) =>
            GetAsync<EventoDto>($"{Ruta}/{id}", ct);

        public Task<EventoDto> CreateAsync(EventoRequest request, CancellationToken ct = default) =>
            PostAsync<EventoDto>(Ruta, request, ct);

        public Task UpdateAsync(EventoUpdateRequest request, CancellationToken ct = default) =>
            SinCuerpoAsync(Ruta, request, ct);

        public Task DeleteAsync(int id, CancellationToken ct = default) =>
            DeleteAsync($"{Ruta}/{id}", ct);
    }
}
