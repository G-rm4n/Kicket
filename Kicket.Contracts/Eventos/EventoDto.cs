using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kicket.Contracts.Eventos
{
    // Evento tal como viaja de la API hacia el cliente.
    public class EventoDto
    {
        // Id del evento.
        public int IdEvento { get; set; }
        // Nombre del evento.
        public string Nombre { get; set; } = string.Empty;
        // Fecha y hora.
        public DateTime Fecha { get; set; }
        // Estadio donde se juega.
        public int IdEstadio { get; set; }
        // Club local.
        public int IdEquipoLocal { get; set; }
        // Club visitante.
        public int IdEquipoVisitante { get; set; }
        // Si fue cancelado.
        public bool EstaCancelado { get; set; }
        // Descripcion / informacion para el comprador (opcional).
        public string? Descripcion { get; set; }
        // Nombres resueltos (los completan los listados paginados; en el resto pueden venir vacios).
        public string? NombreEquipoLocal { get; set; }
        public string? NombreEquipoVisitante { get; set; }
        public string? NombreEstadio { get; set; }
        // Propiedad calculada: true si no está cancelado y la fecha es futura.
        public bool Activo => !EstaCancelado && Fecha > DateTime.Now;
    }
}
