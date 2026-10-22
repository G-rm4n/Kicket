using Kicket.Contracts.Entradas;
using System;
using System.Collections.Generic;

namespace Kicket.Contracts.Compras
{
    public class CompraDto
    {
        public int CompraId { get; set; }
        public int UsuarioId { get; set; }
        public DateTime FechaCompra { get; set; }
        public int Cantidad { get; set; }
        public decimal MontoTotal { get; set; }
        public string? MetodoPago { get; set; }
        public string? PaymentId { get; set; }
        public string? EstadoPago { get; set; }
        public List<EntradaDto> Entradas { get; set; } = new();
    }
}
