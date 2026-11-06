using Core.Interfaces;
using Core.Reportes;
using Data.Interfaces;
using Kicket.Contracts.Reportes;

namespace Core.Services
{
    /// <summary>
    /// Coordina el reporte de ventas: pide los datos agregados al repositorio ADO.NET, arma los graficos
    /// y delega la exportacion. La web, el PDF y el Excel salen del MISMO ReporteVentasDto, asi no pueden
    /// mostrar numeros distintos.
    /// </summary>
    public class ReporteService : IReporteService
    {
        private readonly IReporteVentasRepository _reportes;
        private readonly IEventoRepository _eventos;

        public ReporteService(IReporteVentasRepository reportes, IEventoRepository eventos)
        {
            _reportes = reportes;
            _eventos = eventos;
        }

        public async Task<ReporteVentasDto> ObtenerReporteVentasAsync(ReporteFiltro filtro, CancellationToken ct = default)
        {
            ArgumentNullException.ThrowIfNull(filtro);
            var errores = filtro.Validar();
            if (errores.Count > 0)
                throw new ArgumentException(string.Join(" ", errores));

            string? eventoNombre = null;
            if (filtro.EventoId.HasValue)
            {
                var evento = await _eventos.GetOneById(filtro.EventoId.Value)
                    ?? throw new ArgumentException("El evento elegido para el reporte no existe.");
                eventoNombre = evento.Nombre;
            }

            // Las consultas se hacen una tras otra: cada una abre su propia conexion ADO.NET.
            var reporte = new ReporteVentasDto
            {
                GeneradoEn = DateTime.Now,
                Desde = filtro.Desde?.Date,
                Hasta = filtro.Hasta?.Date,
                EventoNombre = eventoNombre,
                Resumen = await _reportes.ObtenerResumenAsync(filtro, ct),
                PorEvento = await _reportes.ObtenerVentasPorEventoAsync(filtro, ct),
                PorClub = await _reportes.ObtenerVentasPorClubAsync(filtro, ct),
                PorPeriodo = await _reportes.ObtenerVentasPorPeriodoAsync(filtro, ct),
                PorSector = await _reportes.ObtenerVentasPorSectorAsync(filtro, ct)
            };
            reporte.Graficos = ArmarGraficos(reporte);
            return reporte;
        }

        public async Task<byte[]> ExportarPdfAsync(ReporteFiltro filtro, CancellationToken ct = default) =>
            ExportadorPdf.Generar(await ObtenerReporteVentasAsync(filtro, ct));

        public async Task<byte[]> ExportarExcelAsync(ReporteFiltro filtro, CancellationToken ct = default) =>
            ExportadorExcel.Generar(await ObtenerReporteVentasAsync(filtro, ct));

        // Solo graficos con valor informativo: ranking por partido, evolucion diaria, reparto por sector y por club.
        private static List<GraficoDto> ArmarGraficos(ReporteVentasDto r) => new()
        {
            new GraficoDto
            {
                Titulo = "Entradas vendidas por partido",
                Svg = GraficosSvg.BarrasHorizontales(
                    r.PorEvento.OrderByDescending(e => e.Entradas).Take(10)
                        .Select(e => (e.Evento, (decimal)e.Entradas)).ToList(), esMoneda: false)
            },
            new GraficoDto
            {
                Titulo = "Evolución de la recaudación por día",
                Svg = GraficosSvg.Linea(r.PorPeriodo.Select(p => (p.Dia, p.Recaudado)).ToList(), esMoneda: true)
            },
            new GraficoDto
            {
                Titulo = "Distribución de entradas por sector",
                Svg = GraficosSvg.Dona(r.PorSector.Where(s => s.Entradas > 0)
                    .Select(s => ($"{s.Sector} ({s.Estadio})", (decimal)s.Entradas)).ToList())
            },
            new GraficoDto
            {
                Titulo = "Recaudación por club (como local)",
                Svg = GraficosSvg.BarrasHorizontales(
                    r.PorClub.Where(c => c.Recaudado > 0).Select(c => (c.Club, c.Recaudado)).ToList(), esMoneda: true)
            }
        };
    }
}
