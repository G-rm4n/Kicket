using Kicket.Contracts.Entradas;
using System;
using System.Collections.Generic;

namespace Kicket.Contracts.Compras
{
    // Compra tal como viaja de la API hacia el cliente, con sus entradas.
    public class CompraDto
    {
        // Id de la compra.
        public int CompraId { get; set; }
        // Usuario que compró.
        public int UsuarioId { get; set; }
        // Fecha de la compra.
        public DateTime FechaCompra { get; set; }
        // Cantidad de entradas.
        public int Cantidad { get; set; }
        // Total pagado.
        public decimal MontoTotal { get; set; }
        // Medio de pago.
        public string? MetodoPago { get; set; }
        // Id del pago en Mercado Pago.
        public string? PaymentId { get; set; }
        // Estado del pago.
        public string? EstadoPago { get; set; }
        // Entradas generadas por la compra.
        public List<EntradaDto> Entradas { get; set; } = new();
    }
}
