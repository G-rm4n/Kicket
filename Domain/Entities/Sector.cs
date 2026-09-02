using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Sector
    {
        public int SectorId { get; set; }
        public int EstadioId { get; set; }
        public string Nombre { get; set; } = string.Empty; 
        public int CapacidadMaxima { get; set; }
        public int PrecioBase { get; set; }
    }
}
