using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    // Contrato del repositorio de eventos.
    public interface IEventoRepository
    {
        // Busca un evento por id.
        Task<Evento?> GetOneById(int eventoId);
        // Agrega un evento nuevo.
        Task AddAsync(Evento evento);
        // Borra un evento por id; false si no existe.
        Task<bool> DeleteAsync(int id);
        // Devuelve todos los eventos.
        Task<IEnumerable<Evento>> GetAllAsync();
        // Actualiza un evento; false si no existe.
        Task<bool> UpdateAsync(Evento evento);

        // Devuelve una página de eventos, con filtro y orden opcionales.
        Task<IEnumerable<Evento>> AyncGetPaginated(int pagina,int cantidadPorPagina,bool esFiltrado,Expression<Func<Evento, bool>>? filtro = null,bool esOrdenado = false,Func<IQueryable<Evento>, IOrderedQueryable<Evento>>? ordenamiento = null);
    }
}
