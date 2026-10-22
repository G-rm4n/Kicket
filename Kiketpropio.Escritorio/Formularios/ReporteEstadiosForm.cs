using Kiketpropio.Dominio.DTOs;
using Kiketpropio.Escritorio.Servicios;

namespace Kiketpropio.Escritorio.Formularios
{
    public partial class ReporteEstadiosForm : Form
    {
        private List<EstadiosPorCiudad> _reporte = new();

        public ReporteEstadiosForm()
        {
            InitializeComponent();
        }

        private async void ReporteEstadiosForm_Load(object? sender, EventArgs e)
        {
            await CargarReporte();
        }

        private async Task CargarReporte()
        {
            try
            {
                _reporte = await ApiClient.GetAll<EstadiosPorCiudad>("Reporte/estadios-por-ciudad");
                dgvReporte.DataSource = _reporte;
                ConfigurarColumnas();

                var ciudadPrincipal = _reporte.Count > 0 ? _reporte.First().Ciudad : "-";
                lblTotales.Text = $"Estadios habilitados: {_reporte.Sum(r => r.Cantidad)}    Ciudades alcanzadas: {_reporte.Count}    Ciudad con mas estadios: {ciudadPrincipal}";
            }
            catch (HttpRequestException)
            {
                MessageBox.Show("No se pudo conectar con la API de Kicket", "Kicket", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void ConfigurarColumnas()
        {
            if (dgvReporte.Columns.Count == 0) return;

            dgvReporte.Columns["Ciudad"]!.HeaderText = "Ciudad";
            dgvReporte.Columns["Ciudad"]!.FillWeight = 25;
            dgvReporte.Columns["Cantidad"]!.HeaderText = "Cantidad de estadios";
            dgvReporte.Columns["Cantidad"]!.FillWeight = 25;
            dgvReporte.Columns["Estadios"]!.HeaderText = "Estadios";
            dgvReporte.Columns["Estadios"]!.FillWeight = 70;
        }

        private async void btnActualizar_Click(object? sender, EventArgs e)
        {
            await CargarReporte();
        }
    }
}
