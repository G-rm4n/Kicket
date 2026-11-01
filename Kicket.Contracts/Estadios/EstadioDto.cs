namespace Kicket.Contracts.Estadios
{
    /// <summary>Estadio tal como viaja de la API hacia el cliente.</summary>
    public class EstadioDto
    {
        // Id del estadio.
        public int IdEstadio { get; set; }
        // Nombre del estadio.
        public string Nombre { get; set; } = string.Empty;
        // Dirección.
        public string Direccion { get; set; }=string.Empty;
        // Ciudad.
        public string Ciudad { get; set; } = string.Empty;

        /// <summary>Direccion armada, para mostrar directo en una grilla.</summary>
        public string DireccionCompleta => $"{Direccion} {Ciudad}";
    }
}
