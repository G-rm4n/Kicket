using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IEventoRepository
    {
        Task<Evento?> GetOneById(int eventoId);
        Task AddAsync(Evento evento);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<Evento>> GetAllAsync();
        Task<bool> UpdateAsync(Evento evento);

        Task<IEnumerable<Evento>> AyncGetPaginated(int pagina,int cantidadPorPagina,bool esFiltrado,Expression<Func<Evento, bool>>? filtro = null,bool esOrdenado = false,Func<IQueryable<Evento>, IOrderedQueryable<Evento>>? ordenamiento = null);
    }
}
