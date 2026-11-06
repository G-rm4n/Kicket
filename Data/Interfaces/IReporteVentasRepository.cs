using Kicket.Contracts.Reportes;

namespace Data.Interfaces
{
    // Consultas de lectura agregadas para los reportes de ventas. Se implementan con ADO.NET puro
    // (SqlConnection / SqlCommand / SqlDataReader), no con EF Core.
    public interface IReporteVentasRepository
    {
        // Totales generales del periodo.
        Task<ResumenVentasDto> ObtenerResumenAsync(ReporteFiltro filtro, CancellationToken ct = default);
        // Ventas agrupadas por partido.
        Task<List<VentaEventoDto>> ObtenerVentasPorEventoAsync(ReporteFiltro filtro, CancellationToken ct = default);
        // Ventas agrupadas por club (atribuidas al club local).
        Task<List<VentaClubDto>> ObtenerVentasPorClubAsync(ReporteFiltro filtro, CancellationToken ct = default);
        // Ventas agrupadas por dia de compra.
        Task<List<VentaPeriodoDto>> ObtenerVentasPorPeriodoAsync(ReporteFiltro filtro, CancellationToken ct = default);
        // Ventas agrupadas por sector de estadio.
        Task<List<VentaSectorDto>> ObtenerVentasPorSectorAsync(ReporteFiltro filtro, CancellationToken ct = default);
    }
}
