using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Core.Interfaces
{
    public interface ICompraService
    {
        Task<Compra> GenerarCompraAsync(int usuarioId, int eventoId, int sectorId, int cantidad);
        Task<Compra?> ObtenerPorIdAsync(int id);
        Task<IEnumerable<Compra>> ObtenerTodosAsync();
        Task<bool> EliminarCompraAsync(int id);
    }
}