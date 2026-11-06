namespace Kicket.Contracts.Compras
{
    /// <summary>
    /// Un partido de "Mis Entradas" con todas las entradas que el usuario tiene para el,
    /// aunque vengan de compras distintas. Cada partido aparece una sola vez.
    /// </summary>
    public class PartidoConEntradasDto
    {
        public int EventoId { get; set; }
        public string EventoNombre { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
        public string ClubLocal { get; set; } = string.Empty;
        public string ClubVisitante { get; set; } = string.Empty;
        public string EstadioNombre { get; set; } = string.Empty;
        public string EstadioCiudad { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public bool EstaCancelado { get; set; }
        // Totales del grupo.
        public int CantidadEntradas { get; set; }
        public int CantidadCompras { get; set; }
        public decimal TotalPagado { get; set; }
        // Fecha de la compra mas reciente del grupo (es la que se usa para ordenar por compra).
        public DateTime UltimaCompra { get; set; }
        public List<MiEntradaDto> Entradas { get; set; } = new();

        // Estado derivado: no se guarda en la base.
        public bool EsPasado => !EstaCancelado && Fecha <= DateTime.Now;
        public string Estado => EstaCancelado ? "Cancelado" : EsPasado ? "Finalizado" : "Próximo";
    }

    /// <summary>
    /// Entrada vista desde "Mis Entradas". No trae FilaAsiento a proposito: el profesor pidio no
    /// mostrar fila ni asiento en esta pantalla. El dato sigue existiendo en la base y en CompraDto.
    /// </summary>
    public class MiEntradaDto
    {
        public int EntradaId { get; set; }
        public int CompraId { get; set; }
        public DateTime FechaCompra { get; set; }
        public string SectorNombre { get; set; } = string.Empty;
        public string? EstadoPago { get; set; }
        public string? MetodoPago { get; set; }
        public string CodigoQr { get; set; } = string.Empty;
    }
}
