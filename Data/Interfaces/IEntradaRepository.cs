using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IEntradaRepository
    {
        Task<Entrada?> ObtenerPorIdAsync(int id);
        Task<IEnumerable<Entrada>> ObtenerPorEventoAsync(int eventoId);
        Task AddAsync(Entrada entrada);
    }
}
