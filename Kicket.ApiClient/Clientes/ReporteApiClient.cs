using Kicket.ApiClient.Abstracciones;
using Kicket.ApiClient.Http;
using Kicket.Contracts.Reportes;

namespace Kicket.ApiClient.Clientes
{
    // Cliente HTTP tipado para los reportes de ventas.
    public class ReporteApiClient : ApiClientBase, IReporteApiClient
    {
        // Prefijo de las rutas de reportes en la API.
        private const string Ruta = "reportes/ventas";
        // Recibe el HttpClient configurado.
        public ReporteApiClient(HttpClient http) : base(http) { }
        // Pide el reporte con sus datos y gráficos.
        public Task<ReporteVentasDto> GetVentasAsync(ReporteFiltro filtro, CancellationToken ct = default) =>
            GetAsync<ReporteVentasDto>(ConQuery(Ruta, filtro), ct);
        // Descarga el PDF.
        public Task<byte[]> DescargarPdfAsync(ReporteFiltro filtro, CancellationToken ct = default) =>
            GetBytesAsync(ConQuery($"{Ruta}/pdf", filtro), ct);
        // Descarga el Excel.
        public Task<byte[]> DescargarExcelAsync(ReporteFiltro filtro, CancellationToken ct = default) =>
            GetBytesAsync(ConQuery($"{Ruta}/xlsx", filtro), ct);
    }
}
