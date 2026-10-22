namespace Kicket.Contracts.Pagos
{
    public class PreferenciaPagoResponse
    {
        public string PreferenceId { get; set; } = string.Empty;
        public string RedirectUrl { get; set; } = string.Empty;
        public decimal MontoTotal { get; set; }
        public bool EsModoSimulado { get; set; }
        public string EventoNombre { get; set; } = string.Empty;
        public string SectorNombre { get; set; } = string.Empty;
        public int Cantidad { get; set; }
        public int UsuarioId { get; set; }
        public int EventoId { get; set; }
        public int SectorId { get; set; }
    }
}
