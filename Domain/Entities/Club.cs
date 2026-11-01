using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    // Entidad Club: representa un club de fútbol que puede jugar como local o visitante en un evento.
    public class Club
    {
        // Clave primaria del club (la genera la base de datos).
        public int ClubId { get; set; } = 0;
        // Nombre completo del club (máx. 100 caracteres en la BD).
        public string Nombre { get; set; } = string.Empty;
        // Descripción opcional del club (máx. 255 caracteres).
        public string? Descripcion { get; set; } 
        // Sigla corta del club, por ejemplo "NOB" (máx. 10 caracteres).
        public string Abreviatura { get; set; } = string.Empty;
    }
}
