using System.Drawing;
using System.Windows.Forms;

namespace Kicket.WinForms.Controles
{
    /// <summary>
    /// Ayuda para sumar controles (filtros, paginador, campos nuevos) a formularios diseñados con
    /// posiciones fijas, sin redibujar todo el diseño: abre una franja horizontal corriendo hacia abajo
    /// lo que está debajo y agranda la ventana lo mismo.
    /// </summary>
    public static class LayoutListado
    {
        /// <summary>Abre una franja de "alto" píxeles a partir de la coordenada Y indicada y devuelve esa Y.</summary>
        public static int AbrirEspacio(Form formulario, int desdeY, int alto)
        {
            formulario.SuspendLayout();
            foreach (Control control in formulario.Controls)
            {
                if (control.Top >= desdeY)
                    control.Top += alto;
            }
            formulario.ClientSize = new Size(formulario.ClientSize.Width, formulario.ClientSize.Height + alto);
            formulario.ResumeLayout(false);
            return desdeY;
        }

        /// <summary>Pone la barra de filtros justo arriba de la grilla y el paginador justo debajo.</summary>
        public static void Envolver(Form formulario, Control grilla, Control? barraFiltros, Control? paginador)
        {
            if (barraFiltros is not null)
            {
                // Si un título de arriba (por ejemplo "Listado de...") ya invade unos píxeles de la grilla
                // por el re-escalado de fuentes, la barra arranca debajo de ese título.
                int libreDesde = formulario.Controls.Cast<Control>()
                    .Where(c => c != grilla && c.Top < grilla.Top && c.Bottom > grilla.Top
                                && c.Left < grilla.Right && c.Right > grilla.Left)
                    .Select(c => c.Bottom)
                    .DefaultIfEmpty(grilla.Top)
                    .Max();
                int y = grilla.Top;
                AbrirEspacio(formulario, y, (libreDesde - y) + barraFiltros.Height + 6);
                barraFiltros.Location = new Point(grilla.Left, libreDesde + 2);
                barraFiltros.Width = grilla.Width;
                formulario.Controls.Add(barraFiltros);
            }
            if (paginador is not null)
            {
                int y = AbrirEspacio(formulario, grilla.Bottom, paginador.Height + 6);
                paginador.Location = new Point(grilla.Left, y + 3);
                paginador.Width = grilla.Width;
                formulario.Controls.Add(paginador);
            }
        }

        /// <summary>
        /// Abre una fila nueva justo debajo de "referencia" (del alto de esa fila) y devuelve su Y.
        /// Se calcula sobre las posiciones reales: WinForms re-escala el diseño según la fuente y el DPI
        /// de cada PC, así que no sirven coordenadas fijas.
        /// </summary>
        public static int FilaDebajoDe(Form formulario, Control referencia, int separacion = 8)
        {
            int y = referencia.Bottom + separacion;
            AbrirEspacio(formulario, y, referencia.Height + separacion);
            return y;
        }

        /// <summary>Agrega una etiqueta y un control de edición en una posición, con el mismo estilo del formulario.</summary>
        public static T AgregarCampo<T>(Form formulario, string etiqueta, T control, int xEtiqueta, int xControl, int y, int ancho) where T : Control
        {
            var label = new Label { AutoSize = true, Text = etiqueta, Location = new Point(xEtiqueta, y + 3) };
            control.Location = new Point(xControl, y);
            control.Width = ancho;
            formulario.Controls.Add(label);
            formulario.Controls.Add(control);
            return control;
        }
    }
}
