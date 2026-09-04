using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Estadio
    {
        public int EstadioId { get; set; } = 0;
        public string Nombre { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;
        public String Ciudad { get; set; } = string.Empty;
    }
}
