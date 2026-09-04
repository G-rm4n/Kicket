namespace Kicket.Contracts.Usuarios
{
    /// <summary>
    /// Usuario tal como viaja de la API hacia el cliente.
    /// No incluye Pass a proposito: la contrasena nunca sale de la API.
    /// </summary>
    public class UsuarioDto
    {
        public int IdUsuario { get; set; }
        public string DNI { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime FechaNacimiento { get; set; }
        public string Rol { get; set; } = Roles.Usuario;

        public string NombreCompleto => $"{Apellido}, {Nombre}";
    }
}
