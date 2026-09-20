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
    public class SectorRepository : ISectorRepository
    {
        private readonly TPIContext _context;

        public SectorRepository(TPIContext context)
        {
            _context = context;
        }

        public async Task AddSectorAsync(Sector sector)
        {
            _context.Add(sector);
            await _context.SaveChangesAsync();
        }

        public async Task<Sector?> ObtenerSectorPorIdAsync(int sectorId)
        {
            return await _context.Sectores.FirstOrDefaultAsync(s => s.SectorId == sectorId);
        }

        public async Task<IEnumerable<Sector>> GetAll()
        {
            return await _context.Sectores.ToListAsync();
        }

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

        public async Task<bool> UpdateSectorAsync(Sector sector)
        {
            var sectorFound = await _context.Sectores.FirstOrDefaultAsync(s => s.SectorId == sector.SectorId);
            if (sectorFound != null)
            {
                sectorFound.PrecioBase=sector.PrecioBase;
                sectorFound.Nombre=sector.Nombre;
                sectorFound.CapacidadMaxima=sector.CapacidadMaxima;
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
