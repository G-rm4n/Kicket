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

        // Nombre armado "Apellido, Nombre" para mostrar en grillas.
        public string NombreCompleto => $"{Apellido}, {Nombre}";
    }
}
