using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    // Contrato del repositorio de entradas.
    public interface IEntradaRepository
    {
        // Busca una entrada por id.
        Task<Entrada?> ObtenerPorIdAsync(int id);
        // Devuelve todas las entradas de un evento.
        Task<IEnumerable<Entrada>> ObtenerPorEventoAsync(int eventoId);
        // Guarda una entrada nueva.
        Task AddAsync(Entrada entrada);
    }
}
