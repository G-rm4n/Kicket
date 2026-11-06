using Kicket.Contracts.Common;

namespace Kicket.Contracts.Compras
{
    /// <summary>Filtros de GET /compras/paginado (solo Admin). Busqueda: id de pago o email del comprador.</summary>
    public class CompraFiltro : PagedRequest
    {
        public int? CompraId { get; set; }
        public int? UsuarioId { get; set; }
        // Compras que tienen entradas de este evento.
        public int? EventoId { get; set; }
        // Rango de la fecha de compra (dias completos).
        public DateTime? Desde { get; set; }
        public DateTime? Hasta { get; set; }
        // Rango de la fecha del partido.
        public DateTime? PartidoDesde { get; set; }
        public DateTime? PartidoHasta { get; set; }
        // Estado del pago tal como se guarda ("Aprobado", ...).
        public string? EstadoPago { get; set; }

        public override IReadOnlyCollection<string> OrdenesPermitidos => new[] { "id", "fecha", "monto", "cantidad" };

        public override List<string> Validar()
        {
            var errores = base.Validar();
            ValidarRango(errores, Desde, Hasta, "la fecha de compra");
            ValidarRango(errores, PartidoDesde, PartidoHasta, "la fecha del partido");
            return errores;
        }
    }
}
