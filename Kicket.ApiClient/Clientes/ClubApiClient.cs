using Kicket.Contracts.Common;
using Kicket.ApiClient.Abstracciones;
using Kicket.ApiClient.Http;
using Kicket.Contracts.Clubes;

namespace Kicket.ApiClient.Clientes
{
    // Cliente HTTP tipado para los endpoints de clubes. Hereda el manejo de errores de ApiClientBase.
    public class ClubApiClient : ApiClientBase, IClubApiClient
    {
        // Prefijo de las rutas de clubes en la API.
        private const string Ruta = "clubes";

        // Recibe el HttpClient configurado (URL base + AuthTokenHandler).
        public ClubApiClient(HttpClient http) : base(http) { }

        // Lista todos los clubes.
        public async Task<IReadOnlyList<ClubDto>> GetAllAsync(CancellationToken ct = default) =>
            await GetAsync<List<ClubDto>>(Ruta, ct);

        // Pide una pagina filtrada (los filtros viajan en la query string).
        public Task<PagedResult<ClubDto>> GetPaginadoAsync(ClubFiltro filtro, CancellationToken ct = default) =>
            GetAsync<PagedResult<ClubDto>>(ConQuery($"{Ruta}/paginado", filtro), ct);
        // Trae un club por id.
        public Task<ClubDto> GetOneAsync(int id, CancellationToken ct = default) =>
            GetAsync<ClubDto>($"{Ruta}/{id}", ct);

        // Crea un club.
        public Task<ClubDto> CreateAsync(ClubRequest request, CancellationToken ct = default) =>
            PostAsync<ClubDto>(Ruta, request, ct);

        // PUT va sin id en la ruta: el endpoint del equipo es PUT /clubes.
        public Task UpdateAsync(ClubUpdateRequest request, CancellationToken ct = default) =>
            SinCuerpoAsync(Ruta, request, ct);

        // Borra un club por id.
        public Task DeleteAsync(int id, CancellationToken ct = default) =>
            DeleteAsync($"{Ruta}/{id}", ct);
    }
}
