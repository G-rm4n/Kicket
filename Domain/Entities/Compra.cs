using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Compra
    {
        public int CompraId { get; set; }
        public int UsuarioId { get; set; }
        public DateTime FechaCompra { get; set; } = DateTime.Now;
        public int Cantidad { get; set; }
        public decimal MontoTotal { get; set; }
        public string? MetodoPago { get; set; } = "MercadoPago";
        public string? PaymentId { get; set; }
        public string? EstadoPago { get; set; } = "Aprobado";
        public List<Entrada> Entradas { get; set; } = new List<Entrada>();
    }
}
