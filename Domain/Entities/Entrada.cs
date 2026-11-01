using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    // Entidad Entrada: un ticket individual para un evento y sector, con su código QR.
    public class Entrada
    {
        // Clave primaria de la entrada.
        public int EntradaId { get; set; }
        // FK a la compra a la que pertenece.
        public int CompraId { get; set; }
        // FK al evento para el que sirve la entrada.
        public int EventoId { get; set; }
        // FK al sector del estadio.
        public int SectorId { get; set; }
        // Ubicación "Fila X, Asiento Y" (solo para plateas; en populares queda null).
        public string? FilaAsiento { get; set; } //puede ser nulo si la entrada es para el sector popular
        // Código único "KCKT-..." que se muestra como QR.
        public string CodigoQr { get; set; } = string.Empty;
    }
}
