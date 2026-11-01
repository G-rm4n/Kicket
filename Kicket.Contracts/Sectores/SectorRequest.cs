using System.ComponentModel.DataAnnotations;

namespace Kicket.Contracts.Sectores
{
    // Datos para dar de alta un sector (POST /sectores).
    public class SectorRequest
    {
        // Estadio al que pertenece el sector.
        [Required(ErrorMessage = "El estadio es obligatorio.")]
        public int EstadioId { get; set; }

        // Nombre: obligatorio, entre 2 y 100 caracteres.
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 100 caracteres.")]
        public string Nombre { get; set; } = string.Empty;

        // Capacidad: mayor a cero.
        [Range(1, int.MaxValue, ErrorMessage = "La capacidad maxima debe ser mayor a cero.")]
        public int CapacidadMaxima { get; set; }

        // Precio: mayor a cero.
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio base debe ser mayor a cero.")]
        public decimal PrecioBase { get; set; }
    }
}
