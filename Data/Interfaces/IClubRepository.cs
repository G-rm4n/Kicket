using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Data.Interfaces
{
    public interface IClubRepository
    {
        Task AddAsync(Club club);
        Task<bool> DeleteAsync(int id);
        Task<bool> UpdateAsync(Club club);
        Task<Club?> GetByIdAsync(int id);
        Task<IEnumerable<Club>> GetAllAsync();
    }
}
