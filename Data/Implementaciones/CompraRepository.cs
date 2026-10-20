using Data.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Data.Implementaciones
{
    public class CompraRepository : ICompraRepository
    {
        private readonly TPIContext context;

        public CompraRepository(TPIContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<Compra>> GetAllAsync()
        {
            return await context.Compras
                .Include(c => c.Entradas)
                .OrderByDescending(c => c.FechaCompra)
                .ToListAsync();
        }

        public async Task<Compra?> GetByIdAsync(int id)
        {
            return await context.Compras
                .Include(c => c.Entradas)
                .FirstOrDefaultAsync(c => c.CompraId == id);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var compraFound = await context.Compras
                .Include(c => c.Entradas)
                .FirstOrDefaultAsync(e => e.CompraId == id);

            if (compraFound is not null)
            {
                if (compraFound.Entradas.Any())
                {
                    context.Entradas.RemoveRange(compraFound.Entradas);
                }
                context.Compras.Remove(compraFound);
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task AddAsync(Compra compra)
        {
            context.Compras.Add(compra);
            await context.SaveChangesAsync();
        }

        public async Task<int> ObtenerCantidadEntradasVendidasAsync(int eventoId, int sectorId)
        {
            return await context.Entradas
                .CountAsync(e => e.EventoId == eventoId && e.SectorId == sectorId);
        }

        public async Task<IReadOnlyList<string>> ObtenerUbicacionesOcupadasAsync(int eventoId, int sectorId)
        {
            return await context.Entradas
                .Where(e => e.EventoId == eventoId && e.SectorId == sectorId && e.FilaAsiento != null)
                .Select(e => e.FilaAsiento!)
                .ToListAsync();
        }

        public async Task<IEnumerable<Compra>> AyncGetPaginated(int pagina, int cantidadPorPagina, bool esFiltrado, Expression<Func<Compra, bool>>? filtro = null, bool esOrdenado = false, Func<IQueryable<Compra>, IOrderedQueryable<Compra>>? ordenamiento = null)
        {
            IQueryable<Compra> query = context.Set<Compra>().Include(c => c.Entradas);
            if (esFiltrado && filtro is not null)
            {
                query = query.Where(filtro);
            }

            if (esOrdenado && ordenamiento is not null)
            {
                query = ordenamiento(query);
            }

            query = query
                .Skip((pagina - 1) * cantidadPorPagina)
                .Take(cantidadPorPagina);

            return await query.ToListAsync();
        }
    }
}
