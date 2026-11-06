using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;
using Kicket.Contracts.Common;
using Kicket.Contracts.Compras;

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
        // Compras de un usuario.
        Task<IEnumerable<Compra>> ObtenerPorUsuarioAsync(int usuarioId);
        // Pagina de compras para administracion.
        Task<PagedResult<CompraDto>> ObtenerPaginadoAsync(CompraFiltro filtro, CancellationToken ct = default);
        // "Mis Entradas" del usuario, agrupadas por partido.
        Task<PagedResult<PartidoConEntradasDto>> ObtenerMisEntradasAsync(int usuarioId, MisEntradasFiltro filtro, CancellationToken ct = default);
        // Elimina una compra; false si no existe.
        Task<bool> EliminarCompraAsync(int id);
    }
}
