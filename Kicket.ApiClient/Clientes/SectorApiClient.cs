using Kicket.ApiClient.Abstracciones;
using Kicket.ApiClient.Http;
using Kicket.Contracts.Sectores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kicket.ApiClient.Clientes
{
    public class SectorApiClient : ApiClientBase, ISectorApiClient
    {
        private const string Ruta = "sectores";

        public SectorApiClient(HttpClient http) : base(http) { }

        public async Task<IReadOnlyList<SectorDto>> GetAllAsync(CancellationToken ct = default) =>
            await GetAsync<List<SectorDto>>(Ruta, ct);

        public Task<SectorDto> GetOneAsync(int id, CancellationToken ct = default) =>
            GetAsync<SectorDto>($"{Ruta}/{id}", ct);

        public Task<SectorDto> CreateAsync(SectorRequest request, CancellationToken ct = default) =>
            PostAsync<SectorDto>(Ruta, request, ct);

        public Task UpdateAsync(SectorUpdateRequest request, CancellationToken ct = default) =>
            SinCuerpoAsync(Ruta, request, ct);

        public Task DeleteAsync(int id, CancellationToken ct = default) =>
            DeleteAsync($"{Ruta}/{id}", ct);
    }
}