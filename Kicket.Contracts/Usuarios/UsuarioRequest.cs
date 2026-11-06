using System.ComponentModel.DataAnnotations;

namespace Kicket.Contracts.Usuarios
{
    /// <summary>Datos para dar de alta un usuario (POST /usuarios).</summary>
    public class UsuarioRequest
    {

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

        // Contraseña: obligatoria, al menos 6 caracteres.
        [Required(ErrorMessage = "La contrasena es obligatoria.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "La contrasena debe tener al menos 6 caracteres.")]
        public string Pass { get; set; } = string.Empty;

        // Rol: obligatorio; por defecto "Usuario".
        [Required(ErrorMessage = "El rol es obligatorio.")]
        public string Rol { get; set; } = Roles.Usuario;

        // DNI: obligatorio en las altas nuevas (registro y alta por Admin).
        [Required(ErrorMessage = "El DNI es obligatorio.")]
        [RegularExpression(Usuarios.Dni.Patron, ErrorMessage = Usuarios.Dni.MensajeInvalido)]
        public string Dni { get; set; } = string.Empty;

        // Telefono: opcional.
        [RegularExpression(@"^[0-9+()\- ]{6,30}$", ErrorMessage = "El telefono solo puede tener numeros, espacios, +, - y parentesis (6 a 30 caracteres).")]
        public string? Telefono { get; set; }

        // Fecha de nacimiento: opcional (la API controla que no sea futura).
        public DateTime? FechaNacimiento { get; set; }
    }
}
