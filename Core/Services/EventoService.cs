using Core.Interfaces;
using Data.Interfaces;
using Domain.Entities;
using Kicket.Contracts.Eventos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Services
{
    // Lógica de negocio de eventos: valida datos, calcula disponibilidad y maneja la cancelación.
    public class EventoService : IEventoService
    {
        private readonly IEventoRepository _eventoRepository;
        private readonly IEstadioRepository _estadioRepository;
        private readonly IClubRepository _clubRepository;
        private readonly ISectorRepository _sectorRepository;
        private readonly ICompraRepository _compraRepository;

        // Recibe los repositorios necesarios por inyección de dependencias.
        public EventoService(
            IEventoRepository eventoRepository,
            IEstadioRepository estadioRepository,
            IClubRepository clubRepository,
            ISectorRepository sectorRepository,
            ICompraRepository compraRepository)
        {
            _eventoRepository = eventoRepository;
            _estadioRepository = estadioRepository;
            _clubRepository = clubRepository;
            _sectorRepository = sectorRepository;
            _compraRepository = compraRepository;
        }

        // Valida el evento, lo marca como no cancelado y lo guarda.
        public async Task<Evento> CrearEventoAsync(Evento evento)
        {
            await ValidarEventoAsync(evento);

            evento.EstaCancelado = false;
            await _eventoRepository.AddAsync(evento);
            return evento;
        }

        // Devuelve un evento por id.
        public async Task<Evento?> ObtenerPorIdAsync(int id)
        {
            return await _eventoRepository.GetOneById(id);
        }

        // Devuelve todos los eventos.
        public async Task<IEnumerable<Evento>> ObtenerTodosAsync()
        {
            return await _eventoRepository.GetAllAsync();
        }

        // Calcula la disponibilidad de cada sector del estadio del evento.
        public async Task<IReadOnlyList<SectorDisponibilidadDto>?> ObtenerDisponibilidadAsync(int eventoId)
        {
            // Si el evento no existe devuelve null (el endpoint responde 404).
            var evento = await _eventoRepository.GetOneById(eventoId);
            if (evento is null)
            {
                return null;
            }

            // Trae los sectores del estadio donde se juega el evento.
            var sectores = await _sectorRepository.ObtenerPorEstadioAsync(evento.EstadioId);
            var resultado = new List<SectorDisponibilidadDto>();

            // Por cada sector: cuenta las vendidas y calcula el stock (nunca negativo).
            foreach (var sector in sectores)
            {
                int vendidas = await _compraRepository.ObtenerCantidadEntradasVendidasAsync(eventoId, sector.SectorId);
                int stockDisponible = Math.Max(0, sector.CapacidadMaxima - vendidas);

                resultado.Add(new SectorDisponibilidadDto
                {
                    SectorId = sector.SectorId,
                    EstadioId = sector.EstadioId,
                    Nombre = sector.Nombre,
                    PrecioBase = sector.PrecioBase,
                    CapacidadTotal = sector.CapacidadMaxima,
                    EntradasVendidas = vendidas,
                    StockDisponible = stockDisponible
                });
            }

            return resultado;
        }

        // Valida (sin exigir fecha futura) y actualiza el evento.
        public async Task<bool> ActualizarEventoAsync(Evento evento)
        {
            await ValidarEventoAsync(evento, esActualizacion: true);

            return await _eventoRepository.UpdateAsync(evento);
        }

        // Cancela el evento: no lo borra, solo pone EstaCancelado = true.
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

        // Elimina el evento (si tiene entradas vendidas, la BD lo impide).
        public async Task<bool> EliminarEventoAsync(int id)
        {
            return await _eventoRepository.DeleteAsync(id);
        }

        // Reglas: no nulo, nombre obligatorio, fecha futura (solo al crear), local distinto del visitante,
        // y estadio, club local y club visitante tienen que existir en la BD.
        private async Task ValidarEventoAsync(Evento evento, bool esActualizacion = false)
        {
            if (evento is null)
            {
                throw new ArgumentException("El evento no puede ser nulo.");
            }
            if (string.IsNullOrWhiteSpace(evento.Nombre))
            {
                throw new ArgumentException("El nombre del evento es obligatorio.");
            }
            if (!esActualizacion && evento.Fecha <= DateTime.Now)
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
