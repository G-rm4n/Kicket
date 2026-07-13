
namespace Kiketpropio.Dominio.Entidades
{
    public class Club
    {
        public int IdClub { get; set; }
        public string Nombre { get; set; }
        public string Logo_Url { get; set; }

        public Club(int idClub, string nombre, string logo_Url)
        {
            IdClub = idClub;
            Nombre = nombre;
            Logo_Url = logo_Url;
        }
    }
}
