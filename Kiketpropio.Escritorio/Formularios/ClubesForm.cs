using Kiketpropio.Dominio.Entidades;
using Kiketpropio.Escritorio.Servicios;

namespace Kiketpropio.Escritorio.Formularios
{
    public partial class ClubesForm : Form
    {
        public ClubesForm()
        {
            InitializeComponent();
        }

        private async void ClubesForm_Load(object? sender, EventArgs e)
        {
            btnNuevo.Enabled = Sesion.PuedeEditar;
            btnEditar.Enabled = Sesion.PuedeEditar;
            btnEliminar.Enabled = Sesion.EsAdmin;

            await CargarClubes();
        }

        private async Task CargarClubes()
        {
            try
            {
                var clubes = await ApiClient.GetAll<Club>("Club");
                dgvClubes.DataSource = clubes;
                ConfigurarColumnas();
            }
            catch (HttpRequestException)
            {
                MessageBox.Show("No se pudo conectar con la API de Kicket", "Kicket", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void ConfigurarColumnas()
        {
            if (dgvClubes.Columns.Count == 0) return;

            dgvClubes.Columns["IdClub"]!.HeaderText = "ID";
            dgvClubes.Columns["IdClub"]!.FillWeight = 20;
            dgvClubes.Columns["Nombre"]!.HeaderText = "Nombre";
            dgvClubes.Columns["Nombre"]!.FillWeight = 40;
            dgvClubes.Columns["Logo_Url"]!.HeaderText = "URL del logo";
            dgvClubes.Columns["Logo_Url"]!.FillWeight = 60;
        }

        private Club? GetClubSeleccionado()
        {
            if (dgvClubes.CurrentRow is null) return null;

            return dgvClubes.CurrentRow.DataBoundItem as Club;
        }

        private async void btnNuevo_Click(object? sender, EventArgs e)
        {
            using var edicionForm = new ClubEdicionForm(null);
            if (edicionForm.ShowDialog() == DialogResult.OK) await CargarClubes();
        }

        private async void btnEditar_Click(object? sender, EventArgs e)
        {
            var clubSeleccionado = GetClubSeleccionado();
            if (clubSeleccionado is null)
            {
                MessageBox.Show("Seleccione un club de la grilla", "Kicket", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using var edicionForm = new ClubEdicionForm(clubSeleccionado);
            if (edicionForm.ShowDialog() == DialogResult.OK) await CargarClubes();
        }

        private async void btnEliminar_Click(object? sender, EventArgs e)
        {
            var clubSeleccionado = GetClubSeleccionado();
            if (clubSeleccionado is null)
            {
                MessageBox.Show("Seleccione un club de la grilla", "Kicket", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var confirmacion = MessageBox.Show($"¿Confirma la baja del club {clubSeleccionado.Nombre}?", "Kicket", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirmacion != DialogResult.Yes) return;

            var resultado = await ApiClient.Delete("Club", clubSeleccionado.IdClub);
            if (!resultado)
            {
                MessageBox.Show("No se pudo eliminar el club", "Kicket", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            await CargarClubes();
        }

        private async void btnActualizar_Click(object? sender, EventArgs e)
        {
            await CargarClubes();
        }
    }
}
