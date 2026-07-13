

namespace Kiketpropio.Dominio.Entidades
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public string DNI { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public string Pass { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Rol { get; set; }

        public Usuario(int idUsuario, string dNI, string nombre, string apellido, string email, string pass, DateTime fechaNacimiento, string rol)
        {
            IdUsuario = idUsuario;
            DNI = dNI;
            Nombre = nombre;
            Apellido = apellido;
            Email = email;
            Pass = pass;
            FechaNacimiento = fechaNacimiento;
            Rol = rol;
        }
    }
}
