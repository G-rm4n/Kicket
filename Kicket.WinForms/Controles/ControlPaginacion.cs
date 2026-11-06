using System.Drawing;
using System.Windows.Forms;
using Kicket.Contracts.Common;

namespace Kicket.WinForms.Controles
{
    /// <summary>
    /// Paginador para las grillas de escritorio: "Registros X–Y de N", tamaño de página (10/25/50) y
    /// botones primera/anterior/siguiente/última. No pagina por sí mismo: avisa con PaginaCambiada y
    /// el formulario vuelve a pedirle la página a la API con Pagina y TamanoPagina.
    /// </summary>
    [System.ComponentModel.DesignerCategory("Code")]
    public class ControlPaginacion : UserControl
    {
        private readonly Label _resumen = new() { AutoSize = true, Anchor = AnchorStyles.Left, Padding = new Padding(0, 7, 0, 0) };
        private readonly ComboBox _tamano = new() { DropDownStyle = ComboBoxStyle.DropDownList, Width = 60 };
        private readonly Button _primera = Boton("«", "Primera página");
        private readonly Button _anterior = Boton("‹", "Página anterior");
        private readonly Label _pagina = new() { AutoSize = true, Padding = new Padding(6, 7, 6, 0) };
        private readonly Button _siguiente = Boton("›", "Página siguiente");
        private readonly Button _ultima = Boton("»", "Última página");
        private int _totalPaginas = 1;
        private bool _actualizando;

        /// <summary>Página pedida (empieza en 1).</summary>
        public int Pagina { get; set; } = 1;
        /// <summary>Registros por página elegidos en el combo.</summary>
        public int TamanoPagina { get; private set; } = 10;
        /// <summary>Se dispara cuando el usuario cambia de página o de tamaño.</summary>
        public event EventHandler? PaginaCambiada;

        public ControlPaginacion()
        {
            // Alto según la fuente del sistema (escala con el DPI).
            Height = new ComboBox().PreferredHeight + 12;
            foreach (var t in PagedRequest.TamanosSugeridos) _tamano.Items.Add(t);
            _tamano.SelectedItem = TamanoPagina;

            var derecha = new FlowLayoutPanel
            {
                Dock = DockStyle.Right,
                AutoSize = true,
                WrapContents = false,
                FlowDirection = FlowDirection.LeftToRight
            };
            derecha.Controls.AddRange(new Control[]
            {
                new Label { AutoSize = true, Text = "Por página", Padding = new Padding(0, 7, 0, 0) },
                _tamano, _primera, _anterior, _pagina, _siguiente, _ultima
            });
            var izquierda = new FlowLayoutPanel { Dock = DockStyle.Fill };
            izquierda.Controls.Add(_resumen);
            Controls.Add(izquierda);
            Controls.Add(derecha);

            _primera.Click += (_, _) => IrA(1);
            _anterior.Click += (_, _) => IrA(Pagina - 1);
            _siguiente.Click += (_, _) => IrA(Pagina + 1);
            _ultima.Click += (_, _) => IrA(_totalPaginas);
            _tamano.SelectedIndexChanged += (_, _) =>
            {
                if (_actualizando || _tamano.SelectedItem is not int nuevo) return;
                TamanoPagina = nuevo;
                Pagina = 1;
                PaginaCambiada?.Invoke(this, EventArgs.Empty);
            };
            Actualizar(1, 1, 0);
        }

        /// <summary>Refresca textos y botones con la página que devolvió la API.</summary>
        public void Actualizar<T>(PagedResult<T> resultado) =>
            Actualizar(resultado.Pagina, resultado.TotalPaginas, resultado.TotalRegistros);

        /// <summary>Deshabilita todo mientras se espera la respuesta (evita dobles clics).</summary>
        public void Bloquear(bool bloquear)
        {
            _tamano.Enabled = !bloquear;
            ActualizarBotones(bloquear);
        }

        /// <summary>Vuelve a la página 1 (se usa al cambiar los filtros).</summary>
        public void Reiniciar() => Pagina = 1;

        private void Actualizar(int pagina, int totalPaginas, int totalRegistros)
        {
            _actualizando = true;
            Pagina = pagina;
            _totalPaginas = Math.Max(1, totalPaginas);
            int desde = totalRegistros == 0 ? 0 : (pagina - 1) * TamanoPagina + 1;
            int hasta = Math.Min(pagina * TamanoPagina, totalRegistros);
            _resumen.Text = totalRegistros == 0 ? "Sin registros" : $"Registros {desde}–{hasta} de {totalRegistros}";
            _pagina.Text = $"Página {pagina} de {_totalPaginas}";
            ActualizarBotones(false);
            _actualizando = false;
        }

        private void ActualizarBotones(bool bloqueado)
        {
            _primera.Enabled = _anterior.Enabled = !bloqueado && Pagina > 1;
            _siguiente.Enabled = _ultima.Enabled = !bloqueado && Pagina < _totalPaginas;
        }

        private void IrA(int pagina)
        {
            if (pagina < 1 || pagina > _totalPaginas || pagina == Pagina) return;
            Pagina = pagina;
            PaginaCambiada?.Invoke(this, EventArgs.Empty);
        }

        private static Button Boton(string texto, string ayuda)
        {
            var boton = new Button { Text = texto, AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink, MinimumSize = new Size(32, 0), AccessibleName = ayuda };
            new ToolTip().SetToolTip(boton, ayuda);
            return boton;
        }
    }
}
