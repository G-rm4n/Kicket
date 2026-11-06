using Data;
using Data.Implementaciones;
using Kicket.Contracts.Clubes;
using Kicket.Contracts.Compras;
using Kicket.Contracts.Eventos;
using Microsoft.EntityFrameworkCore;

namespace Kicket.Tests
{
    /// <summary>
    /// Repositorios EF Core contra una base en memoria cargada con los datos semilla de TPIContext.
    /// (Las consultas SQL reales y el repositorio ADO.NET se prueban contra SQL Server con las
    /// pruebas de integración de la API, ver MEJORAS_KICKET_V2.md.)
    /// </summary>
    public class RepositoriosTests
    {
        // Ids de los datos semilla.
        private const int Lucia = -2, Martin = -3;
        private const int ClasicoRosarino = -1, CentralBoca = -2, NewellsRiver = -3, CentralRiverCancelado = -4;

        private static TPIContext NuevoContexto()
        {
            var opciones = new DbContextOptionsBuilder<TPIContext>()
                .UseInMemoryDatabase($"kicket-{Guid.NewGuid()}")
                .Options;
            return new TPIContext(opciones); // EnsureCreated carga el seed
        }

        [Fact]
        public async Task Clubes_Paginado_SinRepetirNiSaltearEntrePaginas()
        {
            using var ctx = NuevoContexto();
            var repo = new ClubRepository(ctx);

            var pagina1 = await repo.ObtenerPaginadoAsync(new ClubFiltro { Pagina = 1, TamanoPagina = 3 });
            var pagina2 = await repo.ObtenerPaginadoAsync(new ClubFiltro { Pagina = 2, TamanoPagina = 3 });

            Assert.Equal(4, pagina1.TotalRegistros);
            Assert.Equal(2, pagina1.TotalPaginas);
            Assert.Equal(3, pagina1.Items.Count);
            Assert.Single(pagina2.Items);
            var ids = pagina1.Items.Concat(pagina2.Items).Select(c => c.ClubId).ToList();
            Assert.Equal(4, ids.Distinct().Count());
        }

        [Fact]
        public async Task Clubes_PaginaFueraDeRango_DevuelveLaUltima()
        {
            using var ctx = NuevoContexto();
            var resultado = await new ClubRepository(ctx).ObtenerPaginadoAsync(new ClubFiltro { Pagina = 50, TamanoPagina = 3 });
            Assert.Equal(2, resultado.Pagina);
            Assert.NotEmpty(resultado.Items);
        }

        [Fact]
        public async Task Eventos_FiltroEstado_DistingueProximosPasadosYCancelados()
        {
            using var ctx = NuevoContexto();
            var repo = new EventoRepository(ctx);

            var proximos = await repo.ObtenerPaginadoAsync(new EventoFiltro { Estado = EventoFiltro.EstadoProximos });
            var pasados = await repo.ObtenerPaginadoAsync(new EventoFiltro { Estado = EventoFiltro.EstadoPasados });
            var cancelados = await repo.ObtenerPaginadoAsync(new EventoFiltro { Estado = EventoFiltro.EstadoCancelados });

            Assert.Equal(new[] { CentralBoca, ClasicoRosarino }, proximos.Items.Select(e => e.IdEvento)); // orden por fecha
            Assert.Equal(new[] { NewellsRiver }, pasados.Items.Select(e => e.IdEvento));
            Assert.Equal(new[] { CentralRiverCancelado }, cancelados.Items.Select(e => e.IdEvento));
            Assert.Equal("Boca Juniors", proximos.Items[0].NombreEquipoVisitante);
        }

        [Fact]
        public async Task Eventos_FiltrosCombinados_ClubYRangoDeFechas()
        {
            using var ctx = NuevoContexto();
            var resultado = await new EventoRepository(ctx).ObtenerPaginadoAsync(new EventoFiltro
            {
                ClubId = -4, // River, como local o visitante
                Desde = new DateTime(2027, 1, 1),
                Hasta = new DateTime(2027, 12, 31)
            });
            Assert.Equal(new[] { CentralRiverCancelado }, resultado.Items.Select(e => e.IdEvento));
        }

