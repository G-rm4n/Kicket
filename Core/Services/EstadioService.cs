using Core.Interfaces;
using Data.Implementaciones;
using Data.Interfaces;
using Domain.Entities;
using Kicket.Contracts.Common;
using Kicket.Contracts.Estadios;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Services
{
    // Lógica de negocio de estadios: valida los datos y delega en el repositorio.
    public class EstadioService : IEstadioService
    {
        private readonly IEstadioRepository _estadioRepository;

        // Recibe el repositorio de estadios por inyección de dependencias.
        public EstadioService(IEstadioRepository estadioRepository)
        {
            _estadioRepository = estadioRepository;
        }

        // Valida el estadio y lo guarda.
        public async Task<Estadio> CrearEstadioAsync(Estadio estadio)
        {
            ValidarEstadio(estadio);

            await _estadioRepository.AddAsync(estadio);
            return estadio;
        }

        // Devuelve un estadio por id.
        public async Task<Estadio?> ObtenerPorIdAsync(int id)
        {
            return await _estadioRepository.GetByIdAsync(id);
        }

        // Devuelve todos los estadios.
        public async Task<IEnumerable<Estadio>> ObtenerTodosAsync()
        {
            return await _estadioRepository.GetAllAsync();
        }

        // Valida los filtros y pide la pagina al repositorio.
        public async Task<PagedResult<EstadioDto>> ObtenerPaginadoAsync(EstadioFiltro filtro, CancellationToken ct = default)
        {
            Validaciones.ValidarFiltro(filtro);
            return await _estadioRepository.ObtenerPaginadoAsync(filtro, ct);
        }

        // Valida el estadio y lo actualiza.
        public async Task<bool> ActualizarEstadioAsync(Estadio estadio)
        {
            ValidarEstadio(estadio);

            return await _estadioRepository.UpdateAsync(estadio);
        }

        // Elimina el estadio. Si tiene sectores, la BD lo impide (FK Restrict).
        public async Task<bool> EliminarEstadioAsync(int id)
        {
            return await _estadioRepository.DeleteAsync(id);
        }

        // Reglas: no nulo, y nombre, dirección y ciudad obligatorios (si no, ArgumentException → 400).
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
            estadio.Provincia = Validaciones.TextoOpcional(estadio.Provincia);
            if (estadio.Provincia is { Length: > 100 })
            {
                throw new ArgumentException("La provincia no puede superar los 100 caracteres.");
            }
        }
    }
}