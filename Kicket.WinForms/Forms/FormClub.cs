using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using Kicket.ApiClient.Abstracciones;
using Kicket.Contracts.Clubes;

namespace Kicket.WinForms.Forms
{
    // ABM de clubes: lista, alta, modificación y baja usando la API.
    public partial class FormClub : Form
    {
        // Cliente HTTP de clubes y el id del club seleccionado en la grilla (null si no hay).
        private readonly IClubApiClient _clubApiClient;
        private int? _clubIdSeleccionado = null;

        // Constructor: recibe el cliente de clubes por inyección.
        public FormClub(IClubApiClient clubApiClient)
        {
            InitializeComponent();
            _clubApiClient = clubApiClient;
        }

        // Al cargar: usa las columnas definidas en el diseñador y carga los clubes.
        private async void FormClub_Load(object sender, EventArgs e)
        {
            dataGridClubes.AutoGenerateColumns = false;
            await CargarClubes();
        }

        // Pide los clubes a la API, los muestra en la grilla y limpia el formulario.
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

        // Botón limpiar.
        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
        }

        // Vacía los campos y quita la selección.
        private void LimpiarFormulario()
        {
            textBoxNombreClub.Clear();
            textBoxAbreviatura.Clear();
            textBoxDescripcion.Clear();
            _clubIdSeleccionado = null;
        }

        // Al hacer click en una fila, guarda el id y copia los datos a los campos.
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

        // Crea un club nuevo con los datos de los campos.
        private async void btnGuardar_Click(object sender, EventArgs e)
        {
            // Validación manual: nombre y abreviatura obligatorios.
            if (string.IsNullOrWhiteSpace(textBoxNombreClub.Text) || string.IsNullOrWhiteSpace(textBoxAbreviatura.Text))
            {
                MessageBox.Show("Nombre y Abreviatura son obligatorios.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Si la descripción está vacía, usa el nombre como descripción (la API la exige).
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
                // POST /clubes y recarga la grilla.
                await _clubApiClient.CreateAsync(nuevoClub);
                await CargarClubes();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar el club: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Modifica el club seleccionado.
        private async void buttonModificar_Click(object sender, EventArgs e)
        {
            // Tiene que haber un club seleccionado.
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

            // Arma el pedido de modificación con el id del club seleccionado.
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
                // PUT /clubes y recarga la grilla.
                await _clubApiClient.UpdateAsync(clubModificado);
                await CargarClubes();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al modificar el club: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Elimina el club seleccionado, previa confirmación.
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
                    // DELETE /clubes/{id} y recarga la grilla. Si tiene eventos, la API devuelve error.
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
