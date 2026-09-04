namespace Kicket.Contracts.Clubes
{
    /// <summary>Club tal como viaja de la API hacia el cliente.</summary>
    public class ClubDto
    {
        public int ClubId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public string Abreviatura { get; set; } = string.Empty;
    }
}
