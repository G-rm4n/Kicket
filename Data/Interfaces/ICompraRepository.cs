using Domain.Entities;
using Kicket.Contracts.Common;
using Kicket.Contracts.Compras;
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
        // Compras de un usuario (filtradas en la base, no en memoria).
        Task<IEnumerable<Compra>> ObtenerPorUsuarioAsync(int usuarioId);

        // Pagina de compras para administracion, con nombres de usuario, evento y sector.
        Task<PagedResult<CompraDto>> ObtenerPaginadoAsync(CompraFiltro filtro, CancellationToken ct = default);

        // "Mis Entradas": pagina de PARTIDOS del usuario, cada uno con todas sus entradas.
        Task<PagedResult<PartidoConEntradasDto>> ObtenerMisEntradasAsync(int usuarioId, MisEntradasFiltro filtro, CancellationToken ct = default);
    }
}
