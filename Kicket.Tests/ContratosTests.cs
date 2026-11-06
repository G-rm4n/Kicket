using Kicket.Contracts.Clubes;
using Kicket.Contracts.Common;
using Kicket.Contracts.Compras;
using Kicket.Contracts.Eventos;
using Kicket.Contracts.Usuarios;

namespace Kicket.Tests
{
    // Reglas que viven en Contracts y se comparten entre la API y los clientes.
    public class ContratosTests
    {
        [Theory]
        [InlineData("30123456", true)]
        [InlineData("30.123.456", true)]
        [InlineData(" 5.123.456 ", true)]   // 7 dígitos
        [InlineData("01234567", true)]      // se conservan ceros a la izquierda
        [InlineData("123456", false)]       // muy corto
        [InlineData("123456789", false)]    // muy largo
        [InlineData("30A23456", false)]
        [InlineData("", false)]
        [InlineData(null, false)]
        public void Dni_EsValido(string? dni, bool esperado) => Assert.Equal(esperado, Dni.EsValido(dni));

        [Fact]
        public void Dni_Normalizar_SacaPuntosYEspacios_YConservaCeros()
        {
            Assert.Equal("30123456", Dni.Normalizar("30.123.456"));
            Assert.Equal("01234567", Dni.Normalizar("01.234.567"));
            Assert.Null(Dni.Normalizar("  "));
        }

        [Fact]
        public void PagedRequest_ValoresPorDefecto_SonValidos()
        {
            var filtro = new ClubFiltro { Pagina = null, TamanoPagina = null, Descendente = null };
            filtro.Normalizar();
            Assert.Empty(filtro.Validar());
            Assert.Equal(1, filtro.PaginaActual);
            Assert.Equal(10, filtro.Tamano);
            Assert.False(filtro.EsDescendente);
        }

        [Theory]
        [InlineData(0, 10, null)]
        [InlineData(1, 0, null)]
        [InlineData(1, 101, null)]
        [InlineData(1, 10, "DROP TABLE Clubs")]   // orden fuera de la lista blanca
        public void PagedRequest_ParametrosInvalidos_DevuelveErrores(int pagina, int tamano, string? orden)
        {
            var filtro = new ClubFiltro { Pagina = pagina, TamanoPagina = tamano, OrdenarPor = orden };
            filtro.Normalizar();
            Assert.NotEmpty(filtro.Validar());
        }

        [Fact]
        public void Filtros_RangoDeFechasInvertido_EsInvalido()
        {
            var filtro = new EventoFiltro { Desde = new DateTime(2027, 5, 1), Hasta = new DateTime(2027, 1, 1) };
            Assert.NotEmpty(filtro.Validar());
            var mis = new MisEntradasFiltro { Desde = new DateTime(2027, 5, 1), Hasta = new DateTime(2027, 1, 1) };
            Assert.NotEmpty(mis.Validar());
        }

        [Fact]
        public void Filtros_EstadoDesconocido_EsInvalido()
        {
            Assert.NotEmpty(new EventoFiltro { Estado = "borrados" }.Validar());
            Assert.Empty(new EventoFiltro { Estado = EventoFiltro.EstadoProximos }.Validar());
            Assert.NotEmpty(new UsuarioFiltro { Rol = "SuperAdmin" }.Validar());
        }

        [Theory]
        [InlineData(0, 10, 1)]
        [InlineData(10, 10, 1)]
        [InlineData(11, 10, 2)]
        [InlineData(51, 25, 3)]
        public void PagedResult_TotalPaginas(int total, int tamano, int esperado)
        {
            var resultado = new PagedResult<int> { TotalRegistros = total, TamanoPagina = tamano, Pagina = 1 };
            Assert.Equal(esperado, resultado.TotalPaginas);
        }

        [Fact]
        public void PartidoConEntradas_EstadoDerivadoDeLaFecha()
        {
            Assert.Equal("Finalizado", new PartidoConEntradasDto { Fecha = DateTime.Now.AddDays(-1) }.Estado);
            Assert.Equal("Próximo", new PartidoConEntradasDto { Fecha = DateTime.Now.AddDays(1) }.Estado);
            Assert.Equal("Cancelado", new PartidoConEntradasDto { Fecha = DateTime.Now.AddDays(1), EstaCancelado = true }.Estado);
        }

        [Fact]
        public void MiEntradaDto_NoTieneFilaNiAsiento()
        {
            // Pedido del profesor: "Mis Entradas" no muestra fila ni asiento; el DTO ni siquiera los transporta.
            Assert.Null(typeof(MiEntradaDto).GetProperty("FilaAsiento"));
        }
    }
}
