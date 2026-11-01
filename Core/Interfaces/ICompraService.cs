using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;

namespace Core.Interfaces
{
    // Contrato de la lógica de negocio de compras.
    public interface ICompraService
    {
        // Valida stock y datos, arma la compra con sus entradas (asientos y QR) y la guarda.
        Task<Compra> GenerarCompraAsync(
            int usuarioId,
            int eventoId,
            int sectorId,
            int cantidad,
            string? metodoPago = "MercadoPago",
            string? paymentId = null,
            string? estadoPago = "Aprobado");

        // Devuelve una compra por id.
        Task<Compra?> ObtenerPorIdAsync(int id);
        // Devuelve todas las compras.
        Task<IEnumerable<Compra>> ObtenerTodosAsync();
        // Elimina una compra; false si no existe.
        Task<bool> EliminarCompraAsync(int id);
    }
}
