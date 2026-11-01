using System.ComponentModel.DataAnnotations;

namespace Kicket.Contracts.Pagos
{
    // Datos para pedir una preferencia de pago (iniciar el checkout de Mercado Pago).
    public class PreferenciaPagoRequest
    {
        // Usuario que compra (mayor a 0).
        [Range(1, int.MaxValue, ErrorMessage = "El usuario es obligatorio.")]
        public int UsuarioId { get; set; }

        // Evento elegido (mayor a 0).
        [Range(1, int.MaxValue, ErrorMessage = "El evento es obligatorio.")]
        public int EventoId { get; set; }

        // Sector elegido (mayor a 0).
        [Range(1, int.MaxValue, ErrorMessage = "El sector es obligatorio.")]
        public int SectorId { get; set; }

        // Cantidad: entre 1 y 20.
        [Range(1, 20, ErrorMessage = "La cantidad de entradas debe ser entre 1 y 20.")]
        public int Cantidad { get; set; }
    }
}
