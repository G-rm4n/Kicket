namespace Kicket.Contracts.Clubes
{
    /// <summary>Club tal como viaja de la API hacia el cliente.</summary>
    public class ClubDto
    {
        public int IdClub { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string LogoUrl { get; set; } = string.Empty;
    }
}
