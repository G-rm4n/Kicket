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
    public class EventoRepository:IEventoRepository
    {
        private readonly TPIContext context;

        public EventoRepository(TPIContext context)
        {
            this.context = context;
        }

        public async Task<Evento?> GetOneById(int id)
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

        public async Task<bool> DeleteAsync(int id)
        {
            var eventFound = await context.Eventos.FirstOrDefaultAsync(e => e.IdEvento == id);
            if(eventFound is not null)
            {
                context.Eventos.Remove(eventFound);
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> UpdateAsync(Evento evento)
        {
            var eventFound = await context.Eventos.FirstOrDefaultAsync(e => e.IdEvento == evento.IdEvento);
            if(eventFound is not null)
            {
                eventFound.Nombre = evento.Nombre;
                eventFound.Fecha = evento.Fecha;
                eventFound.ClubLocalId = evento.ClubLocalId;
                eventFound.ClubVisitanteId = evento.ClubVisitanteId;
                eventFound.EstadioId = evento.EstadioId;
                eventFound.EstaCancelado = evento.EstaCancelado;

                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
