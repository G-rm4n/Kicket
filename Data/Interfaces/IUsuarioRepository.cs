using Domain.Entities;
using Kicket.Contracts.Common;
using Kicket.Contracts.Usuarios;
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
        // Indica si otro usuario (distinto de excluirId) ya usa ese email.
        Task<bool> ExistsEmailDeOtro(string email, int excluirId);
        // Indica si ya hay un usuario con ese DNI (ya normalizado); excluirId sirve para la edicion.
        Task<bool> ExistsDni(string dni, int? excluirId = null);

        // Pagina de usuarios (sin contraseña) filtrada y ordenada en la base.
        Task<PagedResult<UsuarioDto>> ObtenerPaginadoAsync(UsuarioFiltro filtro, CancellationToken ct = default);
    }
}