        [Fact]
        public async Task MisEntradas_AgrupaComprasDelMismoPartido()
        {
            using var ctx = NuevoContexto();
            var resultado = await new CompraRepository(ctx).ObtenerMisEntradasAsync(Lucia, new MisEntradasFiltro());

            // Lucía tiene 3 compras en 2 partidos: se ven 2 grupos, cada partido una sola vez.
            Assert.Equal(2, resultado.TotalRegistros);
            Assert.Equal(resultado.Items.Count, resultado.Items.Select(p => p.EventoId).Distinct().Count());

            var centralBoca = resultado.Items.Single(p => p.EventoId == CentralBoca);
            Assert.Equal(2, centralBoca.CantidadCompras);
            Assert.Equal(4, centralBoca.CantidadEntradas);
            Assert.Equal(54000m + 9000m, centralBoca.TotalPagado);   // por compra, sin duplicar por entrada
            Assert.Equal(new DateTime(2026, 5, 20, 9, 30, 0), centralBoca.UltimaCompra);
            Assert.All(centralBoca.Entradas, e => Assert.False(string.IsNullOrEmpty(e.CodigoQr)));
        }

        [Fact]
        public async Task MisEntradas_OrdenPorDefecto_ProximoPrimero_PasadoAlFinal()
        {
            using var ctx = NuevoContexto();
            var resultado = await new CompraRepository(ctx).ObtenerMisEntradasAsync(Martin, new MisEntradasFiltro());

            // Martín: Central-Boca (2027-03), Clásico (2027-12) y Newell's-River (2026-04, ya jugado).
            Assert.Equal(new[] { CentralBoca, ClasicoRosarino, NewellsRiver }, resultado.Items.Select(p => p.EventoId));
        }

        [Fact]
        public async Task MisEntradas_OrdenPorCompraMasReciente()
        {
            using var ctx = NuevoContexto();
            var resultado = await new CompraRepository(ctx).ObtenerMisEntradasAsync(Martin,
                new MisEntradasFiltro { OrdenarPor = MisEntradasFiltro.OrdenCompraReciente });

            // Compras de Martín: Clásico 2026-06-05, Newell's-River 2026-03-28, Central-Boca 2026-02-14.
            Assert.Equal(new[] { ClasicoRosarino, NewellsRiver, CentralBoca }, resultado.Items.Select(p => p.EventoId));
        }

        [Fact]
        public async Task MisEntradas_PaginaPorPartido_NoPorEntrada()
        {
            using var ctx = NuevoContexto();
            var repo = new CompraRepository(ctx);
            var pagina1 = await repo.ObtenerMisEntradasAsync(Martin, new MisEntradasFiltro { TamanoPagina = 2 });
            var pagina2 = await repo.ObtenerMisEntradasAsync(Martin, new MisEntradasFiltro { TamanoPagina = 2, Pagina = 2 });

            Assert.Equal(3, pagina1.TotalRegistros);  // 3 partidos (aunque son 8 entradas)
            Assert.Equal(2, pagina1.Items.Count);
            Assert.Single(pagina2.Items);
            // El grupo de Central-Boca trae sus 4 entradas completas en la misma página.
            Assert.Equal(4, pagina1.Items.Single(p => p.EventoId == CentralBoca).Entradas.Count);
        }

        [Fact]
        public async Task MisEntradas_FiltroPasados_YRango()
        {
            using var ctx = NuevoContexto();
            var repo = new CompraRepository(ctx);
            var pasados = await repo.ObtenerMisEntradasAsync(Martin, new MisEntradasFiltro { Estado = MisEntradasFiltro.EstadoPasados });
            Assert.Equal(new[] { NewellsRiver }, pasados.Items.Select(p => p.EventoId));
            Assert.Equal("Finalizado", pasados.Items[0].Estado);

            var rango = await repo.ObtenerMisEntradasAsync(Martin, new MisEntradasFiltro { Desde = new DateTime(2027, 6, 1) });
            Assert.Equal(new[] { ClasicoRosarino }, rango.Items.Select(p => p.EventoId));
        }

        [Fact]
        public async Task MisEntradas_SoloDevuelveLasDelUsuario()
        {
            using var ctx = NuevoContexto();
            var resultado = await new CompraRepository(ctx).ObtenerMisEntradasAsync(Lucia, new MisEntradasFiltro());
            var comprasDeLucia = ctx.Compras.Where(c => c.UsuarioId == Lucia).Select(c => c.CompraId).ToHashSet();
            Assert.All(resultado.Items.SelectMany(p => p.Entradas), e => Assert.Contains(e.CompraId, comprasDeLucia));
        }

        [Fact]
        public async Task FilaYAsiento_SeConservanEnLaBase()
        {
            using var ctx = NuevoContexto();
            var compra = await new CompraRepository(ctx).GetByIdAsync(-2);
            Assert.NotNull(compra);
            Assert.All(compra!.Entradas, e => Assert.StartsWith("Fila 1, Asiento", e.FilaAsiento));
        }
    }
}
