

namespace Kiketpropio.Dominio.Entidades
{
    public class Estadio
    {
        public int IdEstadio { get; set; }
        public string Nombre { get; set; }
        public string Calle { get; set; }
        public int Nro { get; set; }
        public string Ciudad { get; set; }

        public Estadio(int idEstadio, string nombre, string calle, int nro, string ciudad)
        {
            IdEstadio = idEstadio;
            Nombre = nombre;
            Calle = calle;
            Nro = nro;
            Ciudad = ciudad;
        }
    }
}
