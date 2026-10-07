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
    public class EntradaApiClient : ApiClientBase, IEntradaApiClient
    {
        private const string Ruta = "entradas";

        public EntradaApiClient(HttpClient http) : base(http) { }

        public Task<EntradaDto> GetOneAsync(int id, CancellationToken ct = default) =>
            GetAsync<EntradaDto>($"{Ruta}/{id}", ct);

        public async Task<IReadOnlyList<EntradaDto>> GetPorEventoAsync(int eventoId, CancellationToken ct = default) =>
            await GetAsync<List<EntradaDto>>($"{Ruta}/evento/{eventoId}", ct);
    }
}
