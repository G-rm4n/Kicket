using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty; //La password en un futuro debe estar hasheada
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
        public string Rol { get; set; } = string.Empty;

    }
}
