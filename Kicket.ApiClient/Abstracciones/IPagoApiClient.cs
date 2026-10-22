using Kicket.Contracts.Compras;
using Kicket.Contracts.Pagos;
using System.Threading;
using System.Threading.Tasks;

namespace Kicket.ApiClient.Abstracciones
{
    public interface IPagoApiClient
    {
        Task<PreferenciaPagoResponse> CrearPreferenciaAsync(PreferenciaPagoRequest request, CancellationToken ct = default);
        Task<CompraDto> ConfirmarPagoAsync(ConfirmarPagoRequest request, CancellationToken ct = default);
    }
}
