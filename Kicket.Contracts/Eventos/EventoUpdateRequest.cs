using System.ComponentModel.DataAnnotations;

namespace Kicket.Contracts.Eventos
{
    /// <summary>Datos para modificar un evento. El id va en el cuerpo (PUT /eventos).</summary>
    public class EventoUpdateRequest : EventoRequest
    {
        [Range(1, int.MaxValue, ErrorMessage = "El id del evento es obligatorio.")]
        public int IdEvento { get; set; }

        /// <summary>Permite cancelar (o reactivar) el evento desde la misma pantalla de edicion.</summary>
        public bool EstaCancelado { get; set; }
    }
}