using System.ComponentModel.DataAnnotations;

namespace Kicket.Contracts.Usuarios
{
    /// <summary>
    /// Datos para modificar un usuario (PUT /usuarios, el id va en el cuerpo).
    /// Pass es opcional: si viene vacio, la API conserva la contrasena actual
    /// en lugar de borrarla.
    /// </summary>
    public class UsuarioUpdateRequest
    {
        // Id del usuario a modificar.
        [Required(ErrorMessage = "El id del usuario es obligatorio.")]
        public int IdUsuario { get; set; }

        // Nombre: obligatorio, entre 2 y 60 caracteres.
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(60, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 60 caracteres.")]
        public string Nombre { get; set; } = string.Empty;

        // Apellido: obligatorio, entre 2 y 60 caracteres.
        [Required(ErrorMessage = "El apellido es obligatorio.")]
        [StringLength(60, MinimumLength = 2, ErrorMessage = "El apellido debe tener entre 2 y 60 caracteres.")]
        public string Apellido { get; set; } = string.Empty;

        // Email: obligatorio, con formato válido y hasta 150 caracteres.
        [Required(ErrorMessage = "El email es obligatorio.")]
        [EmailAddress(ErrorMessage = "El email no tiene un formato valido.")]
        [StringLength(150, ErrorMessage = "El email no puede superar los 150 caracteres.")]
        public string Email { get; set; } = string.Empty;

        // Contraseña nueva (opcional): si viene, al menos 6 caracteres.
        [StringLength(100, MinimumLength = 6, ErrorMessage = "La contrasena debe tener al menos 6 caracteres.")]
        public string? Pass { get; set; }

        // Rol: obligatorio.
        [Required(ErrorMessage = "El rol es obligatorio.")]
        public string Rol { get; set; } = Roles.Usuario;
    }
}
