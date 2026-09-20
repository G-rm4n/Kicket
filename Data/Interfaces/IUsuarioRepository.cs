using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IUsuarioRepository
    {
        Task AddAsync(Usuario usuario);
        Task<bool> DeleteAsync(int id);
        Task<bool> UpdateAsync(Usuario usuario);
        Task<Usuario?> GetByIdAsync(int id);
        Task<IEnumerable<Usuario>> GetAllAsync();
        Task<bool> ExistsEmail(string email);
        Task<Usuario?> GetByEmailAsync(string email);

        Task<IEnumerable<Usuario>> AyncGetPaginated(int pagina, int cantidadPorPagina, bool esFiltrado, Expression<Func<Usuario, bool>>? filtro = null, bool esOrdenado = false, Func<IQueryable<Usuario>, IOrderedQueryable<Usuario>>? ordenamiento = null);
    }
}
