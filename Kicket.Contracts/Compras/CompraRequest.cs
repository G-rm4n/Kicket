using System.ComponentModel.DataAnnotations;

namespace Kicket.Contracts.Compras
{
    /// <summary>
    /// Datos para generar una compra (POST /compras). No tiene UpdateRequest: una compra
    /// no se edita, en todo caso se cancela/elimina (ver ICompraService).
    /// </summary>
    public class CompraRequest
    {
        // Usuario que compra.
        [Required(ErrorMessage = "El usuario es obligatorio.")]
        public int UsuarioId { get; set; }

        // Evento elegido.
        [Required(ErrorMessage = "El evento es obligatorio.")]
        public int EventoId { get; set; }

        // Sector elegido.
        [Required(ErrorMessage = "El sector es obligatorio.")]
        public int SectorId { get; set; }

        // Cantidad: entre 1 y 20 entradas por compra.
        [Range(1, 20, ErrorMessage = "La cantidad de entradas debe ser entre 1 y 20.")]
        public int Cantidad { get; set; }

        // Datos del pago (opcionales; tienen valores por defecto).
        public string? MetodoPago { get; set; } = "MercadoPago";
        public string? PaymentId { get; set; }
        public string? EstadoPago { get; set; } = "Aprobado";
    }
}
