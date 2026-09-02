using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Entrada
    {
        public int EntradaId { get; set; }
        public int CompraId { get; set; }
        public int EventoId { get; set; }
        public int SectorId { get; set; }
        public string? FilaAsiento { get; set; } //puede ser nulo si la entrada es para el sector popular
    }
}
