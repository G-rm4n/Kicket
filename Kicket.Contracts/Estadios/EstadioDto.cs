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
        // Provincia (opcional).
        public string? Provincia { get; set; }
        // Suma de la capacidad de sus sectores (dato calculado, no se guarda).
        public int CapacidadTotal { get; set; }
        // Cantidad de sectores del estadio (dato calculado).
        public int CantidadSectores { get; set; }

        /// <summary>Direccion armada, para mostrar directo en una grilla.</summary>
        public string DireccionCompleta => $"{Direccion} {Ciudad}";
    }
}
