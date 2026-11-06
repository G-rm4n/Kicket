using Kicket.Contracts.Common;
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
    // Cliente HTTP tipado para los endpoints de sectores.
    public class SectorApiClient : ApiClientBase, ISectorApiClient
    {
        // Prefijo de las rutas de sectores en la API.
        private const string Ruta = "sectores";

        // Recibe el HttpClient configurado.
        public SectorApiClient(HttpClient http) : base(http) { }

        // Lista todos los sectores.
        public async Task<IReadOnlyList<SectorDto>> GetAllAsync(CancellationToken ct = default) =>
            await GetAsync<List<SectorDto>>(Ruta, ct);

        // Pide una pagina filtrada (los filtros viajan en la query string).
        public Task<PagedResult<SectorDto>> GetPaginadoAsync(SectorFiltro filtro, CancellationToken ct = default) =>
            GetAsync<PagedResult<SectorDto>>(ConQuery($"{Ruta}/paginado", filtro), ct);
        // Trae un sector por id.
        public Task<SectorDto> GetOneAsync(int id, CancellationToken ct = default) =>
            GetAsync<SectorDto>($"{Ruta}/{id}", ct);

        // Lista los sectores de un estadio.
        public async Task<IReadOnlyList<SectorDto>> GetByEstadioIdAsync(int estadioId, CancellationToken ct = default) =>
            await GetAsync<List<SectorDto>>($"{Ruta}/estadio/{estadioId}", ct);

        // Crea un sector.
        public Task<SectorDto> CreateAsync(SectorRequest request, CancellationToken ct = default) =>
            PostAsync<SectorDto>(Ruta, request, ct);

        // Modifica un sector (PUT sin id en la ruta).
        public Task UpdateAsync(SectorUpdateRequest request, CancellationToken ct = default) =>
            SinCuerpoAsync(Ruta, request, ct);

        // Borra un sector por id.
        public Task DeleteAsync(int id, CancellationToken ct = default) =>
            DeleteAsync($"{Ruta}/{id}", ct);
    }
}