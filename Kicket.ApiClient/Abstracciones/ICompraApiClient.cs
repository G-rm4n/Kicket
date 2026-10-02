using Kicket.Contracts.Common;
using Kicket.Contracts.Compras;

namespace Kicket.ApiClient.Abstracciones
{
    /// <summary>
    /// Operaciones de Compra expuestas a la capa de escritorio. Sin UpdateAsync a proposito:
    /// la API no expone PUT /compras (ver la nota en CompraEndPoints).
    /// </summary>
    public interface ICompraApiClient
    {
        Task<IReadOnlyList<CompraDto>> GetAllAsync(CancellationToken ct = default);

        /// <summary>
        /// Historial completo, para el admin. El historial de compras es la lista que mas
        /// crece del sistema, asi que la grilla usa esto y no GetAllAsync.
        /// </summary>
        Task<PagedResult<CompraDto>> GetPaginadoAsync(PageRequest? pagina = null, CancellationToken ct = default);

        /// <summary>
        /// Las compras del usuario logueado. El id sale del token del lado de la API,
        /// no se manda: asi un cliente no puede pedir el historial de otro cambiando un numero.
        /// </summary>
        Task<PagedResult<CompraDto>> GetMisComprasPaginadoAsync(PageRequest? pagina = null, CancellationToken ct = default);

        /// <summary>
        /// Las compras de un cliente puntual. Reservado al admin: es la misma vista de arriba
        /// pero mirando la cuenta de otro, por eso el id si viaja en la ruta.
        /// </summary>
        Task<PagedResult<CompraDto>> GetPorUsuarioPaginadoAsync(int usuarioId, PageRequest? pagina = null, CancellationToken ct = default);

        Task<CompraDto> GetOneAsync(int id, CancellationToken ct = default);
        Task<CompraDto> CreateAsync(CompraRequest request, CancellationToken ct = default);
        Task DeleteAsync(int id, CancellationToken ct = default);
    }
}
