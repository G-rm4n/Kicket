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
    // Implementación del repositorio de compras usando EF Core.
    public class CompraRepository : ICompraRepository
    {
        private readonly TPIContext context;

        // Recibe el DbContext por inyección de dependencias.
        public CompraRepository(TPIContext context)
        {
            this.context = context;
        }

        // Trae todas las compras con sus entradas, de la más nueva a la más vieja.
        public async Task<IEnumerable<Compra>> GetAllAsync()
        {
            return await context.Compras
                .Include(c => c.Entradas)
                .OrderByDescending(c => c.FechaCompra)
                .ToListAsync();
        }

        // Busca una compra por id incluyendo sus entradas.
        public async Task<Compra?> GetByIdAsync(int id)
        {
            return await context.Compras
                .Include(c => c.Entradas)
                .FirstOrDefaultAsync(c => c.CompraId == id);
        }

        // Busca la compra con sus entradas; borra primero las entradas y después la compra.
        // Devuelve false si la compra no existe.
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

        // Guarda la compra (EF también inserta las entradas de su lista).
        public async Task AddAsync(Compra compra)
        {
            context.Compras.Add(compra);
            await context.SaveChangesAsync();
        }

        // Cuenta las entradas ya vendidas para ese evento y sector (sirve para controlar el cupo).
        public async Task<int> ObtenerCantidadEntradasVendidasAsync(int eventoId, int sectorId)
        {
            return await context.Entradas
                .CountAsync(e => e.EventoId == eventoId && e.SectorId == sectorId);
        }

        // Devuelve los textos "Fila X, Asiento Y" ocupados en ese evento y sector (para no repetir asientos).
        public async Task<IReadOnlyList<string>> ObtenerUbicacionesOcupadasAsync(int eventoId, int sectorId)
        {
            return await context.Entradas
                .Where(e => e.EventoId == eventoId && e.SectorId == sectorId && e.FilaAsiento != null)
                .Select(e => e.FilaAsiento!)
                .ToListAsync();
        }

        // Paginado genérico de compras: aplica filtro y orden si se pidieron,
        // salta las páginas anteriores (Skip) y toma la cantidad pedida (Take).
        public async Task<IEnumerable<Compra>> AyncGetPaginated(int pagina, int cantidadPorPagina, bool esFiltrado, Expression<Func<Compra, bool>>? filtro = null, bool esOrdenado = false, Func<IQueryable<Compra>, IOrderedQueryable<Compra>>? ordenamiento = null)
        {
            // Arranca la consulta incluyendo las entradas de cada compra.
            IQueryable<Compra> query = context.Set<Compra>().Include(c => c.Entradas);
            // Si se pidió filtrar, aplica la condición.
            if (esFiltrado && filtro is not null)
            {
                query = query.Where(filtro);
            }

            // Si se pidió ordenar, aplica el ordenamiento.
            if (esOrdenado && ordenamiento is not null)
            {
                query = ordenamiento(query);
            }

            // Calcula la página: salta (pagina - 1) * cantidad y toma "cantidad" registros.
            query = query
                .Skip((pagina - 1) * cantidadPorPagina)
                .Take(cantidadPorPagina);

            return await query.ToListAsync();
        }
    }
}
