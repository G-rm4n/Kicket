using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Evento
    {
        public bool Activo;

        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
    }
}
