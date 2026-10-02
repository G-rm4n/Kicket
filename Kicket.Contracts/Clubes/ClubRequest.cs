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

        // Opcional: Club.Descripcion es nullable en el dominio y ClubService no la exige.
        // El tope son los 255 que declara el TPIContext, no 300: con 300 el formulario
        // dejaba pasar un texto que despues fallaba al guardar.
        [StringLength(255, ErrorMessage = "La descripcion no puede superar los 255 caracteres.")]
        public string Descripcion { get; set; } = string.Empty;

        [StringLength(10, ErrorMessage = "La Abreviatura no puede superar los 10 caracteres.")]
        public string Abreviatura { get; set; } = string.Empty;
    }
}
