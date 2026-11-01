using Kicket.ApiClient.Abstracciones;
using Kicket.ApiClient.Http;
using Kicket.Contracts.Compras;
using Kicket.Contracts.Pagos;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Kicket.ApiClient.Clientes
{
    // Cliente HTTP tipado para los endpoints de pagos.
    public class PagoApiClient : ApiClientBase, IPagoApiClient
    {
        // Prefijo de las rutas de pagos en la API.
        private const string Ruta = "pagos";

        // Recibe el HttpClient configurado.
        public PagoApiClient(HttpClient http) : base(http) { }

        // Pide a la API que cree la preferencia de pago.
        public Task<PreferenciaPagoResponse> CrearPreferenciaAsync(PreferenciaPagoRequest request, CancellationToken ct = default) =>
            PostAsync<PreferenciaPagoResponse>($"{Ruta}/crear-preferencia", request, ct);

        // Confirma el pago en la API y recibe la compra generada.
        public Task<CompraDto> ConfirmarPagoAsync(ConfirmarPagoRequest request, CancellationToken ct = default) =>
            PostAsync<CompraDto>($"{Ruta}/confirmar", request, ct);
    }
}
