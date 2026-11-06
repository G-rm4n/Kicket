using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    // Entidad Usuario: persona registrada en el sistema, con rol "Usuario" o "Admin".
    public class Usuario
    {
        // Clave primaria del usuario.
        public int IdUsuario { get; set; }
        // Nombre de pila.
        public string Nombre { get; set; } = string.Empty;
        // Apellido.
        public string Apellido { get; set; } = string.Empty;
        // Email, usado para iniciar sesión (único en la BD).
        public string Email { get; set; } = string.Empty;
        // Contraseña guardada como hash BCrypt.
        public string Password { get; set; } = string.Empty; // Hash BCrypt, nunca la contraseña en texto plano
        // Fecha de alta del usuario.
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
        // Rol del usuario ("Usuario" o "Admin"); la BD pone "Usuario" por defecto.
        public string Rol { get; set; } = string.Empty;
        // DNI normalizado (solo dígitos, como texto para no perder ceros). Único cuando está cargado;
        // puede ser null en usuarios creados antes de la versión 2 (no se inventan DNI).
        public string? Dni { get; set; }
        // Teléfono de contacto (opcional).
        public string? Telefono { get; set; }
        // Fecha de nacimiento (opcional).
        public DateTime? FechaNacimiento { get; set; }

    }
}
