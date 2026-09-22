using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kicket.Contracts.Entradas
{
    public class EntradaDto
    {
        public int EntradaId { get; set; }
        public int CompraId { get; set; }
        public int EventoId { get; set; }
        public int SectorId { get; set; }

        /// <summary>Puede ser nulo si la entrada es para un sector sin asientos numerados (popular).</summary>
        public string? FilaAsiento { get; set; }
    }
}
