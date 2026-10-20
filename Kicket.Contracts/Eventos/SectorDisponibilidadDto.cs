using System;

namespace Kicket.Contracts.Eventos
{
    public class SectorDisponibilidadDto
    {
        public int SectorId { get; set; }
        public int EstadioId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public decimal PrecioBase { get; set; }
        public int CapacidadTotal { get; set; }
        public int EntradasVendidas { get; set; }
        public int StockDisponible { get; set; }

        public bool EsPlatea => Nombre.Contains("Platea", StringComparison.OrdinalIgnoreCase);
        public bool TieneStock => StockDisponible > 0;
    }
}
