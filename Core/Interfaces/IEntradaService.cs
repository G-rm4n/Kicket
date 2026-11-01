using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;

namespace Core.Interfaces
{
    /// <summary>
    /// una Entrada siempre nace como parte de una Compra
    /// (ver ICompraService.GenerarCompraAsync), nunca se crea, modifica o borra suelta.
    /// </summary>
    // Contrato del servicio de entradas: solo lectura.
    public interface IEntradaService
    {
        // Devuelve una entrada por id.
        Task<Entrada?> ObtenerPorIdAsync(int id);
        // Devuelve las entradas vendidas para un evento.
        Task<IEnumerable<Entrada>> ObtenerPorEventoAsync(int eventoId);
    }
}