using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Implementaciones
{
    public class UsuarioRepository:IUsuarioRepository
    {
        private readonly TPIContext context;

        public UsuarioRepository(TPIContext context)
        {
            this.context=context;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var usuarioFound = await context.Usuarios.FirstOrDefaultAsync(u => u.IdUsuario == id);
            if(usuarioFound is not null)
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
                usuarioFound.Nombre=usuario.Nombre;
                usuarioFound.Apellido=usuario.Apellido;
                usuarioFound.Email=usuario.Email;
                usuarioFound.Password=usuario.Password;
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
            var usuarioFound = await context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);
            if (usuarioFound is null) return false;
            return true;
        }
    }
}
