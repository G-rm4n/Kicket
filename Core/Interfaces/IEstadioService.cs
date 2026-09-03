using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;

namespace Core.Interfaces
{
    public interface IEstadioService
    {
        Task<Estadio> CrearEstadioAsync(Estadio estadio);
        Task<Estadio?> ObtenerPorIdAsync(int id);
        Task<IEnumerable<Estadio>> ObtenerTodosAsync();
        Task<bool> ActualizarEstadioAsync(Estadio estadio);
        Task<bool> EliminarEstadioAsync(int id);
    }
}