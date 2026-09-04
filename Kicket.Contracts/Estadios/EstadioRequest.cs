using System.ComponentModel.DataAnnotations;

namespace Kicket.Contracts.Estadios
{
    /// <summary>Datos para dar de alta un estadio (POST /estadios).</summary>
    public class EstadioRequest
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 100 caracteres.")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "La direccion es obligatoria.")]
        [StringLength(100, ErrorMessage = "La direccion no puede superar los 100 caracteres.")]
        public string Direccion { get; set; } = string.Empty;

        [Required(ErrorMessage = "La ciudad es obligatoria.")]
        [StringLength(100, ErrorMessage = "La ciudad no puede superar los 100 caracteres.")]
        public string Ciudad { get; set; } = string.Empty;
    }
}
