using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface ICompraRepository
    {
        Task AddAsync(Compra compra);
        Task<int> ObtenerCantidadEntradasVendidasAsync(int eventoId, int sectorId);
        Task<IReadOnlyList<string>> ObtenerUbicacionesOcupadasAsync(int eventoId, int sectorId);
        Task<Compra?> GetByIdAsync(int id);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<Compra>> GetAllAsync();

        Task<IEnumerable<Compra>> AyncGetPaginated(int pagina, int cantidadPorPagina, bool esFiltrado, Expression<Func<Compra, bool>>? filtro = null, bool esOrdenado = false, Func<IQueryable<Compra>, IOrderedQueryable<Compra>>? ordenamiento = null);
    }
}
