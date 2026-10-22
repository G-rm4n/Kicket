using System.ComponentModel.DataAnnotations;

namespace Kicket.Contracts.Pagos
{
    public class ConfirmarPagoRequest
    {
        [Range(1, int.MaxValue, ErrorMessage = "El usuario es obligatorio.")]
        public int UsuarioId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "El evento es obligatorio.")]
        public int EventoId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "El sector es obligatorio.")]
        public int SectorId { get; set; }

        [Range(1, 20, ErrorMessage = "La cantidad de entradas debe ser entre 1 y 20.")]
        public int Cantidad { get; set; }

        [Required(ErrorMessage = "El ID de pago de Mercado Pago es obligatorio.")]
        public string PaymentId { get; set; } = string.Empty;

        [Required(ErrorMessage = "El estado del pago es obligatorio.")]
        public string Status { get; set; } = string.Empty;

        public string PreferenceId { get; set; } = string.Empty;

        public string MetodoPago { get; set; } = "MercadoPago";
    }
}
