using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kicket.Contracts.Entradas
{
    // Entrada tal como viaja de la API hacia el cliente.
    public class EntradaDto
    {
        // Id de la entrada.
        public int EntradaId { get; set; }
        // Compra a la que pertenece.
        public int CompraId { get; set; }
        // Evento de la entrada.
        public int EventoId { get; set; }
        // Sector de la entrada.
        public int SectorId { get; set; }

        /// <summary>Puede ser nulo si la entrada es para un sector sin asientos numerados (popular).</summary>
        public string? FilaAsiento { get; set; }
        // Código único que se muestra como QR.
        public string CodigoQr { get; set; } = string.Empty;
    }
}
