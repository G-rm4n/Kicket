
namespace Kiketpropio.Dominio.DTOs
{
    public class UsuariosPorRol
    {
        public string Rol { get; set; }
        public int Cantidad { get; set; }
        public int EdadPromedio { get; set; }

        public UsuariosPorRol()
        {
            Rol = string.Empty;
        }

        public UsuariosPorRol(string rol, int cantidad, int edadPromedio)
        {
            Rol = rol;
            Cantidad = cantidad;
            EdadPromedio = edadPromedio;
        }
    }
}
