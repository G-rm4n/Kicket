using Kicket.ApiClient.Abstracciones;
using Kicket.ApiClient.Http;
using Kicket.Contracts.Entradas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kicket.ApiClient.Clientes
{
    // Cliente HTTP tipado para los endpoints de entradas (solo lectura).
    public class EntradaApiClient : ApiClientBase, IEntradaApiClient
    {
        // Prefijo de las rutas de entradas en la API.
        private const string Ruta = "entradas";

        // Recibe el HttpClient configurado.
        public EntradaApiClient(HttpClient http) : base(http) { }

        // Trae una entrada por id.
        public Task<EntradaDto> GetOneAsync(int id, CancellationToken ct = default) =>
            GetAsync<EntradaDto>($"{Ruta}/{id}", ct);

        // Lista las entradas de un evento.
        public async Task<IReadOnlyList<EntradaDto>> GetPorEventoAsync(int eventoId, CancellationToken ct = default) =>
            await GetAsync<List<EntradaDto>>($"{Ruta}/evento/{eventoId}", ct);
    }
}
