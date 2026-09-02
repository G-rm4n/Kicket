using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface ICompraService
    {
        Task<bool> GenerarCompraAsync(int usuarioId, int eventoId, int sectorId, int cantidad);
    }
}
