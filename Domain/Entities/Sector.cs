using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    // Entidad Sector: una zona de un estadio (ej. Platea, Popular) con capacidad y precio propios.
    public class Sector
    {
        // Clave primaria del sector.
        public int SectorId { get; set; }
        // FK al estadio al que pertenece el sector.
        public int EstadioId { get; set; }
        // Nombre del sector; si contiene "Platea" se asignan asientos numerados.
        public string Nombre { get; set; } = string.Empty; 
        // Cantidad máxima de entradas que se pueden vender en el sector por evento.
        public int CapacidadMaxima { get; set; }
        // Precio base de una entrada del sector (decimal(18,2) en la BD).
        public decimal PrecioBase { get; set; }
    }
}
