using System.ComponentModel.DataAnnotations;

namespace Kicket.Contracts.Estadios
{
    /// <summary>Datos para modificar un estadio. El id va en el cuerpo (PUT /estadios).</summary>
    public class EstadioUpdateRequest : EstadioRequest
    {
        [Range(1, int.MaxValue, ErrorMessage = "El id del estadio es obligatorio.")]
        public int IdEstadio { get; set; }
    }
}
