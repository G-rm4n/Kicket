using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kicket.Contracts.Sectores
{
    public class SectorRequest
    {
        [Range(1, int.MaxValue, ErrorMessage = "El estadio es obligatorio.")]
        public int EstadioId { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 100 caracteres.")]
        public string Nombre { get; set; } = string.Empty;

        [Range(1, int.MaxValue, ErrorMessage = "La capacidad maxima debe ser mayor a cero.")]
        public int CapacidadMaxima { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "El precio base debe ser mayor a cero.")]
        public decimal PrecioBase { get; set; }
    }
}
