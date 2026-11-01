using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Data.Interfaces
{
    // Contrato del repositorio de clubes: operaciones CRUD contra la tabla Clubs.
    public interface IClubRepository
    {
        // Agrega un club nuevo.
        Task AddAsync(Club club);
        // Borra un club por id; devuelve false si no existe.
        Task<bool> DeleteAsync(int id);
        // Actualiza un club existente; devuelve false si no existe.
        Task<bool> UpdateAsync(Club club);
        // Busca un club por id (null si no existe).
        Task<Club?> GetByIdAsync(int id);
        // Devuelve todos los clubes.
        Task<IEnumerable<Club>> GetAllAsync();
    }
}