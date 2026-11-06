namespace Kicket.Contracts.Reportes
{
    /// <summary>
    /// Filtros del reporte de ventas (GET /reportes/ventas, /pdf y /xlsx). Todos opcionales.
    /// Solo se cuentan las compras con EstadoPago = "Aprobado" (ver ReporteVentasDto.CriterioVentas).
    /// </summary>
    public class ReporteFiltro
    {
        // Rango de la fecha de compra (dias completos).
        public DateTime? Desde { get; set; }
        public DateTime? Hasta { get; set; }
        // Limita el reporte a un solo partido.
        public int? EventoId { get; set; }

        public List<string> Validar()
        {
            var errores = new List<string>();
            if (Desde.HasValue && Hasta.HasValue && Desde.Value.Date > Hasta.Value.Date)
                errores.Add("La fecha 'desde' no puede ser posterior a la fecha 'hasta'.");
            return errores;
        }
    }

    /// <summary>Reporte completo: indicadores, tablas y graficos (SVG generados en el servidor).</summary>
    public class ReporteVentasDto
    {
        // Regla de negocio de que es una venta, para mostrarla en pantalla y en los archivos.
        public const string CriterioVentas =
            "Se consideran ventas confirmadas solo las compras con estado de pago \"Aprobado\". " +
            "La recaudacion se toma del monto total de cada compra (no de las entradas), asi no se suma dos veces.";

        public DateTime GeneradoEn { get; set; }
        public DateTime? Desde { get; set; }
        public DateTime? Hasta { get; set; }
        public string? EventoNombre { get; set; }
        public ResumenVentasDto Resumen { get; set; } = new();
        public List<VentaEventoDto> PorEvento { get; set; } = new();
        public List<VentaClubDto> PorClub { get; set; } = new();
        public List<VentaPeriodoDto> PorPeriodo { get; set; } = new();
        public List<VentaSectorDto> PorSector { get; set; } = new();
        public List<GraficoDto> Graficos { get; set; } = new();
    }

    public class ResumenVentasDto
    {
        // Eventos que tuvieron al menos una venta en el periodo.
        public int EventosConVentas { get; set; }
        // Eventos totales cargados en el sistema (no depende del filtro de fechas).
        public int EventosTotales { get; set; }
        public int ComprasConfirmadas { get; set; }
        public int EntradasVendidas { get; set; }
        public decimal TotalRecaudado { get; set; }
        public decimal TicketPromedio { get; set; }
        public int CompradoresDistintos { get; set; }
        // Compras con otro estado de pago (no suman a la recaudacion).
        public int ComprasNoConfirmadas { get; set; }
    }

    public class VentaEventoDto
    {
        public int EventoId { get; set; }
        public string Evento { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
        public string Estadio { get; set; } = string.Empty;
        public int Compras { get; set; }
        public int Entradas { get; set; }
        public decimal Recaudado { get; set; }
    }

    /// <summary>Ventas por club. Regla: cada venta se atribuye al club LOCAL del partido, para no contarla dos veces.</summary>
    public class VentaClubDto
    {
        public int ClubId { get; set; }
        public string Club { get; set; } = string.Empty;
        public int EventosComoLocal { get; set; }
        public int EventosComoVisitante { get; set; }
        public int Entradas { get; set; }
        public decimal Recaudado { get; set; }
    }

    public class VentaPeriodoDto
    {
        public DateTime Dia { get; set; }
        public int Compras { get; set; }
        public int Entradas { get; set; }
        public decimal Recaudado { get; set; }
    }

    public class VentaSectorDto
    {
        public int SectorId { get; set; }
        public string Estadio { get; set; } = string.Empty;
        public string Sector { get; set; } = string.Empty;
        public int CapacidadPorEvento { get; set; }
        public int Entradas { get; set; }
        public decimal Recaudado { get; set; }
    }

    /// <summary>Grafico listo para dibujar: el SVG lo arma el servidor y es el mismo que va al PDF.</summary>
    public class GraficoDto
    {
        public string Titulo { get; set; } = string.Empty;
        public string Svg { get; set; } = string.Empty;
    }
}
