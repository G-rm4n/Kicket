namespace Kicket.Contracts.Usuarios
{
    /// <summary>
    /// Usuario tal como viaja de la API hacia el cliente.
    /// No incluye Pass a proposito: la contrasena nunca sale de la API.
    /// </summary>
    public class UsuarioDto
    {
        // Id del usuario.
        public int IdUsuario { get; set; }
        // Nombre.
        public string Nombre { get; set; } = string.Empty;
        // Apellido.
        public string Apellido { get; set; } = string.Empty;
        // Email.
        public string Email { get; set; } = string.Empty;
        // Rol del usuario.
        public string Rol { get; set; } = Roles.Usuario;
        // DNI normalizado (solo digitos). Puede ser null en usuarios creados antes de la v2.
        public string? Dni { get; set; }
        // Telefono de contacto (opcional).
        public string? Telefono { get; set; }
        // Fecha de nacimiento (opcional).
        public DateTime? FechaNacimiento { get; set; }
        // Fecha de alta en el sistema.
        public DateTime FechaRegistro { get; set; }

        // Nombre armado "Apellido, Nombre" para mostrar en grillas.
        public string NombreCompleto => $"{Apellido}, {Nombre}";
    }
}
