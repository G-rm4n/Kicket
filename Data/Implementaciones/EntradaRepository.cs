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
    public class EntradaRepository:IEntradaRepository
    {
        private readonly TPIContext _context;

        public EntradaRepository(TPIContext context)
        {
            _context = context;
        }
        public async Task<Entrada?> ObtenerPorIdAsync(int idEntrada)
        {
            return await _context.Entradas.FirstOrDefaultAsync(e=>e.EntradaId == idEntrada);
        }

        public async Task<IEnumerable<Entrada>> ObtenerPorEventoAsync(int idEvento)
        {
            return await _context.Entradas.Where(e=>e.EventoId== idEvento).ToListAsync();
        }

        public async Task AddAsync(Entrada entrada)
        {
            _context.Add(entrada);
            await _context.SaveChangesAsync();
        }
    }
}
