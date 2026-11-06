using System.Reflection;
using System.Windows.Forms;
using Kicket.ApiClient.Abstracciones;
using Kicket.ApiClient.Sesion;
using Kicket.WinForms.Controles;
using Kicket.WinForms.Forms;

namespace Kicket.Tests
{
    /// <summary>
    /// Comprueba que los formularios de escritorio se arman bien con los controles nuevos
    /// (menú, filtros y paginador) sin superponerse. No llama a la API: los clientes son simulados
    /// y nunca se usan, porque los datos se cargan en el evento Load y acá no se muestra la ventana.
    /// </summary>
    public class EscritorioTests
    {
        // Crea una implementación vacía de cualquier interfaz de cliente HTTP.
        private static T Simulado<T>() where T : class => DispatchProxy.Create<T, ProxyQueFalla>();

        public class ProxyQueFalla : DispatchProxy
        {
            protected override object? Invoke(MethodInfo? metodo, object?[]? args) =>
                throw new InvalidOperationException($"No se esperaba una llamada a {metodo?.Name} en el constructor.");
        }

        private static void EnHiloSta(Action accion)
        {
            Exception? error = null;
            var hilo = new Thread(() => { try { accion(); } catch (Exception ex) { error = ex; } });
            hilo.SetApartmentState(ApartmentState.STA);
            hilo.Start();
            hilo.Join();
            if (error is not null) throw new Exception(error.Message, error);
        }

        // Los controles agregados en la v2 (sin Name de diseñador, filtros y paginador) no pueden pisar a ningún otro.
        // Los pares de controles del diseñador original no se comparan: no se tocó ese diseño.
        private static bool EsNuevo(Control c) => string.IsNullOrEmpty(c.Name) || c is BarraFiltros || c is ControlPaginacion;

        private static void SinSuperposiciones(Form formulario)
        {
            var controles = formulario.Controls.Cast<Control>().Where(c => c is not MenuStrip).ToList();
            for (int i = 0; i < controles.Count; i++)
                for (int j = i + 1; j < controles.Count; j++)
                    if (EsNuevo(controles[i]) || EsNuevo(controles[j]))
                    Assert.False(controles[i].Bounds.IntersectsWith(controles[j].Bounds),
                        $"{formulario.Name}: '{controles[i].Name}{controles[i].Text}' se superpone con '{controles[j].Name}{controles[j].Text}'");
        }

        [Fact]
        public void FormPrincipal_UsaMenuDesplegable_ConCerrarSesion()
        {
            EnHiloSta(() =>
            {
                using var form = new FormPrincipal(new SesionUsuario());
                var menu = Assert.Single(form.Controls.OfType<MenuStrip>());
                var textos = menu.Items.OfType<ToolStripMenuItem>()
                    .SelectMany(i => i.DropDownItems.OfType<ToolStripMenuItem>())
                    .Select(i => i.Text).ToList();
                Assert.Contains("Clubes", textos);
                Assert.Contains("Estadios y sectores", textos);
                Assert.Contains("Eventos / Partidos", textos);
                Assert.Contains("Usuarios", textos);
                Assert.Contains("Compras y entradas", textos);
                Assert.Contains("Reportes de ventas", textos);
                Assert.Contains("Cerrar sesión", textos);
                Assert.Empty(form.Controls.OfType<Button>()); // ya no quedan los botones de navegación
            });
        }

        [Fact]
        public void Formularios_TienenFiltrosYPaginador_SinSuperponerse()
        {
            EnHiloSta(() =>
            {
                var formularios = new Form[]
                {
                    new FormClub(Simulado<IClubApiClient>()),
                    new FormEstadio(Simulado<IEstadioApiClient>()),
                    new FormEvento(Simulado<IEventoApiClient>(), Simulado<IClubApiClient>(), Simulado<IEstadioApiClient>()),
                    new FormUsuario(Simulado<IUsuarioApiClient>()),
                    new FormSectorDetalle(Simulado<ISectorApiClient>())
                };
                foreach (var form in formularios)
                {
                    using (form)
                    {
                        Assert.Single(form.Controls.OfType<ControlPaginacion>());
                        Assert.Single(form.Controls.OfType<BarraFiltros>());
                        var grilla = form.Controls.OfType<DataGridView>().Single();
                        var filtros = form.Controls.OfType<BarraFiltros>().Single();
                        var paginador = form.Controls.OfType<ControlPaginacion>().Single();
                        Assert.True(filtros.Bottom <= grilla.Top, $"{form.Name}: filtros arriba de la grilla");
                        Assert.True(paginador.Top >= grilla.Bottom, $"{form.Name}: paginador debajo de la grilla");
                        SinSuperposiciones(form);
                    }
                }
            });
        }

        [Fact]
        public void FormCompras_TienePaginador_SinSuperponerse()
        {
            EnHiloSta(() =>
            {
                using var form = new FormCompras(Simulado<ICompraApiClient>(), Simulado<IEventoApiClient>(), Simulado<IReporteApiClient>());
                Assert.Single(form.Controls.OfType<ControlPaginacion>());
                SinSuperposiciones(form);
            });
        }

        [Fact]
        public void FormRegistro_TieneCampoDni_SinSuperponerse()
        {
            EnHiloSta(() =>
            {
                using var form = new FormRegistro(Simulado<IAuthApiClient>());
                Assert.Contains(form.Controls.OfType<Label>(), l => l.Text == "DNI");
                SinSuperposiciones(form);
            });
        }
    }
}
