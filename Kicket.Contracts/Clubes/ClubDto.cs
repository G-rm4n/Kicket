namespace Kicket.Contracts.Clubes
{
    /// <summary>Club tal como viaja de la API hacia el cliente.</summary>
    public class ClubDto
    {
        // Id del club.
        public int ClubId { get; set; }
        // Nombre del club.
        public string Nombre { get; set; } = string.Empty;
        // Descripción opcional.
        public string? Descripcion { get; set; }
        // Sigla del club.
        public string Abreviatura { get; set; } = string.Empty;
    }
}
