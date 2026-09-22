using Kicket.Contracts.Entradas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kicket.Contracts.Compras
{
    public class CompraDto
    {
        public int CompraId { get; set; }
        public int UsuarioId { get; set; }
        public DateTime FechaCompra { get; set; }
        public int Cantidad { get; set; }
        public decimal MontoTotal { get; set; }
        public List<EntradaDto> Entradas { get; set; } = new();
    }
}
