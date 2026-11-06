using System.Drawing;
using System.Windows.Forms;

namespace Kicket.WinForms.Controles
{
    /// <summary>
    /// Franja de filtros para las grillas de escritorio. Cada formulario agrega sus campos con
    /// AgregarTexto / AgregarCombo / AgregarFecha y escucha Buscar y Limpiar.
    /// Enter en un campo de texto también busca.
    /// </summary>
    [System.ComponentModel.DesignerCategory("Code")]
    public class BarraFiltros : FlowLayoutPanel
    {
        private readonly Button _buscar = new() { Text = "Buscar", AutoSize = true };
        private readonly Button _limpiar = new() { Text = "Limpiar filtros", AutoSize = true };
        private readonly List<Action> _limpiadores = new();

        /// <summary>El usuario pidió aplicar los filtros.</summary>
        public event EventHandler? Buscar;
        /// <summary>El usuario borró los filtros (ya quedaron vacíos cuando se dispara).</summary>
        public event EventHandler? Limpiar;

        public BarraFiltros()
        {
            // Alto según la fuente del sistema (así escala con el DPI igual que el resto del formulario).
            Height = new TextBox().PreferredHeight + 12;
            WrapContents = false;
            AutoScroll = false;
            _buscar.Click += (_, _) => Buscar?.Invoke(this, EventArgs.Empty);
            _limpiar.Click += (_, _) =>
            {
                foreach (var limpiar in _limpiadores) limpiar();
                Limpiar?.Invoke(this, EventArgs.Empty);
            };
            Controls.Add(_buscar);
            Controls.Add(_limpiar);
        }

        public TextBox AgregarTexto(string etiqueta, int ancho, string? ayuda = null)
        {
            var texto = new TextBox { Width = ancho, PlaceholderText = ayuda ?? string.Empty };
            texto.KeyDown += (_, e) =>
            {
                if (e.KeyCode != Keys.Enter) return;
                e.SuppressKeyPress = true;
                Buscar?.Invoke(this, EventArgs.Empty);
            };
            Insertar(etiqueta, texto);
            _limpiadores.Add(texto.Clear);
            return texto;
        }

        public ComboBox AgregarCombo(string etiqueta, int ancho, params object[] opciones)
        {
            var combo = new ComboBox { Width = ancho, DropDownStyle = ComboBoxStyle.DropDownList };
            combo.Items.AddRange(opciones);
            if (combo.Items.Count > 0) combo.SelectedIndex = 0;
            Insertar(etiqueta, combo);
            _limpiadores.Add(() => { if (combo.Items.Count > 0) combo.SelectedIndex = 0; });
            return combo;
        }

        /// <summary>Fecha opcional: el checkbox del control indica si el filtro está activo.</summary>
        public DateTimePicker AgregarFecha(string etiqueta)
        {
            var fecha = new DateTimePicker { Width = 130, Format = DateTimePickerFormat.Short, ShowCheckBox = true, Checked = false };
            Insertar(etiqueta, fecha);
            _limpiadores.Add(() => fecha.Checked = false);
            return fecha;
        }

        /// <summary>Habilita o deshabilita los botones mientras se espera la respuesta.</summary>
        public void Bloquear(bool bloquear) => _buscar.Enabled = _limpiar.Enabled = !bloquear;

        // Inserta etiqueta + control antes de los botones.
        private void Insertar(string etiqueta, Control control)
        {
            var label = new Label { Text = etiqueta, AutoSize = true, Padding = new Padding(0, 6, 0, 0) };
            control.Margin = new Padding(3, 3, 10, 3);
            Controls.Add(label);
            Controls.Add(control);
            Controls.SetChildIndex(label, Controls.Count - 4);
            Controls.SetChildIndex(control, Controls.Count - 3);
        }

        /// <summary>Valor de una fecha opcional (null si el checkbox no está tildado).</summary>
        public static DateTime? Valor(DateTimePicker fecha) => fecha.Checked ? fecha.Value.Date : null;
    }
}
