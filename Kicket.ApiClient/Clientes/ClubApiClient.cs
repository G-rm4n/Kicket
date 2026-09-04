using Kicket.ApiClient.Abstracciones;
using Kicket.ApiClient.Http;
using Kicket.Contracts.Clubes;

namespace Kicket.ApiClient.Clientes
{
    public class ClubApiClient : ApiClientBase, IClubApiClient
    {
        private const string Ruta = "clubes";

        public ClubApiClient(HttpClient http) : base(http) { }

        public async Task<IReadOnlyList<ClubDto>> GetAllAsync(CancellationToken ct = default) =>
            await GetAsync<List<ClubDto>>(Ruta, ct);

        public Task<ClubDto> GetOneAsync(int id, CancellationToken ct = default) =>
            GetAsync<ClubDto>($"{Ruta}/{id}", ct);

        public Task<ClubDto> CreateAsync(ClubRequest request, CancellationToken ct = default) =>
            PostAsync<ClubDto>(Ruta, request, ct);

        // PUT va sin id en la ruta: el endpoint del equipo es PUT /clubes.
        public Task UpdateAsync(ClubUpdateRequest request, CancellationToken ct = default) =>
            SinCuerpoAsync(Ruta, request, ct);

        public Task DeleteAsync(int id, CancellationToken ct = default) =>
            DeleteAsync($"{Ruta}/{id}", ct);
    }
}
