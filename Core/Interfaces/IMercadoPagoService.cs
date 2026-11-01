using Kicket.Contracts.Pagos;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    // Contrato del servicio de pagos con Mercado Pago.
    public interface IMercadoPagoService
    {
        // Crea la preferencia de pago (real o simulada) y devuelve la URL a la que hay que redirigir al usuario.
        Task<PreferenciaPagoResponse> CrearPreferenciaAsync(PreferenciaPagoRequest request, string baseUrlRetorno);
        // Indica si un pago, identificado por su paymentId, se considera aprobado.
        Task<bool> ValidarPagoAprobadoAsync(string paymentId);
    }
}
