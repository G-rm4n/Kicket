using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Implementaciones
{
    public class EstadioRepository:IEstadioRepository
    {
        private readonly TPIContext context;

        public EstadioRepository(TPIContext context)
        {
            this.context = context;
        }

        public async Task<Estadio?> GetByIdAsync(int id)
        {
            return await context.Estadios.FirstOrDefaultAsync(e => e.EstadioId == id);
        }

        public async Task<IEnumerable<Estadio>> GetAllAsync()
        {
            return await context.Estadios.ToListAsync();
        }

        public async Task AddAsync(Estadio estadio)
        {
            context.Estadios.Add(estadio);
            await context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var estadioFound = await context.Estadios.FirstOrDefaultAsync(e => e.EstadioId == id);
            if(estadioFound is not null)
            {
                context.Estadios.Remove(estadioFound);
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> UpdateAsync(Estadio estadio)
        {
            var estadioFound = await context.Estadios.FirstOrDefaultAsync(e => e.EstadioId == estadio.EstadioId);
            if(estadioFound is not null)
            {
                estadioFound.Direccion = estadio.Direccion;
                estadioFound.Ciudad = estadio.Ciudad;
                estadioFound.Nombre = estadio.Nombre;

                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
