using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    // Entidad Estadio: lugar físico donde se juegan los eventos; tiene sectores.
    public class Estadio
    {
        // Clave primaria del estadio.
        public int EstadioId { get; set; } = 0;
        // Nombre del estadio.
        public string Nombre { get; set; } = string.Empty;
        // Dirección del estadio.
        public string Direccion { get; set; } = string.Empty;
        // Ciudad donde está el estadio.
        public String Ciudad { get; set; } = string.Empty;
        // Provincia donde está el estadio (opcional, máx. 100 caracteres).
        public string? Provincia { get; set; }
    }
}
