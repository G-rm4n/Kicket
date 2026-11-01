using Kicket.Contracts.Compras;
using Kicket.Contracts.Pagos;
using System.Threading;
using System.Threading.Tasks;

namespace Kicket.ApiClient.Abstracciones
{
    // Operaciones de pago con Mercado Pago expuestas a las capas clientes.
    public interface IPagoApiClient
    {
        // POST /pagos/crear-preferencia: inicia el pago y devuelve la URL a la que redirigir.
        Task<PreferenciaPagoResponse> CrearPreferenciaAsync(PreferenciaPagoRequest request, CancellationToken ct = default);
        // POST /pagos/confirmar: confirma el pago aprobado y genera la compra.
        Task<CompraDto> ConfirmarPagoAsync(ConfirmarPagoRequest request, CancellationToken ct = default);
    }
}
