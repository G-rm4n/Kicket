using Kicket.Contracts.Reportes;

namespace Kicket.ApiClient.Abstracciones
{
    // Reportes de ventas (solo Admin). La web y el escritorio usan el mismo cliente:
    // la lógica del reporte vive una sola vez en la API.
    public interface IReporteApiClient
    {
        // GET /reportes/ventas: indicadores, tablas y gráficos SVG.
        Task<ReporteVentasDto> GetVentasAsync(ReporteFiltro filtro, CancellationToken ct = default);
        // GET /reportes/ventas/pdf: el reporte en PDF.
        Task<byte[]> DescargarPdfAsync(ReporteFiltro filtro, CancellationToken ct = default);
        // GET /reportes/ventas/xlsx: el reporte en Excel.
        Task<byte[]> DescargarExcelAsync(ReporteFiltro filtro, CancellationToken ct = default);
    }
}
