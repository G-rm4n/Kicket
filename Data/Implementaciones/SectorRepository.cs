using Data.Consultas;
using Data.Interfaces;
using Domain.Entities;
using Kicket.Contracts.Common;
using Kicket.Contracts.Sectores;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Implementaciones
{
    // Implementación del repositorio de sectores usando EF Core.
    public class SectorRepository : ISectorRepository
    {
        private readonly TPIContext _context;

        // Recibe el DbContext por inyección de dependencias.
        public SectorRepository(TPIContext context)
        {
            _context = context;
        }

        // Agrega un sector y guarda.
        public async Task AddSectorAsync(Sector sector)
        {
            _context.Add(sector);
            await _context.SaveChangesAsync();
        }

        // Busca un sector por id.
        public async Task<Sector?> ObtenerSectorPorIdAsync(int sectorId)
        {
            return await _context.Sectores.FirstOrDefaultAsync(s => s.SectorId == sectorId);
        }

        // Trae todos los sectores.
        public async Task<IEnumerable<Sector>> GetAll()
        {
            return await _context.Sectores.ToListAsync();
        }

        // Trae los sectores que pertenecen a un estadio.
        public async Task<IEnumerable<Sector>> ObtenerPorEstadioAsync(int estadioId)
        {
            return await _context.Sectores.Where(s => s.EstadioId == estadioId).ToListAsync();
        }

        // Pagina de sectores con el nombre del estadio. El tipo (Platea/Popular) no es columna:
        // se filtra con la misma regla que usa CompraService (el nombre contiene "Platea").
        public async Task<PagedResult<SectorDto>> ObtenerPaginadoAsync(SectorFiltro filtro, CancellationToken ct = default)
        {
            var consulta = from s in _context.Sectores.AsNoTracking()
                           join e in _context.Estadios on s.EstadioId equals e.EstadioId
                           select new { s, Estadio = e.Nombre };

            if (filtro.EstadioId.HasValue)
            {
                int estadioId = filtro.EstadioId.Value;
                consulta = consulta.Where(x => x.s.EstadioId == estadioId);
            }
            if (filtro.Busqueda is not null)
            {
                var texto = filtro.Busqueda;
                consulta = consulta.Where(x => x.s.Nombre.Contains(texto) || x.Estadio.Contains(texto));
            }
            if (string.Equals(filtro.Tipo, SectorFiltro.TipoPlatea, StringComparison.OrdinalIgnoreCase))
            {
                consulta = consulta.Where(x => x.s.Nombre.Contains("Platea"));
            }
            else if (string.Equals(filtro.Tipo, SectorFiltro.TipoPopular, StringComparison.OrdinalIgnoreCase))
            {
                consulta = consulta.Where(x => !x.s.Nombre.Contains("Platea"));
            }
            if (filtro.CapacidadMinima.HasValue)
            {
                int minima = filtro.CapacidadMinima.Value;
                consulta = consulta.Where(x => x.s.CapacidadMaxima >= minima);
            }

            bool desc = filtro.EsDescendente;
            var ordenada = filtro.OrdenarPor switch
            {
                "id" => consulta.OrdenarPor(x => x.s.SectorId, desc),
                "capacidad" => consulta.OrdenarPor(x => x.s.CapacidadMaxima, desc),
                "precio" => consulta.OrdenarPor(x => x.s.PrecioBase, desc),
                _ => consulta.OrdenarPor(x => x.s.Nombre, desc)
            };

            return await ordenada.LuegoPor(x => x.s.SectorId, desc)
                .Select(x => new SectorDto
                {
                    SectorId = x.s.SectorId,
                    EstadioId = x.s.EstadioId,
                    Nombre = x.s.Nombre,
                    CapacidadMaxima = x.s.CapacidadMaxima,
                    PrecioBase = x.s.PrecioBase,
                    NombreEstadio = x.Estadio
                })
                .PaginarAsync(filtro, ct);
        }

        // Busca el sector; si existe lo borra y devuelve true; si no, false.
        // Si el sector tiene entradas vendidas, SaveChanges tira DbUpdateException (FK Restrict).
        public async Task<bool> DeleteSectorAsync(int idSector)
        {
            var sectorFound = await _context.Sectores.FirstOrDefaultAsync(s => s.SectorId == idSector);
            if (sectorFound != null)
            {
                _context.Remove(sectorFound);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        // Busca el sector existente, copia precio, nombre, capacidad y estadio, y guarda. False si no existe.
        public async Task<bool> UpdateSectorAsync(Sector sector)
        {
            var sectorFound = await _context.Sectores.FirstOrDefaultAsync(s => s.SectorId == sector.SectorId);
            if (sectorFound != null)
            {
                sectorFound.PrecioBase = sector.PrecioBase;
                sectorFound.Nombre = sector.Nombre;
                sectorFound.CapacidadMaxima = sector.CapacidadMaxima;
                sectorFound.EstadioId = sector.EstadioId;
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
