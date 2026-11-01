using System.ComponentModel.DataAnnotations;

namespace Kicket.Contracts.Pagos
{
    // Datos que manda el cliente al volver de Mercado Pago para confirmar el pago y generar la compra.
    public class ConfirmarPagoRequest
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

        // Id del pago que devolvió Mercado Pago (o el simulador).
        [Required(ErrorMessage = "El ID de pago de Mercado Pago es obligatorio.")]
        public string PaymentId { get; set; } = string.Empty;

        // Estado del pago ("approved", "rejected", etc.).
        [Required(ErrorMessage = "El estado del pago es obligatorio.")]
        public string Status { get; set; } = string.Empty;

        // Id de la preferencia de pago.
        public string PreferenceId { get; set; } = string.Empty;

        // Medio de pago; por defecto "MercadoPago".
        public string MetodoPago { get; set; } = "MercadoPago";
    }
}
