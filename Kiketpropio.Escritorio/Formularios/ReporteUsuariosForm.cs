using Kiketpropio.Dominio.DTOs;
using Kiketpropio.Escritorio.Servicios;

namespace Kiketpropio.Escritorio.Formularios
{
    public partial class ReporteUsuariosForm : Form
    {
        private static readonly Color[] _colores =
        {
            Color.FromArgb(31, 77, 120),
            Color.FromArgb(31, 157, 85),
            Color.FromArgb(192, 57, 43),
            Color.FromArgb(107, 120, 132),
            Color.FromArgb(214, 158, 46)
        };

        private List<UsuariosPorRol> _reporte = new();

        public ReporteUsuariosForm()
        {
            InitializeComponent();
        }

        private async void ReporteUsuariosForm_Load(object? sender, EventArgs e)
        {
            await CargarReporte();
        }

        private async Task CargarReporte()
        {
            try
            {
                _reporte = await ApiClient.GetAll<UsuariosPorRol>("Reporte/usuarios-por-rol");
                dgvReporte.DataSource = _reporte;
                ConfigurarColumnas();

                lblTotales.Text = $"Usuarios registrados: {_reporte.Sum(r => r.Cantidad)}    Perfiles en uso: {_reporte.Count}";
                pnlGrafico.Invalidate();
            }
            catch (HttpRequestException)
            {
                MessageBox.Show("No se pudo conectar con la API de Kicket", "Kicket", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void ConfigurarColumnas()
        {
            if (dgvReporte.Columns.Count == 0) return;

            dgvReporte.Columns["Rol"]!.HeaderText = "Perfil";
            dgvReporte.Columns["Cantidad"]!.HeaderText = "Cantidad de usuarios";
            dgvReporte.Columns["EdadPromedio"]!.HeaderText = "Edad promedio";
        }

        private void pnlGrafico_Paint(object? sender, PaintEventArgs e)
        {
            var lienzo = e.Graphics;
            lienzo.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            using var fuenteTitulo = new Font("Segoe UI", 11F, FontStyle.Bold);
            using var fuenteEtiqueta = new Font("Segoe UI", 9F);
            using var pincelTexto = new SolidBrush(Color.FromArgb(34, 39, 43));
            using var lapizEje = new Pen(Color.FromArgb(216, 222, 228));

            lienzo.DrawString("Usuarios por perfil", fuenteTitulo, pincelTexto, 16, 12);

            if (_reporte.Count == 0)
            {
                lienzo.DrawString("Sin datos para graficar", fuenteEtiqueta, pincelTexto, 16, 48);
                return;
            }

            var margenIzquierdo = 60;
            var margenSuperior = 50;
            var margenInferior = 40;
            var alturaGrafico = pnlGrafico.Height - margenSuperior - margenInferior;
            var anchoGrafico = pnlGrafico.Width - margenIzquierdo - 30;
            var baseY = margenSuperior + alturaGrafico;
            var maximo = _reporte.Max(r => r.Cantidad);
            if (maximo == 0) maximo = 1;

            lienzo.DrawLine(lapizEje, margenIzquierdo, margenSuperior, margenIzquierdo, baseY);
            lienzo.DrawLine(lapizEje, margenIzquierdo, baseY, margenIzquierdo + anchoGrafico, baseY);

            for (int marca = 0; marca <= 4; marca++)
            {
                var valor = (int)Math.Ceiling(maximo * marca / 4.0);
                var y = baseY - (int)(alturaGrafico * (marca / 4.0));
                lienzo.DrawLine(lapizEje, margenIzquierdo, y, margenIzquierdo + anchoGrafico, y);
                lienzo.DrawString(valor.ToString(), fuenteEtiqueta, pincelTexto, 20, y - 8);
            }

            var anchoColumna = anchoGrafico / _reporte.Count;
            var anchoBarra = Math.Min(90, anchoColumna - 30);

            for (int indice = 0; indice < _reporte.Count; indice++)
            {
                var fila = _reporte[indice];
                var altura = (int)(alturaGrafico * (fila.Cantidad / (double)maximo));
                var x = margenIzquierdo + (anchoColumna * indice) + ((anchoColumna - anchoBarra) / 2);
                var y = baseY - altura;

                using var pincelBarra = new SolidBrush(_colores[indice % _colores.Length]);
                lienzo.FillRectangle(pincelBarra, x, y, anchoBarra, altura);

                var textoCantidad = fila.Cantidad.ToString();
                var medidaCantidad = lienzo.MeasureString(textoCantidad, fuenteEtiqueta);
                lienzo.DrawString(textoCantidad, fuenteEtiqueta, pincelTexto, x + ((anchoBarra - medidaCantidad.Width) / 2), y - 18);

                var medidaRol = lienzo.MeasureString(fila.Rol, fuenteEtiqueta);
                lienzo.DrawString(fila.Rol, fuenteEtiqueta, pincelTexto, x + ((anchoBarra - medidaRol.Width) / 2), baseY + 8);
            }
        }

        private async void btnActualizar_Click(object? sender, EventArgs e)
        {
            await CargarReporte();
        }
    }
}
