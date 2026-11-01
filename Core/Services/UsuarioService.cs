using Core.Interfaces;
using Data.Implementaciones;
using Data.Interfaces;
using Domain.Entities;
using Kicket.Contracts.Usuarios;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Services
{
    // Lógica de negocio de usuarios: registro, login, edición y baja.
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;

        // Recibe el repositorio de usuarios por inyección de dependencias.
        public UsuarioService(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        // Registra un usuario: valida datos, controla email repetido, pone fecha y rol por defecto y hashea la contraseña.
        public async Task<Usuario> RegistrarUsuarioAsync(Usuario usuario)
        {
            ValidarUsuario(usuario);

            // Si el email ya existe tira InvalidOperationException (el handler global responde 409).
            bool emailYaRegistrado = await _usuarioRepository.ExistsEmail(usuario.Email);
            if (emailYaRegistrado)
            {
                throw new InvalidOperationException("Ya existe un usuario registrado con ese email.");
            }

            // Fecha de alta y rol "Usuario" si no vino ninguno.
            usuario.FechaRegistro = DateTime.Now;
            if (string.IsNullOrWhiteSpace(usuario.Rol))
            {
                usuario.Rol = Roles.Usuario;
            }

            // Guarda la contraseña hasheada, nunca en texto plano.
            usuario.Password = Contrasenas.Hashear(usuario.Password);
            await _usuarioRepository.AddAsync(usuario);
            return usuario;
        }

        // Login: email y contraseña obligatorios; si no coinciden tira UnauthorizedAccessException (401).
        public async Task<Usuario> LoginAsync(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("Email y contraseña son obligatorios.");
            }

            var usuario = await _usuarioRepository.GetByEmailAsync(email);

            if (usuario is null || !Contrasenas.Verificar(password, usuario.Password))
            {
                throw new UnauthorizedAccessException("Email o contraseña incorrectos.");
            }

            return usuario;
        }

        // Devuelve un usuario por id.
        public async Task<Usuario?> ObtenerPorIdAsync(int id)
        {
            return await _usuarioRepository.GetByIdAsync(id);
        }

        // Devuelve todos los usuarios.
        public async Task<IEnumerable<Usuario>> ObtenerTodosAsync()
        {
            return await _usuarioRepository.GetAllAsync();
        }

        // Valida y actualiza el usuario; solo hashea la contraseña si es nueva (no un hash ya existente).
        public async Task<bool> ActualizarUsuarioAsync(Usuario usuario)
        {
            ValidarUsuario(usuario);

            // El PUT reenvía el hash guardado cuando no se cambia la clave: no hay que volver a hashearlo.
            if (!Contrasenas.EsHash(usuario.Password))
            {
                usuario.Password = Contrasenas.Hashear(usuario.Password);
            }

            return await _usuarioRepository.UpdateAsync(usuario);
        }

        // Elimina un usuario (y en cascada sus compras y entradas).
        public async Task<bool> EliminarUsuarioAsync(int id)
        {
            return await _usuarioRepository.DeleteAsync(id);
        }

        // Reglas: no nulo, nombre, apellido y contraseña obligatorios, y email con "@".
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