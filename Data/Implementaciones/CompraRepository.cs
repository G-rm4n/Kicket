using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Data.Interfaces;

namespace Data.Implementaciones
{
    public class CompraRepository : ICompraRepository
    {
        public Task AgregarCompraAsync(Compra compra)
        {
            throw new NotImplementedException();
        }

        public Task<int> ObtenerCantidadEntradasVendidasAsync(int eventoId, int sectorId)
        {
            throw new NotImplementedException();
        }
    }
}
