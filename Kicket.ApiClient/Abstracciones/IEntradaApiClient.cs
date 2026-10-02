using Kicket.Contracts.Entradas;

namespace Kicket.ApiClient.Abstracciones
{
    /// <summary>
    /// Operaciones de Entrada expuestas a la capa de escritorio. Solo lectura: las entradas
    /// se generan dentro de ICompraApiClient.CreateAsync, nunca sueltas.
    /// </summary>
    public interface IEntradaApiClient
    {
        Task<EntradaDto> GetOneAsync(int id, CancellationToken ct = default);
        Task<IReadOnlyList<EntradaDto>> GetPorEventoAsync(int eventoId, CancellationToken ct = default);
    }
}
