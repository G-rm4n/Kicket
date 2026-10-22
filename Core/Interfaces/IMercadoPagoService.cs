using Kicket.Contracts.Pagos;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IMercadoPagoService
    {
        Task<PreferenciaPagoResponse> CrearPreferenciaAsync(PreferenciaPagoRequest request, string baseUrlRetorno);
        Task<bool> ValidarPagoAprobadoAsync(string paymentId);
    }
}
