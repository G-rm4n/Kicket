using Core.Interfaces;
using Data.Implementaciones;
using Data.Interfaces;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    // Servicio de entradas: solo consulta, delega en el repositorio.
    public class EntradaService : IEntradaService
    {
        private readonly IEntradaRepository _entradaRepository;

        // Recibe el repositorio de entradas por inyección de dependencias.
        public EntradaService(IEntradaRepository entradaRepository)
        {
            _entradaRepository = entradaRepository;
        }

        // Devuelve una entrada por id.
        public async Task<Entrada?> ObtenerPorIdAsync(int id)
        {
            return await _entradaRepository.ObtenerPorIdAsync(id);
        }

        // Devuelve las entradas de un evento.
        public async Task<IEnumerable<Entrada>> ObtenerPorEventoAsync(int eventoId)
        {
            return await _entradaRepository.ObtenerPorEventoAsync(eventoId);
        }
    }
}
