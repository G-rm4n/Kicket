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
    public class EntradaService : IEntradaService
    {
        private readonly IEntradaRepository _entradaRepository;

        public EntradaService(IEntradaRepository entradaRepository)
        {
            _entradaRepository = entradaRepository;
        }

        public async Task<Entrada?> ObtenerPorIdAsync(int id)
        {
            return await _entradaRepository.ObtenerPorIdAsync(id);
        }

        public async Task<IEnumerable<Entrada>> ObtenerPorEventoAsync(int eventoId)
        {
            return await _entradaRepository.ObtenerPorEventoAsync(eventoId);
        }
    }
}
