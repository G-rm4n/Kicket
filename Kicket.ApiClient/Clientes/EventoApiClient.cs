using Kicket.Contracts.Common;
using Kicket.ApiClient.Abstracciones;
using Kicket.ApiClient.Http;
using Kicket.Contracts.Eventos;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Kicket.ApiClient.Clientes
{
    // Cliente HTTP tipado para los endpoints de eventos.
    public class EventoApiClient : ApiClientBase, IEventoApiClient
    {
        // Prefijo de las rutas de eventos en la API.
        private const string Ruta = "eventos";

        // Recibe el HttpClient configurado.
        public EventoApiClient(HttpClient http) : base(http) { }

        // Lista todos los eventos.
        public async Task<IReadOnlyList<EventoDto>> GetAllAsync(CancellationToken ct = default) =>
            await GetAsync<List<EventoDto>>(Ruta, ct);

        // Pide una pagina filtrada (los filtros viajan en la query string).
        public Task<PagedResult<EventoDto>> GetPaginadoAsync(EventoFiltro filtro, CancellationToken ct = default) =>
            GetAsync<PagedResult<EventoDto>>(ConQuery($"{Ruta}/paginado", filtro), ct);
        // Trae un evento por id.
        public Task<EventoDto> GetOneAsync(int id, CancellationToken ct = default) =>
            GetAsync<EventoDto>($"{Ruta}/{id}", ct);

        // Trae la disponibilidad (stock por sector) de un evento.
        public async Task<IReadOnlyList<SectorDisponibilidadDto>> GetDisponibilidadAsync(int eventoId, CancellationToken ct = default) =>
            await GetAsync<List<SectorDisponibilidadDto>>($"{Ruta}/{eventoId}/disponibilidad", ct);

        // Crea un evento.
        public Task<EventoDto> CreateAsync(EventoRequest request, CancellationToken ct = default) =>
            PostAsync<EventoDto>(Ruta, request, ct);

        // Modifica un evento (PUT sin id en la ruta).
        public Task UpdateAsync(EventoUpdateRequest request, CancellationToken ct = default) =>
            SinCuerpoAsync(Ruta, request, ct);

        // Borra un evento por id.
        public Task DeleteAsync(int id, CancellationToken ct = default) =>
            DeleteAsync($"{Ruta}/{id}", ct);
    }
}
