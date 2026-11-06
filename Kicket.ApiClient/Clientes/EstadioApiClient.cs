using Kicket.Contracts.Common;
using Kicket.ApiClient.Abstracciones;
using Kicket.ApiClient.Http;
using Kicket.Contracts.Estadios;

namespace Kicket.ApiClient.Clientes
{
    // Cliente HTTP tipado para los endpoints de estadios.
    public class EstadioApiClient : ApiClientBase, IEstadioApiClient
    {
        // Prefijo de las rutas de estadios en la API.
        private const string Ruta = "estadios";

        // Recibe el HttpClient configurado.
        public EstadioApiClient(HttpClient http) : base(http) { }

        // Lista todos los estadios.
        public async Task<IReadOnlyList<EstadioDto>> GetAllAsync(CancellationToken ct = default) =>
            await GetAsync<List<EstadioDto>>(Ruta, ct);

        // Pide una pagina filtrada (los filtros viajan en la query string).
        public Task<PagedResult<EstadioDto>> GetPaginadoAsync(EstadioFiltro filtro, CancellationToken ct = default) =>
            GetAsync<PagedResult<EstadioDto>>(ConQuery($"{Ruta}/paginado", filtro), ct);
        // Trae un estadio por id.
        public Task<EstadioDto> GetOneAsync(int id, CancellationToken ct = default) =>
            GetAsync<EstadioDto>($"{Ruta}/{id}", ct);

        // Crea un estadio.
        public Task<EstadioDto> CreateAsync(EstadioRequest request, CancellationToken ct = default) =>
            PostAsync<EstadioDto>(Ruta, request, ct);

        // Modifica un estadio (PUT sin id en la ruta).
        public Task UpdateAsync(EstadioUpdateRequest request, CancellationToken ct = default) =>
            SinCuerpoAsync(Ruta, request, ct);

        // Borra un estadio por id.
        public Task DeleteAsync(int id, CancellationToken ct = default) =>
            DeleteAsync($"{Ruta}/{id}", ct);
    }
}
