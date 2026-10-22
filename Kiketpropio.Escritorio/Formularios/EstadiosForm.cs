using Kiketpropio.Dominio.Entidades;
using Kiketpropio.Escritorio.Servicios;

namespace Kiketpropio.Escritorio.Formularios
{
    public partial class EstadiosForm : Form
    {
        public EstadiosForm()
        {
            InitializeComponent();
        }

        private async void EstadiosForm_Load(object? sender, EventArgs e)
        {
            btnNuevo.Enabled = Sesion.PuedeEditar;
            btnEditar.Enabled = Sesion.PuedeEditar;
            btnEliminar.Enabled = Sesion.EsAdmin;

            await CargarEstadios();
        }

        private async Task CargarEstadios()
        {
            try
            {
                var estadios = await ApiClient.GetAll<Estadio>("Estadio");
                dgvEstadios.DataSource = estadios;
                ConfigurarColumnas();
            }
            catch (HttpRequestException)
            {
                MessageBox.Show("No se pudo conectar con la API de Kicket", "Kicket", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void ConfigurarColumnas()
        {
            if (dgvEstadios.Columns.Count == 0) return;

            dgvEstadios.Columns["IdEstadio"]!.HeaderText = "ID";
            dgvEstadios.Columns["IdEstadio"]!.FillWeight = 15;
            dgvEstadios.Columns["Nombre"]!.HeaderText = "Nombre";
            dgvEstadios.Columns["Nombre"]!.FillWeight = 35;
            dgvEstadios.Columns["Calle"]!.HeaderText = "Calle";
            dgvEstadios.Columns["Calle"]!.FillWeight = 30;
            dgvEstadios.Columns["Nro"]!.HeaderText = "Numero";
            dgvEstadios.Columns["Nro"]!.FillWeight = 15;
            dgvEstadios.Columns["Ciudad"]!.HeaderText = "Ciudad";
            dgvEstadios.Columns["Ciudad"]!.FillWeight = 25;
        }

        private Estadio? GetEstadioSeleccionado()
        {
            if (dgvEstadios.CurrentRow is null) return null;

            return dgvEstadios.CurrentRow.DataBoundItem as Estadio;
        }

        private async void btnNuevo_Click(object? sender, EventArgs e)
        {
            using var edicionForm = new EstadioEdicionForm(null);
            if (edicionForm.ShowDialog() == DialogResult.OK) await CargarEstadios();
        }

        private async void btnEditar_Click(object? sender, EventArgs e)
        {
            var estadioSeleccionado = GetEstadioSeleccionado();
            if (estadioSeleccionado is null)
            {
                MessageBox.Show("Seleccione un estadio de la grilla", "Kicket", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using var edicionForm = new EstadioEdicionForm(estadioSeleccionado);
            if (edicionForm.ShowDialog() == DialogResult.OK) await CargarEstadios();
        }

        private async void btnEliminar_Click(object? sender, EventArgs e)
        {
            var estadioSeleccionado = GetEstadioSeleccionado();
            if (estadioSeleccionado is null)
            {
                MessageBox.Show("Seleccione un estadio de la grilla", "Kicket", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var confirmacion = MessageBox.Show($"¿Confirma la baja del estadio {estadioSeleccionado.Nombre}?", "Kicket", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirmacion != DialogResult.Yes) return;

            var resultado = await ApiClient.Delete("Estadio", estadioSeleccionado.IdEstadio);
            if (!resultado)
            {
                MessageBox.Show("No se pudo eliminar el estadio", "Kicket", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            await CargarEstadios();
        }

        private async void btnActualizar_Click(object? sender, EventArgs e)
        {
            await CargarEstadios();
        }
    }
}
