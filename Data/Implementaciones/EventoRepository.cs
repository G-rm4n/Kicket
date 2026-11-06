using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Consultas;
using Data.Interfaces;
using Domain.Entities;
using Kicket.Contracts.Common;
using Kicket.Contracts.Eventos;
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

        // Pagina de eventos: une Eventos con Clubs (local y visitante) y Estadios para poder buscar
        // y mostrar por nombre. El estado (proximo/pasado) se deduce de la fecha, no se guarda.
        public async Task<PagedResult<EventoDto>> ObtenerPaginadoAsync(EventoFiltro filtro, CancellationToken ct = default)
        {
            var ahora = DateTime.Now;
            var consulta = from e in context.Eventos.AsNoTracking()
                           join local in context.Clubs on e.ClubLocalId equals local.ClubId
                           join visitante in context.Clubs on e.ClubVisitanteId equals visitante.ClubId
                           join estadio in context.Estadios on e.EstadioId equals estadio.EstadioId
                           select new { e, Local = local.Nombre, Visitante = visitante.Nombre, Estadio = estadio.Nombre };

            if (filtro.Busqueda is not null)
            {
                var texto = filtro.Busqueda;
                consulta = consulta.Where(x => x.e.Nombre.Contains(texto) || x.Local.Contains(texto)
                                               || x.Visitante.Contains(texto) || x.Estadio.Contains(texto));
            }
            if (filtro.ClubId.HasValue)
            {
                int clubId = filtro.ClubId.Value;
                consulta = consulta.Where(x => x.e.ClubLocalId == clubId || x.e.ClubVisitanteId == clubId);
            }
            if (filtro.ClubLocalId.HasValue)
            {
                int localId = filtro.ClubLocalId.Value;
                consulta = consulta.Where(x => x.e.ClubLocalId == localId);
            }
            if (filtro.ClubVisitanteId.HasValue)
            {
                int visitanteId = filtro.ClubVisitanteId.Value;
                consulta = consulta.Where(x => x.e.ClubVisitanteId == visitanteId);
            }
            if (filtro.EstadioId.HasValue)
            {
                int estadioId = filtro.EstadioId.Value;
                consulta = consulta.Where(x => x.e.EstadioId == estadioId);
            }
            // Rango de fechas por dias completos: desde las 00:00 del "desde" hasta antes de las 00:00 del dia siguiente al "hasta".
            if (filtro.Desde.HasValue)
            {
                var desde = filtro.Desde.Value.Date;
                consulta = consulta.Where(x => x.e.Fecha >= desde);
            }
            if (filtro.Hasta.HasValue)
            {
                var hastaExclusivo = filtro.Hasta.Value.Date.AddDays(1);
                consulta = consulta.Where(x => x.e.Fecha < hastaExclusivo);
            }
            consulta = filtro.Estado?.ToLowerInvariant() switch
            {
                EventoFiltro.EstadoProximos => consulta.Where(x => !x.e.EstaCancelado && x.e.Fecha > ahora),
                EventoFiltro.EstadoPasados => consulta.Where(x => !x.e.EstaCancelado && x.e.Fecha <= ahora),
                EventoFiltro.EstadoCancelados => consulta.Where(x => x.e.EstaCancelado),
                _ => consulta
            };

            bool desc = filtro.EsDescendente;
            var ordenada = filtro.OrdenarPor switch
            {
                "id" => consulta.OrdenarPor(x => x.e.IdEvento, desc),
                "nombre" => consulta.OrdenarPor(x => x.e.Nombre, desc),
                _ => consulta.OrdenarPor(x => x.e.Fecha, desc)
            };

            return await ordenada.LuegoPor(x => x.e.IdEvento, desc)
                .Select(x => new EventoDto
                {
                    IdEvento = x.e.IdEvento,
                    Nombre = x.e.Nombre,
                    Fecha = x.e.Fecha,
                    IdEstadio = x.e.EstadioId,
                    IdEquipoLocal = x.e.ClubLocalId,
                    IdEquipoVisitante = x.e.ClubVisitanteId,
                    EstaCancelado = x.e.EstaCancelado,
                    Descripcion = x.e.Descripcion,
                    NombreEquipoLocal = x.Local,
                    NombreEquipoVisitante = x.Visitante,
                    NombreEstadio = x.Estadio
                })
                .PaginarAsync(filtro, ct);
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
                eventFound.Descripcion = evento.Descripcion;

                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
