using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface ISectorRepository
    {
        Task<Sector> ObtenerSectorPorIdAsync(int sectorId);
    }
}
