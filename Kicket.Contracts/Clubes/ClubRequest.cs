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
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 100 caracteres.")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "La URL del logo es obligatoria.")]
        [StringLength(300, ErrorMessage = "La Descripccion no puede superar los 300 caracteres.")]
        public string Descripcion { get; set; } = string.Empty;

        [StringLength(300, ErrorMessage = "La Abreviatura no puede superar los 300 caracteres.")]
        public string Abreviatura { get; set; } = string.Empty;
    }
}
