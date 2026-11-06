using System.Data;
using Data.Interfaces;
using Kicket.Contracts.Reportes;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Data.Implementaciones
{
    /// <summary>
    /// Reportes de ventas con ADO.NET. Se eligio ADO.NET para este caso porque son consultas de solo
    /// lectura con agregaciones (GROUP BY, SUM, COUNT DISTINCT, CTE) que se escriben y controlan mejor
    /// en SQL directo, y se leen fila por fila con SqlDataReader sin cargar entidades.
    ///
    /// Seguridad: todos los valores que vienen del usuario viajan como SqlParameter (nunca se concatenan),
    /// y la cadena de conexion es la MISMA que usa EF Core (se toma del TPIContext).
    /// </summary>
    public class ReporteVentasRepository : IReporteVentasRepository
    {
        // Solo cuentan como venta las compras con este estado de pago.
        public const string EstadoConfirmado = "Aprobado";

        private readonly string _cadenaConexion;

        // Recibe el contexto de EF solo para reutilizar su cadena de conexion.
        public ReporteVentasRepository(TPIContext context)
        {
            _cadenaConexion = context.Database.GetConnectionString()
                ?? throw new InvalidOperationException("No hay cadena de conexion configurada.");
        }

        // CTE base: una fila por compra confirmada, con su evento, sector y cantidad de entradas.
        // Una compra siempre es de un solo evento y sector (asi la arma CompraService), por eso MIN() no mezcla datos.
        // Agrupar primero por compra evita sumar MontoTotal una vez por cada entrada al hacer el JOIN.
        private const string CteCompras = @"
WITH CompraVenta AS (
    SELECT c.CompraId, c.UsuarioId, c.FechaCompra, c.MontoTotal,
           MIN(e.EventoId) AS EventoId, MIN(e.SectorId) AS SectorId, COUNT(*) AS Entradas
    FROM dbo.Compras c
    INNER JOIN dbo.Entradas e ON e.CompraId = c.CompraId
    WHERE c.EstadoPago = @estado
      AND (@desde IS NULL OR c.FechaCompra >= @desde)
      AND (@hasta IS NULL OR c.FechaCompra < @hasta)
    GROUP BY c.CompraId, c.UsuarioId, c.FechaCompra, c.MontoTotal
    HAVING (@eventoId IS NULL OR MIN(e.EventoId) = @eventoId)
)";

        public async Task<ResumenVentasDto> ObtenerResumenAsync(ReporteFiltro filtro, CancellationToken ct = default)
        {
            // Tres SELECT en un solo comando: se leen como tres result sets con NextResultAsync.
            string sql = CteCompras + @"
SELECT COUNT(*), ISNULL(SUM(Entradas), 0), ISNULL(SUM(MontoTotal), 0),
       COUNT(DISTINCT EventoId), COUNT(DISTINCT UsuarioId)
FROM CompraVenta;

SELECT COUNT(*) FROM dbo.Eventos;

SELECT COUNT(*) FROM dbo.Compras c
WHERE ISNULL(c.EstadoPago, '') <> @estado
  AND (@desde IS NULL OR c.FechaCompra >= @desde)
  AND (@hasta IS NULL OR c.FechaCompra < @hasta);";

            var resumen = new ResumenVentasDto();
            await using var conexion = new SqlConnection(_cadenaConexion);
            await conexion.OpenAsync(ct);
            await using var comando = CrearComando(conexion, sql, filtro);
            await using var lector = await comando.ExecuteReaderAsync(ct);

            if (await lector.ReadAsync(ct))
            {
                resumen.ComprasConfirmadas = lector.GetInt32(0);
                resumen.EntradasVendidas = lector.GetInt32(1);
                resumen.TotalRecaudado = lector.GetDecimal(2);
                resumen.EventosConVentas = lector.GetInt32(3);
                resumen.CompradoresDistintos = lector.GetInt32(4);
            }
            if (await lector.NextResultAsync(ct) && await lector.ReadAsync(ct))
            {
                resumen.EventosTotales = lector.GetInt32(0);
            }
            if (await lector.NextResultAsync(ct) && await lector.ReadAsync(ct))
            {
                resumen.ComprasNoConfirmadas = lector.GetInt32(0);
            }

            resumen.TicketPromedio = resumen.ComprasConfirmadas == 0
                ? 0
                : Math.Round(resumen.TotalRecaudado / resumen.ComprasConfirmadas, 2);
            return resumen;
        }

        public async Task<List<VentaEventoDto>> ObtenerVentasPorEventoAsync(ReporteFiltro filtro, CancellationToken ct = default)
        {
            string sql = CteCompras + @"
SELECT ev.IdEvento, ev.Nombre, ev.Fecha, es.Nombre,
       COUNT(cv.CompraId), ISNULL(SUM(cv.Entradas), 0), ISNULL(SUM(cv.MontoTotal), 0)
FROM CompraVenta cv
INNER JOIN dbo.Eventos ev ON ev.IdEvento = cv.EventoId
INNER JOIN dbo.Estadios es ON es.EstadioId = ev.EstadioId
GROUP BY ev.IdEvento, ev.Nombre, ev.Fecha, es.Nombre
ORDER BY 7 DESC, ev.IdEvento;";

            return await LeerListaAsync(sql, filtro, l => new VentaEventoDto
            {
                EventoId = l.GetInt32(0),
                Evento = l.GetString(1),
                Fecha = l.GetDateTime(2),
                Estadio = l.GetString(3),
                Compras = l.GetInt32(4),
                Entradas = l.GetInt32(5),
                Recaudado = l.GetDecimal(6)
            }, ct);
        }

        public async Task<List<VentaClubDto>> ObtenerVentasPorClubAsync(ReporteFiltro filtro, CancellationToken ct = default)
        {
            // Regla: la venta se atribuye al club LOCAL del partido. Asi cada peso se cuenta una sola vez
            // (si se sumara a local y visitante, el total por club seria el doble del total real).
            string sql = CteCompras + @",
VentaEvento AS (
    SELECT EventoId, SUM(Entradas) AS Entradas, SUM(MontoTotal) AS Recaudado
    FROM CompraVenta GROUP BY EventoId
)
SELECT cl.ClubId, cl.Nombre,
       (SELECT COUNT(*) FROM dbo.Eventos x WHERE x.ClubLocalId = cl.ClubId),
       (SELECT COUNT(*) FROM dbo.Eventos x WHERE x.ClubVisitanteId = cl.ClubId),
       ISNULL(SUM(ve.Entradas), 0), ISNULL(SUM(ve.Recaudado), 0)
FROM dbo.Clubs cl
LEFT JOIN dbo.Eventos ev ON ev.ClubLocalId = cl.ClubId
LEFT JOIN VentaEvento ve ON ve.EventoId = ev.IdEvento
GROUP BY cl.ClubId, cl.Nombre
ORDER BY 6 DESC, cl.Nombre;";

            return await LeerListaAsync(sql, filtro, l => new VentaClubDto
            {
                ClubId = l.GetInt32(0),
                Club = l.GetString(1),
                EventosComoLocal = l.GetInt32(2),
                EventosComoVisitante = l.GetInt32(3),
                Entradas = l.GetInt32(4),
                Recaudado = l.GetDecimal(5)
            }, ct);
        }

        public async Task<List<VentaPeriodoDto>> ObtenerVentasPorPeriodoAsync(ReporteFiltro filtro, CancellationToken ct = default)
        {
            string sql = CteCompras + @"
SELECT CAST(FechaCompra AS date), COUNT(*), SUM(Entradas), SUM(MontoTotal)
FROM CompraVenta
GROUP BY CAST(FechaCompra AS date)
ORDER BY 1;";

            return await LeerListaAsync(sql, filtro, l => new VentaPeriodoDto
            {
                Dia = l.GetDateTime(0),
                Compras = l.GetInt32(1),
                Entradas = l.GetInt32(2),
                Recaudado = l.GetDecimal(3)
            }, ct);
        }

        public async Task<List<VentaSectorDto>> ObtenerVentasPorSectorAsync(ReporteFiltro filtro, CancellationToken ct = default)
        {
            string sql = CteCompras + @"
SELECT s.SectorId, es.Nombre, s.Nombre, s.CapacidadMaxima,
       ISNULL(SUM(cv.Entradas), 0), ISNULL(SUM(cv.MontoTotal), 0)
FROM dbo.Sectores s
INNER JOIN dbo.Estadios es ON es.EstadioId = s.EstadioId
LEFT JOIN CompraVenta cv ON cv.SectorId = s.SectorId
GROUP BY s.SectorId, es.Nombre, s.Nombre, s.CapacidadMaxima
ORDER BY es.Nombre, s.Nombre;";

            return await LeerListaAsync(sql, filtro, l => new VentaSectorDto
            {
                SectorId = l.GetInt32(0),
                Estadio = l.GetString(1),
                Sector = l.GetString(2),
                CapacidadPorEvento = l.GetInt32(3),
                Entradas = l.GetInt32(4),
                Recaudado = l.GetDecimal(5)
            }, ct);
        }

        // Abre la conexion, ejecuta la consulta y convierte cada fila con la funcion recibida.
        private async Task<List<T>> LeerListaAsync<T>(string sql, ReporteFiltro filtro, Func<SqlDataReader, T> leerFila, CancellationToken ct)
        {
            var resultado = new List<T>();
            await using var conexion = new SqlConnection(_cadenaConexion);
            await conexion.OpenAsync(ct);
            await using var comando = CrearComando(conexion, sql, filtro);
            await using var lector = await comando.ExecuteReaderAsync(ct);
            while (await lector.ReadAsync(ct))
            {
                resultado.Add(leerFila(lector));
            }
            return resultado;
        }

        // Arma el SqlCommand con los parametros del filtro. Los nulos viajan como DBNull,
        // y la consulta los trata con "@param IS NULL OR ...".
        private static SqlCommand CrearComando(SqlConnection conexion, string sql, ReporteFiltro filtro)
        {
            var comando = new SqlCommand(sql, conexion) { CommandType = CommandType.Text };
            comando.Parameters.Add(new SqlParameter("@estado", SqlDbType.NVarChar, 50) { Value = EstadoConfirmado });
            comando.Parameters.Add(new SqlParameter("@desde", SqlDbType.DateTime2)
            {
                Value = filtro.Desde.HasValue ? (object)filtro.Desde.Value.Date : DBNull.Value
            });
            // "Hasta" incluye el dia completo: se compara contra las 00:00 del dia siguiente.
            comando.Parameters.Add(new SqlParameter("@hasta", SqlDbType.DateTime2)
            {
                Value = filtro.Hasta.HasValue ? (object)filtro.Hasta.Value.Date.AddDays(1) : DBNull.Value
            });
            comando.Parameters.Add(new SqlParameter("@eventoId", SqlDbType.Int)
            {
                Value = filtro.EventoId.HasValue ? (object)filtro.EventoId.Value : DBNull.Value
            });
            return comando;
        }
    }
}
