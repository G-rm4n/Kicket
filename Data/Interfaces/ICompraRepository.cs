using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Data.Interfaces
{
    public interface ICompraRepository
    {
        Task AgregarCompraAsync(Compra compra);
        Task<int> ObtenerCantidadEntradasVendidasAsync(int eventoId, int sectorId);
    }
}
