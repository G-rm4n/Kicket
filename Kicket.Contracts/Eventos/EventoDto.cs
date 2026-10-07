using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kicket.Contracts.Eventos
{
    public class EventoDto
    {
        public int IdEvento { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
        public int IdEstadio { get; set; }
        public int IdEquipoLocal { get; set; }
        public int IdEquipoVisitante { get; set; }
        public bool EstaCancelado { get; set; }
        public bool Activo => !EstaCancelado && Fecha > DateTime.Now;
    }
}
