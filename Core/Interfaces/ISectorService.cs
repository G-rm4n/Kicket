using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface ISectorService
    {
        Task<Sector> CrearSectorAsync(Sector sector);
        Task<Sector?> ObtenerPorIdAsync(int id);
        Task<IEnumerable<Sector>> ObtenerTodosAsync();
        Task<bool> ActualizarSectorAsync(Sector sector);
        Task<bool> EliminarSectorAsync(int id);
    }
}
