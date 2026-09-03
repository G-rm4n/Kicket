using Core.Interfaces;
using Data.Implementaciones;
using Data.Interfaces;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Services
{
    public class EstadioService : IEstadioService
    {
        private readonly IEstadioRepository _estadioRepository;

        public EstadioService(IEstadioRepository estadioRepository)
        {
            _estadioRepository = estadioRepository;
        }

        public async Task<Estadio> CrearEstadioAsync(Estadio estadio)
        {
            ValidarEstadio(estadio);

            await _estadioRepository.AddAsync(estadio);
            return estadio;
        }

        public async Task<Estadio?> ObtenerPorIdAsync(int id)
        {
            return await _estadioRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Estadio>> ObtenerTodosAsync()
        {
            return await _estadioRepository.GetAllAsync();
        }

        public async Task<bool> ActualizarEstadioAsync(Estadio estadio)
        {
            ValidarEstadio(estadio);

            return await _estadioRepository.UpdateAsync(estadio);
        }

        public async Task<bool> EliminarEstadioAsync(int id)
        {
            return await _estadioRepository.DeleteAsync(id);
        }

        private static void ValidarEstadio(Estadio estadio)
        {
            if (estadio is null)
            {
                throw new ArgumentException("El estadio no puede ser nulo.");
            }
            if (string.IsNullOrWhiteSpace(estadio.Nombre))
            {
                throw new ArgumentException("El nombre del estadio es obligatorio.");
            }
            if (string.IsNullOrWhiteSpace(estadio.Direccion))
            {
                throw new ArgumentException("La dirección del estadio es obligatoria.");
            }
            if (string.IsNullOrWhiteSpace(estadio.Ciudad))
            {
                throw new ArgumentException("La ciudad del estadio es obligatoria.");
            }
        }
    }
}