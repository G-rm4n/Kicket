using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;

namespace Core.Interfaces
{
    // Contrato de la lógica de negocio de clubes.
    public interface IClubService
    {
        // Valida y crea un club.
        Task<Club> CrearClubAsync(Club club);
        // Devuelve un club por id (null si no existe).
        Task<Club?> ObtenerPorIdAsync(int id);
        // Devuelve todos los clubes.
        Task<IEnumerable<Club>> ObtenerTodosAsync();
        // Valida y actualiza un club; false si no existe.
        Task<bool> ActualizarClubAsync(Club club);
        // Elimina un club; false si no existe.
        Task<bool> EliminarClubAsync(int id);
    }
}