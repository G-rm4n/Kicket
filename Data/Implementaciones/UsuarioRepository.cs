using Data.Consultas;
using Data.Interfaces;
using Kicket.Contracts.Common;
using Kicket.Contracts.Usuarios;
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
                usuarioFound.Dni = usuario.Dni;
                usuarioFound.Telefono = usuario.Telefono;
                usuarioFound.FechaNacimiento = usuario.FechaNacimiento;
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

        // Devuelve true si otro usuario ya usa ese email.
        public async Task<bool> ExistsEmailDeOtro(string email, int excluirId)
        {
            return await context.Usuarios.AnyAsync(u => u.Email == email && u.IdUsuario != excluirId);
        }

        // Devuelve true si ya hay un usuario con ese DNI (excluyendo, si se indica, al usuario que se edita).
        public async Task<bool> ExistsDni(string dni, int? excluirId = null)
        {
            return await context.Usuarios.AnyAsync(u => u.Dni == dni && (excluirId == null || u.IdUsuario != excluirId));
        }

        // Pagina de usuarios filtrada y ordenada en la base; nunca devuelve la contraseña.
        public async Task<PagedResult<UsuarioDto>> ObtenerPaginadoAsync(UsuarioFiltro filtro, CancellationToken ct = default)
        {
            IQueryable<Usuario> consulta = context.Usuarios.AsNoTracking();
            if (filtro.Busqueda is not null)
            {
                var texto = filtro.Busqueda;
                var digitos = Kicket.Contracts.Usuarios.Dni.Normalizar(texto) ?? texto;
                consulta = consulta.Where(u => u.Nombre.Contains(texto) || u.Apellido.Contains(texto)
                                               || u.Email.Contains(texto) || (u.Dni != null && u.Dni.Contains(digitos)));
            }
            if (!string.IsNullOrWhiteSpace(filtro.Rol))
            {
                var rol = filtro.Rol.Trim();
                consulta = consulta.Where(u => u.Rol == rol);
            }
            var dni = Kicket.Contracts.Usuarios.Dni.Normalizar(filtro.Dni);
            if (dni is not null)
            {
                consulta = consulta.Where(u => u.Dni != null && u.Dni.Contains(dni));
            }

            bool desc = filtro.EsDescendente;
            var ordenada = filtro.OrdenarPor switch
            {
                "id" => consulta.OrdenarPor(u => u.IdUsuario, desc),
                "nombre" => consulta.OrdenarPor(u => u.Nombre, desc),
                "email" => consulta.OrdenarPor(u => u.Email, desc),
                "dni" => consulta.OrdenarPor(u => u.Dni, desc),
                "rol" => consulta.OrdenarPor(u => u.Rol, desc),
                "registro" => consulta.OrdenarPor(u => u.FechaRegistro, desc),
                _ => consulta.OrdenarPor(u => u.Apellido, desc)
            };

            return await ordenada.LuegoPor(u => u.IdUsuario, desc)
                .Select(u => new UsuarioDto
                {
                    IdUsuario = u.IdUsuario, Nombre = u.Nombre, Apellido = u.Apellido, Email = u.Email,
                    Rol = u.Rol, Dni = u.Dni, Telefono = u.Telefono, FechaNacimiento = u.FechaNacimiento,
                    FechaRegistro = u.FechaRegistro
                })
                .PaginarAsync(filtro, ct);
        }
    }
}
