using Kicket.ApiClient.Abstracciones;
using Kicket.ApiClient.Http;
using Kicket.Contracts.Compras;
using Kicket.Contracts.Pagos;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Kicket.ApiClient.Clientes
{
    public class PagoApiClient : ApiClientBase, IPagoApiClient
    {
        private const string Ruta = "pagos";

        public PagoApiClient(HttpClient http) : base(http) { }

        public Task<PreferenciaPagoResponse> CrearPreferenciaAsync(PreferenciaPagoRequest request, CancellationToken ct = default) =>
            PostAsync<PreferenciaPagoResponse>($"{Ruta}/crear-preferencia", request, ct);

        public Task<CompraDto> ConfirmarPagoAsync(ConfirmarPagoRequest request, CancellationToken ct = default) =>
            PostAsync<CompraDto>($"{Ruta}/confirmar", request, ct);
    }
}
