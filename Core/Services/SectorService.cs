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
    // Lógica de negocio de sectores: valida los datos y delega en el repositorio.
    public class SectorService : ISectorService
    {
        private readonly ISectorRepository _sectorRepository;
        private readonly IEstadioRepository _estadioRepository;

        // Recibe los repositorios de sectores y estadios por inyección de dependencias.
        public SectorService(ISectorRepository sectorRepository, IEstadioRepository estadioRepository)
        {
            _sectorRepository = sectorRepository;
            _estadioRepository = estadioRepository;
        }

        // Valida el sector y lo guarda.
        public async Task<Sector> CrearSectorAsync(Sector sector)
        {
            await ValidarSectorAsync(sector);

            await _sectorRepository.AddSectorAsync(sector);
            return sector;
        }

        // Devuelve un sector por id.
        public async Task<Sector?> ObtenerPorIdAsync(int id)
        {
            return await _sectorRepository.ObtenerSectorPorIdAsync(id);
        }

        // Devuelve todos los sectores.
        public async Task<IEnumerable<Sector>> ObtenerTodosAsync()
        {
            return await _sectorRepository.GetAll();
        }
        // Devuelve los sectores de un estadio.
        public async Task<IEnumerable<Sector>> ObtenerPorEstadioAsync(int estadioId)
        {
            return await _sectorRepository.ObtenerPorEstadioAsync(estadioId);
        }
        // Valida el sector y lo actualiza.
        public async Task<bool> ActualizarSectorAsync(Sector sector)
        {
            await ValidarSectorAsync(sector);

            return await _sectorRepository.UpdateSectorAsync(sector);
        }

        // Elimina el sector (si tiene entradas vendidas, la BD lo impide).
        public async Task<bool> EliminarSectorAsync(int id)
        {
            return await _sectorRepository.DeleteSectorAsync(id);
        }

        // Reglas: no nulo, nombre obligatorio, capacidad y precio mayores a cero, y el estadio tiene que existir.
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

            // Verifica en la BD que el estadio del sector exista.
            var estadio = await _estadioRepository.GetByIdAsync(sector.EstadioId);
            if (estadio is null)
            {
                throw new ArgumentException("El estadio seleccionado no existe.");
            }
        }
    }
}
