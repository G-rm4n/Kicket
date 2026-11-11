using System.Xml.Linq;
using ClosedXML.Excel;
using Core.Reportes;
using Core.Services;
using Data;
using Data.Implementaciones;
using Domain.Entities;
using Kicket.Contracts.Clubes;
using Kicket.Contracts.Reportes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Kicket.Tests
{
    // Reglas de negocio de Core: DNI y datos de usuario, validación de filtros y generación de reportes.
    public class ServiciosTests
    {
        private static TPIContext NuevoContexto()
        {
            var opciones = new DbContextOptionsBuilder<TPIContext>()
                .UseInMemoryDatabase($"kicket-{Guid.NewGuid()}").Options;
            var contexto = new TPIContext(opciones);
            contexto.Database.EnsureCreated(); // Crea la base en memoria y carga los datos semilla.
            return contexto;
        }

        private static Usuario UsuarioNuevo(string email, string? dni) => new()
        {
            Nombre = "Ana",
            Apellido = "Lopez",
            Email = email,
            Password = "secreto1",
            Dni = dni
        };

        [Fact]
        public async Task Registro_ConDniValido_LoGuardaNormalizado()
        {
            using var ctx = NuevoContexto();
            var servicio = new UsuarioService(new UsuarioRepository(ctx));
            var creado = await servicio.RegistrarUsuarioAsync(UsuarioNuevo("ana@mail.com", "32.555.666"));
            Assert.Equal("32555666", creado.Dni);
            Assert.NotEqual("secreto1", creado.Password); // se guarda el hash, no la contraseña
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("12AB")]
        [InlineData("123")]
        public async Task Registro_SinDniOConDniInvalido_SeRechaza(string? dni)
        {
            using var ctx = NuevoContexto();
            var servicio = new UsuarioService(new UsuarioRepository(ctx));
            await Assert.ThrowsAsync<ArgumentException>(() => servicio.RegistrarUsuarioAsync(UsuarioNuevo("x@mail.com", dni)));
        }

        [Fact]
        public async Task Registro_ConDniRepetido_SeRechaza()
        {
            using var ctx = NuevoContexto();
            var servicio = new UsuarioService(new UsuarioRepository(ctx));
            // 38456789 es el DNI de Lucía en los datos semilla.
            await Assert.ThrowsAsync<InvalidOperationException>(() => servicio.RegistrarUsuarioAsync(UsuarioNuevo("otra@mail.com", "38.456.789")));
        }

        [Fact]
        public async Task RegistroPublico_AuthService_ExigeDni()
        {
            using var ctx = NuevoContexto();
            var config = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Jwt:Key"] = "clave-de-prueba-de-al-menos-32-caracteres!!",
                ["Jwt:Issuer"] = "test",
                ["Jwt:Audience"] = "test"
            }).Build();
            var auth = new AuthService(config, new UsuarioRepository(ctx));

            await Assert.ThrowsAsync<ArgumentException>(() => auth.RegistrarUsuario(UsuarioNuevo("sin-dni@mail.com", null)));
            await auth.RegistrarUsuario(UsuarioNuevo("con-dni@mail.com", "41222333"));
            var login = await auth.Login("con-dni@mail.com", "secreto1");
            Assert.NotNull(login);
            Assert.Equal("Cliente", login!.Value.Item3.Rol);
        }

        [Fact]
        public async Task Edicion_SinDni_ConservaUsuarioViejo_YConDniAjenoFalla()
        {
            using var ctx = NuevoContexto();
            // Usuario "viejo" (anterior a la v2) sin DNI.
            ctx.Usuarios.Add(new Usuario { Nombre = "Viejo", Apellido = "Usuario", Email = "viejo@mail.com", Password = "$2a$11$abcdefghijklmnopqrstuuJw6cWQ4YyQd1D3e3aPj6Gg3xHXhQ1W2", Rol = "Cliente" });
            await ctx.SaveChangesAsync();
            var viejo = ctx.Usuarios.Single(u => u.Email == "viejo@mail.com");
            var servicio = new UsuarioService(new UsuarioRepository(ctx));

            var editado = new Usuario { IdUsuario = viejo.IdUsuario, Nombre = "Viejo", Apellido = "Editado", Email = viejo.Email, Password = viejo.Password, Rol = "Cliente", Dni = null };
            Assert.True(await servicio.ActualizarUsuarioAsync(editado));

            editado.Dni = "38456789"; // DNI de Lucía
            await Assert.ThrowsAsync<InvalidOperationException>(() => servicio.ActualizarUsuarioAsync(editado));
        }

        [Fact]
        public async Task Servicio_FiltroInvalido_TiraArgumentException()
        {
            using var ctx = NuevoContexto();
            var servicio = new ClubService(new ClubRepository(ctx));
            await Assert.ThrowsAsync<ArgumentException>(() => servicio.ObtenerPaginadoAsync(new ClubFiltro { OrdenarPor = "x; DROP TABLE" }));
            await Assert.ThrowsAsync<ArgumentException>(() => servicio.ObtenerPaginadoAsync(new ClubFiltro { TamanoPagina = 1000 }));
        }

        [Fact]
        public async Task Club_AnioDeFundacionFuturo_SeRechaza()
        {
            using var ctx = NuevoContexto();
            var servicio = new ClubService(new ClubRepository(ctx));
            var club = new Club { Nombre = "Nuevo", Abreviatura = "NUE", AnioFundacion = DateTime.Today.Year + 1 };
            await Assert.ThrowsAsync<ArgumentException>(() => servicio.CrearClubAsync(club));
        }

        // ---------- Reportes ----------

        private static ReporteVentasDto ReporteDeEjemplo()
        {
            var r = new ReporteVentasDto
            {
                GeneradoEn = new DateTime(2026, 11, 5, 10, 0, 0),
                Resumen = new ResumenVentasDto { ComprasConfirmadas = 3, EntradasVendidas = 7, TotalRecaudado = 99000m, TicketPromedio = 33000m, EventosConVentas = 2, EventosTotales = 4, CompradoresDistintos = 2 },
                PorEvento =
                {
                    new VentaEventoDto { EventoId = 1, Evento = "Central vs Boca <clásico> & \"más\"", Fecha = new DateTime(2027, 3, 20), Estadio = "Gigante", Compras = 2, Entradas = 5, Recaudado = 63000m },
                    new VentaEventoDto { EventoId = 2, Evento = "Newell's vs River", Fecha = new DateTime(2026, 4, 12), Estadio = "Coloso", Compras = 1, Entradas = 2, Recaudado = 36000m }
                },
                PorPeriodo =
                {
                    new VentaPeriodoDto { Dia = new DateTime(2026, 2, 3), Compras = 1, Entradas = 3, Recaudado = 54000m },
                    new VentaPeriodoDto { Dia = new DateTime(2026, 5, 20), Compras = 2, Entradas = 4, Recaudado = 45000m }
                },
                PorSector = { new VentaSectorDto { Estadio = "Gigante", Sector = "Platea Norte", CapacidadPorEvento = 4000, Entradas = 7, Recaudado = 99000m } },
                PorClub = { new VentaClubDto { Club = "Rosario Central", EventosComoLocal = 1, Entradas = 7, Recaudado = 99000m } }
            };
            r.Graficos.Add(new GraficoDto { Titulo = "Barras", Svg = GraficosSvg.BarrasHorizontales(r.PorEvento.Select(e => (e.Evento, (decimal)e.Entradas)).ToList(), false) });
            r.Graficos.Add(new GraficoDto { Titulo = "Linea", Svg = GraficosSvg.Linea(r.PorPeriodo.Select(p => (p.Dia, p.Recaudado)).ToList(), true) });
            r.Graficos.Add(new GraficoDto { Titulo = "Dona", Svg = GraficosSvg.Dona(r.PorSector.Select(s => (s.Sector, (decimal)s.Entradas)).ToList()) });
            return r;
        }

        [Fact]
        public void Graficos_SonSvgValidos_YEscapanElTexto()
        {
            var r = ReporteDeEjemplo();
            foreach (var g in r.Graficos)
            {
                var xml = XDocument.Parse(g.Svg); // falla si el SVG está mal formado (por ejemplo, texto sin escapar)
                Assert.Equal("svg", xml.Root!.Name.LocalName);
            }
            Assert.Contains("&lt;clásico&gt;", r.Graficos[0].Svg);
            Assert.DoesNotContain(",5\"", r.Graficos[1].Svg); // coordenadas con punto decimal, no coma
        }

        [Fact]
        public void Graficos_SinDatos_MuestranAviso()
        {
            Assert.Contains("Sin ventas confirmadas", GraficosSvg.BarrasHorizontales(new List<(string, decimal)>(), true));
            Assert.Contains("Sin ventas confirmadas", GraficosSvg.Dona(new List<(string, decimal)>()));
        }

        [Fact]
        public void ExportarPdf_GeneraUnPdfConContenido()
        {
            var pdf = ExportadorPdf.Generar(ReporteDeEjemplo());
            Assert.Equal("%PDF", System.Text.Encoding.ASCII.GetString(pdf, 0, 4));
            Assert.True(pdf.Length > 5000);
        }

        [Fact]
        public void ExportarExcel_TieneHojasDatosTipadosYTotales()
        {
            var bytes = ExportadorExcel.Generar(ReporteDeEjemplo());
            using var libro = new XLWorkbook(new MemoryStream(bytes));

            Assert.Equal(new[] { "Resumen", "Por partido", "Por club", "Por sector", "Por día" }, libro.Worksheets.Select(h => h.Name));
            var hoja = libro.Worksheet("Por partido");
            Assert.Equal("Partido", hoja.Cell(1, 1).GetString());
            Assert.True(hoja.Cell(2, 2).DataType == XLDataType.DateTime);    // fecha real, no texto
            Assert.Equal(63000m, hoja.Cell(2, 6).GetValue<decimal>());       // número real, no texto
            Assert.Equal("SUM(F2:F3)", hoja.Cell(4, 6).FormulaA1);           // fila de totales
            Assert.NotNull(hoja.AutoFilter);
        }

        [Fact]
        public async Task ReporteService_RangoInvertido_SeRechaza()
        {
            using var ctx = NuevoContexto();
            var servicio = new ReporteService(new ReporteVentasRepositorySinUso(), new EventoRepository(ctx));
            await Assert.ThrowsAsync<ArgumentException>(() =>
                servicio.ObtenerReporteVentasAsync(new ReporteFiltro { Desde = new DateTime(2026, 5, 1), Hasta = new DateTime(2026, 1, 1) }));
        }

        // La validación corta antes de llegar a la base: este repositorio no se usa.
        private sealed class ReporteVentasRepositorySinUso : Data.Interfaces.IReporteVentasRepository
        {
            public Task<ResumenVentasDto> ObtenerResumenAsync(ReporteFiltro f, CancellationToken ct = default) => throw new InvalidOperationException();
            public Task<List<VentaEventoDto>> ObtenerVentasPorEventoAsync(ReporteFiltro f, CancellationToken ct = default) => throw new InvalidOperationException();
            public Task<List<VentaClubDto>> ObtenerVentasPorClubAsync(ReporteFiltro f, CancellationToken ct = default) => throw new InvalidOperationException();
            public Task<List<VentaPeriodoDto>> ObtenerVentasPorPeriodoAsync(ReporteFiltro f, CancellationToken ct = default) => throw new InvalidOperationException();
            public Task<List<VentaSectorDto>> ObtenerVentasPorSectorAsync(ReporteFiltro f, CancellationToken ct = default) => throw new InvalidOperationException();
        }
    }
}
