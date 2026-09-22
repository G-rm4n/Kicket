using Core.Interfaces;
using Data.Interfaces;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public class SectorService : ISectorService
    {
        private readonly ISectorRepository _sectorRepository;
        private readonly IEstadioRepository _estadioRepository;

        public SectorService(ISectorRepository sectorRepository, IEstadioRepository estadioRepository)
        {
            _sectorRepository = sectorRepository;
            _estadioRepository = estadioRepository;
        }

        public async Task<Sector> CrearSectorAsync(Sector sector)
        {
            await ValidarSectorAsync(sector);

            await _sectorRepository.AddSectorAsync(sector);
            return sector;
        }

        public async Task<Sector?> ObtenerPorIdAsync(int id)
        {
            return await _sectorRepository.ObtenerSectorPorIdAsync(id);
        }

        public async Task<IEnumerable<Sector>> ObtenerTodosAsync()
        {
            return await _sectorRepository.GetAll();
        }

        public async Task<bool> ActualizarSectorAsync(Sector sector)
        {
            await ValidarSectorAsync(sector);

            return await _sectorRepository.UpdateSectorAsync(sector);
        }

        public async Task<bool> EliminarSectorAsync(int id)
        {
            return await _sectorRepository.DeleteSectorAsync(id);
        }

        private async Task ValidarSectorAsync(Sector sector)
        {
            if (sector is null)
            {
                throw new ArgumentException("El sector no puede ser nulo.");
            }
            if (string.IsNullOrWhiteSpace(sector.Nombre))
            {
                throw new ArgumentException("El nombre del sector es obligatorio.");
            }
            if (sector.CapacidadMaxima <= 0)
            {
                throw new ArgumentException("La capacidad maxima debe ser mayor a cero.");
            }
            if (sector.PrecioBase <= 0)
            {
                throw new ArgumentException("El precio base debe ser mayor a cero.");
            }

            var estadio = await _estadioRepository.GetByIdAsync(sector.EstadioId);
            if (estadio is null)
            {
                throw new ArgumentException("El estadio seleccionado no existe.");
            }
        }
    }
}
