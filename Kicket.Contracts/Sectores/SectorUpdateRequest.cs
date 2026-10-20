using System.ComponentModel.DataAnnotations;

namespace Kicket.Contracts.Sectores
{
    public class SectorUpdateRequest : SectorRequest
    {
        [Required(ErrorMessage = "El id del sector es obligatorio.")]
        public int SectorId { get; set; }
    }
}
