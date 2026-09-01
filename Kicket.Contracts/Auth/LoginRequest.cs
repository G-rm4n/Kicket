using System.ComponentModel.DataAnnotations;

namespace Kicket.Contracts.Auth
{
    /// <summary>Credenciales que manda la pantalla de login.</summary>
    public class LoginRequest
    {
        [Required(ErrorMessage = "El email es obligatorio.")]
        [EmailAddress(ErrorMessage = "El email no tiene un formato valido.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contrasena es obligatoria.")]
        public string Pass { get; set; } = string.Empty;
    }
}
