using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;

namespace Core.Interfaces
{
    /// <summary>
    /// una Entrada siempre nace como parte de una Compra
    /// (ver ICompraService.GenerarCompraAsync), nunca se crea, modifica o borra suelta.
    /// </summary>
    public interface IEntradaService
    {
        Task<Entrada?> ObtenerPorIdAsync(int id);
        Task<IEnumerable<Entrada>> ObtenerPorEventoAsync(int eventoId);
    }
}