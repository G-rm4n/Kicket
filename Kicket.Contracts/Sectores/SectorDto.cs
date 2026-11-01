using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kicket.Contracts.Sectores
{
    // Sector tal como viaja de la API hacia el cliente.
    public class SectorDto
    {
        // Id del sector.
        public int SectorId { get; set; }
        // Estadio al que pertenece.
        public int EstadioId { get; set; }
        // Nombre del sector.
        public string Nombre { get; set; } = string.Empty;
        // Capacidad máxima.
        public int CapacidadMaxima { get; set; }
        // Precio base de la entrada.
        public decimal PrecioBase { get; set; }
    }
}
