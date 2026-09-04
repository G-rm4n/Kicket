using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Club
    {
        public int ClubId { get; set; } = 0;
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; } 
        public string Abreviatura { get; set; } = string.Empty;
    }
}
