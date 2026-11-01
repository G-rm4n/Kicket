using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    // Contrato del repositorio de usuarios.
    public interface IUsuarioRepository
    {
        // Agrega un usuario nuevo.
        Task AddAsync(Usuario usuario);
        // Borra un usuario por id; false si no existe.
        Task<bool> DeleteAsync(int id);
        // Actualiza un usuario; false si no existe.
        Task<bool> UpdateAsync(Usuario usuario);
        // Busca un usuario por id.
        Task<Usuario?> GetByIdAsync(int id);
        // Devuelve todos los usuarios.
        Task<IEnumerable<Usuario>> GetAllAsync();
        // Indica si ya hay un usuario con ese email.
        Task<bool> ExistsEmail(string email);
        // Busca un usuario por email (se usa en el login).
        Task<Usuario?> GetByEmailAsync(string email);

        // Devuelve una página de usuarios, con filtro y orden opcionales.
        Task<IEnumerable<Usuario>> AyncGetPaginated(int pagina, int cantidadPorPagina, bool esFiltrado, Expression<Func<Usuario, bool>>? filtro = null, bool esOrdenado = false, Func<IQueryable<Usuario>, IOrderedQueryable<Usuario>>? ordenamiento = null);
    }
}
