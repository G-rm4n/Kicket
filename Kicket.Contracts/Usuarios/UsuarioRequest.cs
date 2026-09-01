using System.ComponentModel.DataAnnotations;

namespace Kicket.Contracts.Usuarios
{
    /// <summary>Datos para dar de alta un usuario (POST /usuarios).</summary>
    public class UsuarioRequest
    {
        [Required(ErrorMessage = "El DNI es obligatorio.")]
        [RegularExpression(@"^\d{7,9}$", ErrorMessage = "El DNI debe tener entre 7 y 9 digitos.")]
        public string DNI { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(60, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 60 caracteres.")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El apellido es obligatorio.")]
        [StringLength(60, MinimumLength = 2, ErrorMessage = "El apellido debe tener entre 2 y 60 caracteres.")]
        public string Apellido { get; set; } = string.Empty;

        [Required(ErrorMessage = "El email es obligatorio.")]
        [EmailAddress(ErrorMessage = "El email no tiene un formato valido.")]
        [StringLength(150, ErrorMessage = "El email no puede superar los 150 caracteres.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contrasena es obligatoria.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "La contrasena debe tener al menos 6 caracteres.")]
        public string Pass { get; set; } = string.Empty;

        [Required(ErrorMessage = "La fecha de nacimiento es obligatoria.")]
        public DateTime FechaNacimiento { get; set; }

        [Required(ErrorMessage = "El rol es obligatorio.")]
        public string Rol { get; set; } = Roles.Usuario;
    }
}
