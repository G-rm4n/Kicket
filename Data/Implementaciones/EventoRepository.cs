using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Data.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Implementaciones
{
    // Implementación del repositorio de eventos usando EF Core.
    public class EventoRepository:IEventoRepository
    {
        private readonly TPIContext context;

        // Recibe el DbContext por inyección de dependencias.
        public EventoRepository(TPIContext context)
        {
            this.context = context;
        }

        // Paginado genérico de eventos: filtra y ordena si se pidió, y aplica Skip/Take.
        public async Task<IEnumerable<Evento>> AyncGetPaginated(int pagina,int cantidadPorPagina,bool esFiltrado,Expression<Func<Evento, bool>>? filtro = null,bool esOrdenado = false,Func<IQueryable<Evento>, IOrderedQueryable<Evento>>? ordenamiento = null)
        {
            // Arranca con todos los eventos como IQueryable (todavía no consulta la BD).
            IQueryable<Evento> query = context.Set<Evento>();
            // Si se pidió filtrar, aplica la condición.
            if (esFiltrado && filtro is not null)
            {
                query=query.Where(filtro);
            }

            // Si se pidió ordenar, aplica el ordenamiento.
            if(esOrdenado &&  ordenamiento is not null)
            {
                query = ordenamiento(query);
            }

            // Saltea las páginas anteriores y toma la cantidad de la página actual.
            query = query
                .Skip((pagina - 1) * cantidadPorPagina)
                .Take(cantidadPorPagina);

            // Recién acá se ejecuta la consulta contra la BD.
            return await query.ToListAsync();
        }
        // Busca un evento por id.
        public async Task<Evento?> GetOneById(int id)
        {
            return await context.Eventos.FirstOrDefaultAsync(e => e.IdEvento == id);
        }

        // Trae todos los eventos.
        public async Task<IEnumerable<Evento>> GetAllAsync()
        {
            return await context.Eventos.ToListAsync();
        }

        // Agrega un evento y guarda.
        public async Task AddAsync(Evento evento)
        {
            context.Eventos.Add(evento);
            await context.SaveChangesAsync();
        }

        // Busca el evento; si existe lo borra y devuelve true; si no, false.
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

        // Busca el evento existente, copia todos los campos editables (incluido si está cancelado) y guarda.
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
