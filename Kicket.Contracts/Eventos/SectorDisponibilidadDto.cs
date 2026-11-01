using System;

namespace Kicket.Contracts.Eventos
{
    // Disponibilidad de un sector para un evento: precio, capacidad, vendidas y stock.
    public class SectorDisponibilidadDto
    {
        // Id del sector.
        public int SectorId { get; set; }
        // Estadio del sector.
        public int EstadioId { get; set; }
        // Nombre del sector.
        public string Nombre { get; set; } = string.Empty;
        // Precio de cada entrada.
        public decimal PrecioBase { get; set; }
        // Capacidad máxima del sector.
        public int CapacidadTotal { get; set; }
        // Entradas ya vendidas para el evento.
        public int EntradasVendidas { get; set; }
        // Lugares que quedan (capacidad - vendidas).
        public int StockDisponible { get; set; }

        // true si el sector es una platea (tiene asientos numerados).
        public bool EsPlatea => Nombre.Contains("Platea", StringComparison.OrdinalIgnoreCase);
        // true si todavía quedan lugares.
        public bool TieneStock => StockDisponible > 0;
    }
}
