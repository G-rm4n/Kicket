using Kicket.Contracts.Common;

namespace Kicket.Contracts.Eventos
{
    /// <summary>
    /// Filtros de GET /eventos/paginado. Busqueda: nombre del evento o nombre de cualquiera de los dos clubes.
    /// El estado no es una columna: "proximos" y "pasados" se deducen de la fecha.
    /// </summary>
    public class EventoFiltro : PagedRequest
    {
        // Valores de Estado.
        public const string EstadoTodos = "todos";
        public const string EstadoProximos = "proximos";   // no cancelados con fecha futura (se pueden vender)
        public const string EstadoPasados = "pasados";     // no cancelados con fecha ya pasada
        public const string EstadoCancelados = "cancelados";
        public static readonly string[] Estados = { EstadoTodos, EstadoProximos, EstadoPasados, EstadoCancelados };

        // Club que juega, como local o visitante.
        public int? ClubId { get; set; }
        public int? ClubLocalId { get; set; }
        public int? ClubVisitanteId { get; set; }
        public int? EstadioId { get; set; }
        // Rango de fecha del partido (dias completos).
        public DateTime? Desde { get; set; }
        public DateTime? Hasta { get; set; }
        public string? Estado { get; set; }

        public override IReadOnlyCollection<string> OrdenesPermitidos => new[] { "id", "fecha", "nombre" };

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
