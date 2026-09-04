namespace Kicket.Contracts.Estadios
{
    /// <summary>Estadio tal como viaja de la API hacia el cliente.</summary>
    public class EstadioDto
    {
        public int IdEstadio { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Direccion { get; set; }=string.Empty;
        public string Ciudad { get; set; } = string.Empty;

        /// <summary>Direccion armada, para mostrar directo en una grilla.</summary>
        public string DireccionCompleta => $"{Direccion} {Ciudad}";
    }
}
