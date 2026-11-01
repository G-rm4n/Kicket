using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;

namespace Core.Interfaces
{
    // Contrato de la lógica de negocio de usuarios.
    public interface IUsuarioService
    {
        // Valida, hashea la contraseña y registra un usuario.
        Task<Usuario> RegistrarUsuarioAsync(Usuario usuario);
        // Verifica email y contraseña; tira excepción si no coinciden.
        Task<Usuario> LoginAsync(string email, string password);
        // Devuelve un usuario por id.
        Task<Usuario?> ObtenerPorIdAsync(int id);
        // Devuelve todos los usuarios.
        Task<IEnumerable<Usuario>> ObtenerTodosAsync();
        // Valida y actualiza un usuario; false si no existe.
        Task<bool> ActualizarUsuarioAsync(Usuario usuario);
        // Elimina un usuario; false si no existe.
        Task<bool> EliminarUsuarioAsync(int id);
    }
}