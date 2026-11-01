using Data.Interfaces;
using Domain.Entities;
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