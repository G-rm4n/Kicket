using System.ComponentModel.DataAnnotations;

namespace Kicket.Contracts.Clubes
{
    /// <summary>
    /// Datos para dar de alta un club (POST /clubes).
    /// Las anotaciones las valida la API y tambien las puede usar el formulario de
    /// escritorio antes de mandar la request: una sola definicion, dos usos.
    /// </summary>
    public class ClubRequest
    {
        // Id del club (en el alta no se usa; lo genera la base).
        public int Id { get; set; }

        // Nombre: obligatorio, entre 2 y 100 caracteres.
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 100 caracteres.")]
        public string Nombre { get; set; } = string.Empty;

        // Descripción: obligatoria en los formularios, hasta 255 caracteres (largo de la columna).
        [Required(ErrorMessage = "La descripcion es obligatoria.")]
        [StringLength(255, ErrorMessage = "La descripcion no puede superar los 255 caracteres.")]
        public string Descripcion { get; set; } = string.Empty;

        // Abreviatura: opcional, hasta 10 caracteres.
        [StringLength(10, ErrorMessage = "La Abreviatura no puede superar los 10 caracteres.")]
        public string Abreviatura { get; set; } = string.Empty;

        // Ciudad: opcional, hasta 100 caracteres.
        [StringLength(100, ErrorMessage = "La ciudad no puede superar los 100 caracteres.")]
        public string? Ciudad { get; set; }

        // Año de fundacion: opcional (la API controla que no sea futuro).
        [Range(1850, 2100, ErrorMessage = "El año de fundacion debe estar entre 1850 y el año actual.")]
        public int? AnioFundacion { get; set; }
    }
}
