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
    // Implementación del repositorio de entradas usando EF Core.
    public class EntradaRepository:IEntradaRepository
    {
        private readonly TPIContext _context;

        // Recibe el DbContext por inyección de dependencias.
        public EntradaRepository(TPIContext context)
        {
            _context = context;
        }
        // Busca una entrada por id.
        public async Task<Entrada?> ObtenerPorIdAsync(int idEntrada)
        {
            return await _context.Entradas.FirstOrDefaultAsync(e=>e.EntradaId == idEntrada);
        }

        // Devuelve todas las entradas de un evento.
        public async Task<IEnumerable<Entrada>> ObtenerPorEventoAsync(int idEvento)
        {
            return await _context.Entradas.Where(e=>e.EventoId== idEvento).ToListAsync();
        }

        // Agrega una entrada y guarda los cambios.
        public async Task AddAsync(Entrada entrada)
        {
            _context.Add(entrada);
            await _context.SaveChangesAsync();
        }
    }
}
