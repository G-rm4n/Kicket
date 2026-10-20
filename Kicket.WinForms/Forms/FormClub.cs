using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using Kicket.ApiClient.Abstracciones;
using Kicket.Contracts.Clubes;

namespace Kicket.WinForms.Forms
{
    public partial class FormClub : Form
    {
        private readonly IClubApiClient _clubApiClient;
        private int? _clubIdSeleccionado = null;

        public FormClub(IClubApiClient clubApiClient)
        {
            InitializeComponent();
            _clubApiClient = clubApiClient;
        }

        private async void FormClub_Load(object sender, EventArgs e)
        {
            dataGridClubes.AutoGenerateColumns = false;
            await CargarClubes();
        }

        private async Task CargarClubes()
        {
            try
            {
                var clubes = await _clubApiClient.GetAllAsync();
                dataGridClubes.DataSource = clubes;
                LimpiarFormulario();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los clubes: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
        }

        private void LimpiarFormulario()
        {
            textBoxNombreClub.Clear();
            textBoxAbreviatura.Clear();
            textBoxDescripcion.Clear();
            _clubIdSeleccionado = null;
        }

        private void dataGridClubes_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var fila = dataGridClubes.Rows[e.RowIndex];
                _clubIdSeleccionado = Convert.ToInt32(fila.Cells["ColumnID"].Value);
                textBoxNombreClub.Text = fila.Cells["ColumnNombre"].Value?.ToString();
                textBoxAbreviatura.Text = fila.Cells["ColumnAbreviatura"].Value?.ToString();
                textBoxDescripcion.Text = fila.Cells["ColumnDescripcion"].Value?.ToString();
            }
        }

        private async void btnGuardar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxNombreClub.Text) || string.IsNullOrWhiteSpace(textBoxAbreviatura.Text))
            {
                MessageBox.Show("Nombre y Abreviatura son obligatorios.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var nuevoClub = new ClubRequest
            {
                Nombre = textBoxNombreClub.Text.Trim(),
                Abreviatura = textBoxAbreviatura.Text.Trim(),
                Descripcion = string.IsNullOrWhiteSpace(textBoxDescripcion.Text)
                    ? textBoxNombreClub.Text.Trim()
                    : textBoxDescripcion.Text.Trim()
            };

            try
            {
                await _clubApiClient.CreateAsync(nuevoClub);
                await CargarClubes();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar el club: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void buttonModificar_Click(object sender, EventArgs e)
        {
            if (_clubIdSeleccionado == null)
            {
                MessageBox.Show("Seleccione un club de la lista primero.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(textBoxNombreClub.Text) || string.IsNullOrWhiteSpace(textBoxAbreviatura.Text))
            {
                MessageBox.Show("Nombre y Abreviatura son obligatorios.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var clubModificado = new ClubUpdateRequest
            {
                IdClub = _clubIdSeleccionado.Value,
                Nombre = textBoxNombreClub.Text.Trim(),
                Abreviatura = textBoxAbreviatura.Text.Trim(),
                Descripcion = string.IsNullOrWhiteSpace(textBoxDescripcion.Text)
                    ? textBoxNombreClub.Text.Trim()
                    : textBoxDescripcion.Text.Trim()
            };

            try
            {
                await _clubApiClient.UpdateAsync(clubModificado);
                await CargarClubes();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al modificar el club: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void buttonEliminar_Click(object sender, EventArgs e)
        {
            if (_clubIdSeleccionado == null)
            {
                MessageBox.Show("Seleccione un club de la lista primero.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var respuesta = MessageBox.Show("¿Estás seguro de eliminar este club?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (respuesta == DialogResult.Yes)
            {
                try
                {
                    await _clubApiClient.DeleteAsync(_clubIdSeleccionado.Value);
                    await CargarClubes();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al eliminar el club: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
