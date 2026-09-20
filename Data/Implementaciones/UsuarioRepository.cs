using Data.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Data.Implementaciones
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly TPIContext context;

        public UsuarioRepository(TPIContext context)
        {
            this.context = context;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var usuarioFound = await context.Usuarios.FirstOrDefaultAsync(u => u.IdUsuario == id);
            if (usuarioFound is not null)
            {
                context.Remove(usuarioFound);
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> UpdateAsync(Usuario usuario)
        {
            var usuarioFound = await context.Usuarios.FirstOrDefaultAsync(u => u.IdUsuario == usuario.IdUsuario);
            if (usuarioFound is not null)
            {
                usuarioFound.Nombre = usuario.Nombre;
                usuarioFound.Apellido = usuario.Apellido;
                usuarioFound.Email = usuario.Email;
                usuarioFound.Password = usuario.Password;
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task AddAsync(Usuario usuario)
        {
            context.Add(usuario);
            await context.SaveChangesAsync();
        }

        public async Task<Usuario?> GetByIdAsync(int id)
        {
            return await context.Usuarios.FirstOrDefaultAsync(u => u.IdUsuario == id);
        }

        public async Task<IEnumerable<Usuario>> GetAllAsync()
        {
            return await context.Usuarios.ToListAsync();
        }

        public async Task<bool> ExistsEmail(string email)
        {
            return await context.Usuarios.AnyAsync(u => u.Email == email);
        }

        public async Task<Usuario?> GetByEmailAsync(string email)
        {
            return await context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<IEnumerable<Usuario>> AyncGetPaginated(int pagina, int cantidadPorPagina, bool esFiltrado, Expression<Func<Usuario, bool>>? filtro = null, bool esOrdenado = false, Func<IQueryable<Usuario>, IOrderedQueryable<Usuario>>? ordenamiento = null)
        {
            IQueryable<Usuario> query = context.Set<Usuario>();

            if (esFiltrado && filtro is not null)
            {
                query=query.Where(filtro).Where(u=>u.Rol=="Usuario");
            }

            if(esOrdenado && ordenamiento is not null)
            {
                query=ordenamiento(query);
            }

            query = query
                .Skip((pagina - 1) * cantidadPorPagina)
                .Take(cantidadPorPagina);

            return await query.ToListAsync();
        }
    }
}