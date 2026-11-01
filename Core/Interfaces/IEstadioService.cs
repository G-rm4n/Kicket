using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;

namespace Core.Interfaces
{
    // Contrato de la lógica de negocio de estadios.
    public interface IEstadioService
    {
        // Valida y crea un estadio.
        Task<Estadio> CrearEstadioAsync(Estadio estadio);
        // Devuelve un estadio por id.
        Task<Estadio?> ObtenerPorIdAsync(int id);
        // Devuelve todos los estadios.
        Task<IEnumerable<Estadio>> ObtenerTodosAsync();
        // Valida y actualiza un estadio; false si no existe.
        Task<bool> ActualizarEstadioAsync(Estadio estadio);
        // Elimina un estadio; false si no existe.
        Task<bool> EliminarEstadioAsync(int id);
    }
}