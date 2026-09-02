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

        public async Task<Evento?> GetByIdAsync(int id)
        {
            return await context.Eventos.FirstOrDefaultAsync(e => e.IdEvento == id);
        }

        public async Task<IEnumerable<Evento>> GetAllAsync()
        {
            return await context.Eventos.ToListAsync();
        }

        public async Task AddAsync(Evento evento)
        {
            context.Eventos.Add(evento);
            await context.SaveChangesAsync();
        }
    }
}
