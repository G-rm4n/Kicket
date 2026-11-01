using Data.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Data.Implementaciones
{
    // Implementación del repositorio de usuarios usando EF Core.
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly TPIContext context;

        // Recibe el DbContext por inyección de dependencias.
        public UsuarioRepository(TPIContext context)
        {
            this.context = context;
        }

        // Busca el usuario; si existe lo borra (y en cascada sus compras) y devuelve true; si no, false.
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

        // Busca el usuario existente y copia nombre, apellido, email y contraseña (ya hasheada).
        // El rol solo se cambia si viene informado.
        public async Task<bool> UpdateAsync(Usuario usuario)
        {
            var usuarioFound = await context.Usuarios.FirstOrDefaultAsync(u => u.IdUsuario == usuario.IdUsuario);
            if (usuarioFound is not null)
            {
                usuarioFound.Nombre = usuario.Nombre;
                usuarioFound.Apellido = usuario.Apellido;
                usuarioFound.Email = usuario.Email;
                usuarioFound.Password = usuario.Password;
                if (!string.IsNullOrWhiteSpace(usuario.Rol))
                {
                    usuarioFound.Rol = usuario.Rol;
                }
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        // Agrega un usuario y guarda.
        public async Task AddAsync(Usuario usuario)
        {
            context.Add(usuario);
            await context.SaveChangesAsync();
        }

        // Busca un usuario por id.
        public async Task<Usuario?> GetByIdAsync(int id)
        {
            return await context.Usuarios.FirstOrDefaultAsync(u => u.IdUsuario == id);
        }

        // Trae todos los usuarios.
        public async Task<IEnumerable<Usuario>> GetAllAsync()
        {
            return await context.Usuarios.ToListAsync();
        }

        // Devuelve true si ya existe un usuario con ese email.
        public async Task<bool> ExistsEmail(string email)
        {
            return await context.Usuarios.AnyAsync(u => u.Email == email);
        }

        // Busca un usuario por email (null si no existe).
        public async Task<Usuario?> GetByEmailAsync(string email)
        {
            return await context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);
        }

        // Paginado de usuarios: si hay filtro, además limita a los de rol "Usuario";
        // después ordena si se pidió y aplica Skip/Take.
        public async Task<IEnumerable<Usuario>> AyncGetPaginated(int pagina, int cantidadPorPagina, bool esFiltrado, Expression<Func<Usuario, bool>>? filtro = null, bool esOrdenado = false, Func<IQueryable<Usuario>, IOrderedQueryable<Usuario>>? ordenamiento = null)
        {
            IQueryable<Usuario> query = context.Set<Usuario>();

            // Al filtrar, se excluyen los administradores.
            if (esFiltrado && filtro is not null)
            {
                query = query.Where(filtro).Where(u => u.Rol == "Usuario");
            }

            // Si se pidió ordenar, aplica el ordenamiento.
            if (esOrdenado && ordenamiento is not null)
            {
                query = ordenamiento(query);
            }

            // Saltea las páginas anteriores y toma la cantidad de la página actual.
            query = query
                .Skip((pagina - 1) * cantidadPorPagina)
                .Take(cantidadPorPagina);

            return await query.ToListAsync();
        }
    }
}
