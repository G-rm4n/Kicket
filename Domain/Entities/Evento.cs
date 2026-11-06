using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    // Entidad Evento: un partido entre un club local y uno visitante, en un estadio y fecha dados.
    public class Evento
    {
        // Clave primaria del evento.
        public int IdEvento { get; set; }
        // Nombre del evento (ej. "Clásico Rosarino").
        public string Nombre { get; set; } = string.Empty;
        // Fecha y hora del partido.
        public DateTime Fecha { get; set; }
        // FK al estadio donde se juega.
        public int EstadioId { get; set; }
        // FK al club local.
        public int ClubLocalId { get; set; }
        // FK al club visitante.
        public int ClubVisitanteId { get; set; }
        // Indica si el evento fue cancelado (por defecto false).
        public bool EstaCancelado { get; set; }
        // Información para el comprador (horario de apertura, recomendaciones...). Opcional, máx. 500.
        public string? Descripcion { get; set; }
        // Devuelve true si el evento se puede vender: no está cancelado y la fecha todavía no pasó.
        public bool EstaDisponible()
        {
            return !EstaCancelado && Fecha > DateTime.Now;
        }

    }
}
