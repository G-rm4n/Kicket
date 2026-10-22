
namespace Kiketpropio.Dominio.DTOs
{
    public class EstadiosPorCiudad
    {
        public string Ciudad { get; set; }
        public int Cantidad { get; set; }
        public string Estadios { get; set; }

        public EstadiosPorCiudad()
        {
            Ciudad = string.Empty;
            Estadios = string.Empty;
        }

        public EstadiosPorCiudad(string ciudad, int cantidad, string estadios)
        {
            Ciudad = ciudad;
            Cantidad = cantidad;
            Estadios = estadios;
        }
    }
}
