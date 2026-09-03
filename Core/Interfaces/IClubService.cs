using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;

namespace Core.Interfaces
{
    public interface IClubService
    {
        Task<Club> CrearClubAsync(Club club);
        Task<Club?> ObtenerPorIdAsync(int id);
        Task<IEnumerable<Club>> ObtenerTodosAsync();
        Task<bool> ActualizarClubAsync(Club club);
        Task<bool> EliminarClubAsync(int id);
    }
}