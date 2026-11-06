using Core.Interfaces;
using Kicket.Contracts.Reportes;

namespace WebApi.EndPoints
{
    // Endpoints de reportes de ventas. Todo el grupo es solo para Admin: la autorización está en la API,
    // no alcanza con ocultar el botón en la pantalla.
    public static class ReporteEndPoints
    {
        private const string TipoXlsx = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        public static void MapReporteEndPoints(this WebApplication app)
        {
            var reportesGroup = app.MapGroup("/reportes")
                                   .RequireAuthorization("Solo admin");

            // GET /reportes/ventas?desde=2026-01-01&hasta=2026-12-31&eventoId=-2 : datos + gráficos SVG.
            reportesGroup.MapGet("/ventas", async ([AsParameters] ReporteFiltro filtro, IReporteService reportes, CancellationToken ct) =>
                Results.Ok(await reportes.ObtenerReporteVentasAsync(filtro, ct)))
            .WithName("GetReporteVentas")
            .Produces<ReporteVentasDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);

            // GET /reportes/ventas/pdf : el mismo reporte como archivo PDF.
            reportesGroup.MapGet("/ventas/pdf", async ([AsParameters] ReporteFiltro filtro, IReporteService reportes, CancellationToken ct) =>
                Results.File(await reportes.ExportarPdfAsync(filtro, ct), "application/pdf", NombreArchivo("pdf")))
            .WithName("GetReporteVentasPdf")
            .Produces(StatusCodes.Status200OK, contentType: "application/pdf");

            // GET /reportes/ventas/xlsx : el mismo reporte como planilla de Excel.
            reportesGroup.MapGet("/ventas/xlsx", async ([AsParameters] ReporteFiltro filtro, IReporteService reportes, CancellationToken ct) =>
                Results.File(await reportes.ExportarExcelAsync(filtro, ct), TipoXlsx, NombreArchivo("xlsx")))
            .WithName("GetReporteVentasXlsx")
            .Produces(StatusCodes.Status200OK, contentType: TipoXlsx);
        }

        private static string NombreArchivo(string extension) => $"kicket-ventas-{DateTime.Now:yyyyMMdd-HHmm}.{extension}";
    }
}
