
namespace Kiketpropio.Dominio.DTOs
{
    public class LoginResponse
    {
        public string Token { get; set; }
        public string Email { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Rol { get; set; }
        public DateTime Expiracion { get; set; }

        public LoginResponse()
        {
            Token = string.Empty;
            Email = string.Empty;
            Nombre = string.Empty;
            Apellido = string.Empty;
            Rol = string.Empty;
        }

        public LoginResponse(string token, string email, string nombre, string apellido, string rol, DateTime expiracion)
        {
            Token = token;
            Email = email;
            Nombre = nombre;
            Apellido = apellido;
            Rol = rol;
            Expiracion = expiracion;
        }
    }
}
