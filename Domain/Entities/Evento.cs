using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Evento
    {
        public int IdEvento { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
        public int EstadioId { get; set; }
        public int ClubLocalId { get; set; }
        public int ClubVisitanteId { get; set; }
        public bool EstaCancelado { get; set; }
        public bool EstaDisponible()
        {
            return !EstaCancelado && Fecha > DateTime.Now;
        }

    }
}
