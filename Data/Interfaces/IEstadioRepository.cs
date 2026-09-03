using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Data.Interfaces
{
    public interface IEstadioRepository
    {
        Task AddAsync(Estadio estadio);
        Task<bool> DeleteAsync(int id);
        Task<bool> UpdateAsync(Estadio estadio);
        Task<Estadio?> GetByIdAsync(int id);
        Task<IEnumerable<Estadio>> GetAllAsync();
    }
}
