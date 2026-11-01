namespace Kicket.Contracts.Pagos
{
    // Respuesta al crear la preferencia: id, URL a la que redirigir y el resumen de la compra.
    public class PreferenciaPagoResponse
    {
        // Id de la preferencia (real o "SIM-MP-..." si es simulada).
        public string PreferenceId { get; set; } = string.Empty;
        // URL a la que hay que mandar al usuario para pagar.
        public string RedirectUrl { get; set; } = string.Empty;
        // Total a pagar.
        public decimal MontoTotal { get; set; }
        // true si se está usando el simulador local en lugar de Mercado Pago real.
        public bool EsModoSimulado { get; set; }
        // Datos del resumen de la compra (para mostrar en pantalla y volver a enviar al confirmar).
        public string EventoNombre { get; set; } = string.Empty;
        public string SectorNombre { get; set; } = string.Empty;
        public int Cantidad { get; set; }
        public int UsuarioId { get; set; }
        public int EventoId { get; set; }
        public int SectorId { get; set; }
    }
}
