using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kicket.Contracts.Sectores
{
    public class SectorUpdateRequest : SectorRequest
    {
        [Range(1, int.MaxValue, ErrorMessage = "El id del sector es obligatorio.")]
        public int SectorId { get; set; }
    }
}
