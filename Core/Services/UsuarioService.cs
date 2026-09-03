using Core.Interfaces;
using Data.Implementaciones;
using Data.Interfaces;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioService(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public async Task<Usuario> RegistrarUsuarioAsync(Usuario usuario)
        {
            ValidarUsuario(usuario);

            bool emailYaRegistrado = await _usuarioRepository.ExistsEmail(usuario.Email);
            if (emailYaRegistrado)
            {
                throw new InvalidOperationException("Ya existe un usuario registrado con ese email.");
            }

            usuario.FechaRegistro = DateTime.Now;
            if (string.IsNullOrWhiteSpace(usuario.Rol))
            {
                usuario.Rol = "Cliente";
            }

            // TODO: la entidad Usuario deja explícito que la password debe
            // guardarse hasheada (ver comentario en Domain.Entities.Usuario).
            // Falta integrar un hasher (ej. BCrypt) antes de guardar.
            await _usuarioRepository.AddAsync(usuario);
            return usuario;
        }

        public async Task<Usuario> LoginAsync(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("Email y contraseña son obligatorios.");
            }

            var usuario = await _usuarioRepository.GetByEmailAsync(email);

            // TODO: comparar contra el hash una vez que el registro guarde
            // la password hasheada, en vez de comparar texto plano.
            if (usuario is null || usuario.Password != password)
            {
                throw new UnauthorizedAccessException("Email o contraseña incorrectos.");
            }

            return usuario;
        }

        public async Task<Usuario?> ObtenerPorIdAsync(int id)
        {
            return await _usuarioRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Usuario>> ObtenerTodosAsync()
        {
            return await _usuarioRepository.GetAllAsync();
        }

        public async Task<bool> ActualizarUsuarioAsync(Usuario usuario)
        {
            ValidarUsuario(usuario);

            return await _usuarioRepository.UpdateAsync(usuario);
        }

        public async Task<bool> EliminarUsuarioAsync(int id)
        {
            return await _usuarioRepository.DeleteAsync(id);
        }

        private static void ValidarUsuario(Usuario usuario)
        {
            if (usuario is null)
            {
                throw new ArgumentException("El usuario no puede ser nulo.");
            }
            if (string.IsNullOrWhiteSpace(usuario.Nombre))
            {
                throw new ArgumentException("El nombre es obligatorio.");
            }
            if (string.IsNullOrWhiteSpace(usuario.Apellido))
            {
                throw new ArgumentException("El apellido es obligatorio.");
            }
            if (string.IsNullOrWhiteSpace(usuario.Email) || !usuario.Email.Contains('@'))
            {
                throw new ArgumentException("El email no es válido.");
            }
            if (string.IsNullOrWhiteSpace(usuario.Password))
            {
                throw new ArgumentException("La contraseña es obligatoria.");
            }
        }
    }
}