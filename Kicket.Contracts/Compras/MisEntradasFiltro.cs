using Kicket.Contracts.Common;

namespace Kicket.Contracts.Compras
{
    /// <summary>
    /// Filtros de GET /compras/mis-entradas. El usuario NO viaja aca: la API lo saca del token.
    /// Se pagina por partido (cada pagina trae N partidos con todas sus entradas), no por entrada suelta.
    /// </summary>
    public class MisEntradasFiltro : PagedRequest
    {
        // Ordenes posibles.
        public const string OrdenPartidoProximo = "partido_asc";     // por defecto: el proximo partido primero
        public const string OrdenPartidoLejano = "partido_desc";
        public const string OrdenCompraReciente = "compra_desc";      // usa la compra MAS RECIENTE del grupo
        public const string OrdenCompraAntigua = "compra_asc";

        // Estados de la vista (se deducen de la fecha y de EstaCancelado, no se guardan).
        public const string EstadoTodos = "todos";
        public const string EstadoProximos = "proximos";
        public const string EstadoPasados = "pasados";
        public const string EstadoCancelados = "cancelados";
        public static readonly string[] Estados = { EstadoTodos, EstadoProximos, EstadoPasados, EstadoCancelados };

        // Rango de fecha del partido.
        public DateTime? Desde { get; set; }
        public DateTime? Hasta { get; set; }
        public string? Estado { get; set; }

        public override IReadOnlyCollection<string> OrdenesPermitidos =>
            new[] { OrdenPartidoProximo, OrdenPartidoLejano, OrdenCompraReciente, OrdenCompraAntigua };

        public override List<string> Validar()
        {
            var errores = base.Validar();
            ValidarRango(errores, Desde, Hasta, "la fecha del partido");
            if (!string.IsNullOrWhiteSpace(Estado) && !Estados.Contains(Estado, StringComparer.OrdinalIgnoreCase))
                errores.Add($"Estado invalido. Valores validos: {string.Join(", ", Estados)}.");
            return errores;
        }
    }
}
