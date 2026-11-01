using Kicket.Contracts.Entradas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kicket.ApiClient.Abstracciones
{
    // Interfaz "contenedora": la que se usa de verdad es la IEntradaApiClient anidada de abajo.
    public interface IEntradaApiClient
    {
        /// <summary>
        /// Operaciones de Entrada expuestas a la capa de escritorio. Solo lectura: las entradas
        /// se generan dentro de ICompraApiClient.CreateAsync, nunca sueltas.
        /// </summary>
        public interface IEntradaApiClient
        {
            // GET /entradas/{id}: trae una entrada.
            Task<EntradaDto> GetOneAsync(int id, CancellationToken ct = default);
            // GET /entradas/evento/{id}: entradas de un evento.
            Task<IReadOnlyList<EntradaDto>> GetPorEventoAsync(int eventoId, CancellationToken ct = default);
        }
    }
}
