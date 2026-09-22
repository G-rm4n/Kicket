using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IEventoService
    {
        Task<Evento> CrearEventoAsync(Evento evento);
        Task<Evento?> ObtenerPorIdAsync(int id);
        Task<IEnumerable<Evento>> ObtenerTodosAsync();
        Task<bool> ActualizarEventoAsync(Evento evento);
        Task<bool> CancelarEventoAsync(int id);
        Task<bool> EliminarEventoAsync(int id);
    }
}
