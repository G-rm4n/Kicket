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
    public class EventoService : IEventoService
    {
        private readonly IEventoRepository _eventoRepository;
        private readonly IEstadioRepository _estadioRepository;
        private readonly IClubRepository _clubRepository;

        public EventoService(IEventoRepository eventoRepository, IEstadioRepository estadioRepository, IClubRepository clubRepository)
        {
            _eventoRepository = eventoRepository;
            _estadioRepository = estadioRepository;
            _clubRepository = clubRepository;
        }

        public async Task<Evento> CrearEventoAsync(Evento evento)
        {
            await ValidarEventoAsync(evento);

            evento.EstaCancelado = false;
            await _eventoRepository.AddAsync(evento);
            return evento;
        }

        public async Task<Evento?> ObtenerPorIdAsync(int id)
        {
            return await _eventoRepository.GetOneById(id);
        }

        public async Task<IEnumerable<Evento>> ObtenerTodosAsync()
        {
            return await _eventoRepository.GetAllAsync();
        }

        public async Task<bool> ActualizarEventoAsync(Evento evento)
        {
            await ValidarEventoAsync(evento);

            return await _eventoRepository.UpdateAsync(evento);
        }

        public async Task<bool> CancelarEventoAsync(int id)
        {
            var evento = await _eventoRepository.GetOneById(id);
            if (evento is null)
            {
                return false;
            }

            evento.EstaCancelado = true;
            return await _eventoRepository.UpdateAsync(evento);
        }

        public async Task<bool> EliminarEventoAsync(int id)
        {
            return await _eventoRepository.DeleteAsync(id);
        }

        private async Task ValidarEventoAsync(Evento evento)
        {
            if (evento is null)
            {
                throw new ArgumentException("El evento no puede ser nulo.");
            }
            if (string.IsNullOrWhiteSpace(evento.Nombre))
            {
                throw new ArgumentException("El nombre del evento es obligatorio.");
            }
            if (evento.Fecha <= DateTime.Now)
            {
                throw new ArgumentException("La fecha del evento debe ser futura.");
            }
            if (evento.ClubLocalId == evento.ClubVisitanteId)
            {
                throw new ArgumentException("El club local y el visitante no pueden ser el mismo.");
            }

            var estadio = await _estadioRepository.GetByIdAsync(evento.EstadioId);
            if (estadio is null)
            {
                throw new ArgumentException("El estadio seleccionado no existe.");
            }

            var clubLocal = await _clubRepository.GetByIdAsync(evento.ClubLocalId);
            if (clubLocal is null)
            {
                throw new ArgumentException("El club local seleccionado no existe.");
            }

            var clubVisitante = await _clubRepository.GetByIdAsync(evento.ClubVisitanteId);
            if (clubVisitante is null)
            {
                throw new ArgumentException("El club visitante seleccionado no existe.");
            }
        }
    }
}
