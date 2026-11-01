using System.ComponentModel.DataAnnotations;

namespace Kicket.Contracts.Sectores
{
    // Datos para modificar un sector (PUT /sectores); hereda los campos del alta y agrega el id.
    public class SectorUpdateRequest : SectorRequest
    {
        // Id del sector a modificar.
        [Required(ErrorMessage = "El id del sector es obligatorio.")]
        public int SectorId { get; set; }
    }
}
