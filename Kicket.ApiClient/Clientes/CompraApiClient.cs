using Kicket.Contracts.Common;
using Kicket.ApiClient.Abstracciones;
using Kicket.ApiClient.Http;
using Kicket.Contracts.Compras;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Kicket.ApiClient.Clientes
{
    // Cliente HTTP tipado para los endpoints de compras.
    public class CompraApiClient : ApiClientBase, ICompraApiClient
    {
        // Prefijo de las rutas de compras en la API.
        private const string Ruta = "compras";

        // Recibe el HttpClient configurado.
        public CompraApiClient(HttpClient http) : base(http) { }

        // Lista todas las compras.
        public async Task<IReadOnlyList<CompraDto>> GetAllAsync(CancellationToken ct = default) =>
            await GetAsync<List<CompraDto>>(Ruta, ct);

        // Lista las compras de un usuario.
        public async Task<IReadOnlyList<CompraDto>> GetByUsuarioIdAsync(int usuarioId, CancellationToken ct = default) =>
            await GetAsync<List<CompraDto>>($"{Ruta}/usuario/{usuarioId}", ct);

        // Pide una pagina filtrada (los filtros viajan en la query string).
        public Task<PagedResult<CompraDto>> GetPaginadoAsync(CompraFiltro filtro, CancellationToken ct = default) =>
            GetAsync<PagedResult<CompraDto>>(ConQuery($"{Ruta}/paginado", filtro), ct);
        // Pide los partidos del usuario logueado con sus entradas.
        public Task<PagedResult<PartidoConEntradasDto>> GetMisEntradasAsync(MisEntradasFiltro filtro, CancellationToken ct = default) =>
            GetAsync<PagedResult<PartidoConEntradasDto>>(ConQuery($"{Ruta}/mis-entradas", filtro), ct);
        // Trae una compra por id.
        public Task<CompraDto> GetOneAsync(int id, CancellationToken ct = default) =>
            GetAsync<CompraDto>($"{Ruta}/{id}", ct);

        // Crea una compra.
        public Task<CompraDto> CreateAsync(CompraRequest request, CancellationToken ct = default) =>
            PostAsync<CompraDto>(Ruta, request, ct);

        // Borra una compra por id.
        public Task DeleteAsync(int id, CancellationToken ct = default) =>
            DeleteAsync($"{Ruta}/{id}", ct);
    }
}
