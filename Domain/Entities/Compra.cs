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
        //se comentan porque no se ve sentido, una compra tendra varias entradas  o una, ellas poseeran los datos del sector y evento
        //public int EventoId { get; set; }
        //public int SectorId { get; set; }
        public DateTime FechaCompra { get; set; } = DateTime.Now;
        public int Cantidad { get; set; }
        public decimal MontoTotal { get; set; }
        public List<Entrada> Entradas { get; set; } = new List<Entrada>();
    }
}
