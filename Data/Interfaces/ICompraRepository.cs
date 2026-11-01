using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    // Contrato del repositorio de compras.
    public interface ICompraRepository
    {
        // Guarda una compra nueva (junto con sus entradas).
        Task AddAsync(Compra compra);
        // Cuenta cuántas entradas se vendieron para un evento y sector.
        Task<int> ObtenerCantidadEntradasVendidasAsync(int eventoId, int sectorId);
        // Devuelve las ubicaciones (fila/asiento) ya ocupadas en un evento y sector.
        Task<IReadOnlyList<string>> ObtenerUbicacionesOcupadasAsync(int eventoId, int sectorId);
        // Busca una compra por id, con sus entradas.
        Task<Compra?> GetByIdAsync(int id);
        // Borra una compra y sus entradas; false si no existe.
        Task<bool> DeleteAsync(int id);
        // Devuelve todas las compras.
        Task<IEnumerable<Compra>> GetAllAsync();

        // Devuelve una página de compras, con filtro y orden opcionales.
        Task<IEnumerable<Compra>> AyncGetPaginated(int pagina, int cantidadPorPagina, bool esFiltrado, Expression<Func<Compra, bool>>? filtro = null, bool esOrdenado = false, Func<IQueryable<Compra>, IOrderedQueryable<Compra>>? ordenamiento = null);
    }
}
