using Kicket.Contracts.Reportes;

namespace Core.Interfaces
{
    // Reportes de ventas para administradores: datos, graficos y exportacion.
    public interface IReporteService
    {
        // Arma el reporte completo (indicadores, tablas y graficos SVG) con datos reales.
        Task<ReporteVentasDto> ObtenerReporteVentasAsync(ReporteFiltro filtro, CancellationToken ct = default);
        // Genera el mismo reporte como PDF (QuestPDF).
        Task<byte[]> ExportarPdfAsync(ReporteFiltro filtro, CancellationToken ct = default);
        // Genera el mismo reporte como planilla de Excel (ClosedXML).
        Task<byte[]> ExportarExcelAsync(ReporteFiltro filtro, CancellationToken ct = default);
    }
}
