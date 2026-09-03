using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Data.Interfaces
{
    internal interface IUsuarioRepository
    {
        Task AddAsync(Usuario usuario);
        Task<bool> DeleteAsync(int id);
        Task<bool> UpdateAsync(Usuario usuario);
        Task<Usuario?> GetByIdAsync(int id);
        Task<IEnumerable<Usuario>> GetAllAsync();
        Task<bool> ExistsEmail(string email);
    }
}
