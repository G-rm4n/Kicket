using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    // Entidad Compra: agrupa las entradas que un usuario compró en una misma operación de pago.
    public class Compra
    {
        // Clave primaria de la compra.
        public int CompraId { get; set; }
        // FK al usuario que hizo la compra.
        public int UsuarioId { get; set; }
        // Fecha y hora de la compra (por defecto, el momento en que se crea el objeto).
        public DateTime FechaCompra { get; set; } = DateTime.Now;
        // Cantidad de entradas compradas.
        public int Cantidad { get; set; }
        // Importe total pagado (decimal(18,2) en la BD).
        public decimal MontoTotal { get; set; }
        // Medio de pago usado; por defecto "MercadoPago".
        public string? MetodoPago { get; set; } = "MercadoPago";
        // Id del pago que devuelve Mercado Pago (o el simulador).
        public string? PaymentId { get; set; }
        // Estado del pago; por defecto "Aprobado".
        public string? EstadoPago { get; set; } = "Aprobado";
        // Entradas generadas por esta compra (relación uno a muchos).
        public List<Entrada> Entradas { get; set; } = new List<Entrada>();
    }
}
