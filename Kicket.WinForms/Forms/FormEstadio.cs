using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Kicket.ApiClient.Abstracciones;
using Kicket.Contracts.Estadios;

namespace Kicket.WinForms.Forms
{
    public partial class FormEstadio : Form
    {
        private readonly IEstadioApiClient _estadioApiClient;
        private int? _estadioIdSeleccionado = null;

        public FormEstadio(IEstadioApiClient estadioApiClient)
        {
            InitializeComponent();
            _estadioApiClient = estadioApiClient;
        }

        // --- 1. CARGA INICIAL ---
        private async void FormEstadio_Load(object sender, EventArgs e)
        {
            dataGridViewEstadios.AutoGenerateColumns = false;
            await CargarEstadios();
        }

        private async Task CargarEstadios()
        {
            try
            {
                var estadios = await _estadioApiClient.GetAllAsync();
                dataGridViewEstadios.DataSource = estadios;
                LimpiarFormulario();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los estadios: {ex.Message}");
            }
        }

        // --- 2. SELECCIONAR DE LA TABLA ---
        private void dataGridEstadios_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var fila = dataGridViewEstadios.Rows[e.RowIndex];

                // Asume que las columnas visuales están en este orden: ID (0), Nombre (1), Direccion (2), Ciudad (3)
                _estadioIdSeleccionado = Convert.ToInt32(fila.Cells[0].Value);
                textBoxNombre.Text = fila.Cells[1].Value?.ToString();
                textBoxDireccion.Text = fila.Cells[2].Value?.ToString();
                textBoxCiudad.Text = fila.Cells[3].Value?.ToString();
            }
        }

        // --- 3. BOTÓN LIMPIAR ---
        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
        }

        private void LimpiarFormulario()
        {
            textBoxNombre.Clear();
            textBoxDireccion.Clear();
            textBoxCiudad.Clear();
            _estadioIdSeleccionado = null;
        }

        // --- 4. BOTÓN GUARDAR (Create) ---
        private async void btnGuardar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxNombre.Text))
            {
                MessageBox.Show("El nombre del estadio es obligatorio.", "Aviso");
                return;
            }

            var nuevoEstadio = new EstadioRequest
            {
                Nombre = textBoxNombre.Text,
                Direccion = textBoxDireccion.Text,
                Ciudad = textBoxCiudad.Text
            };

            try
            {
                await _estadioApiClient.CreateAsync(nuevoEstadio);
                await CargarEstadios();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar: {ex.Message}");
            }
        }

        // --- 5. BOTÓN MODIFICAR (Update) ---
        private async void btnModificar_Click(object sender, EventArgs e)
        {
            if (_estadioIdSeleccionado == null)
            {
                MessageBox.Show("Seleccione un estadio de la lista primero.", "Aviso");
                return;
            }

            var estadioModificado = new EstadioUpdateRequest
            {
                IdEstadio = _estadioIdSeleccionado.Value,
                Nombre = textBoxNombre.Text,
                Direccion = textBoxDireccion.Text,
                Ciudad = textBoxCiudad.Text
            };

            try
            {
                await _estadioApiClient.UpdateAsync(estadioModificado);
                await CargarEstadios();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al modificar: {ex.Message}");
            }
        }

        // --- 6. BOTÓN ELIMINAR (Delete) ---
        private async void btnEliminar_Click(object sender, EventArgs e)
        {
            if (_estadioIdSeleccionado == null) return;

            var respuesta = MessageBox.Show("¿Estás seguro de eliminar este estadio?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (respuesta == DialogResult.Yes)
            {
                try
                {
                    await _estadioApiClient.DeleteAsync(_estadioIdSeleccionado.Value);
                    await CargarEstadios();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al eliminar: {ex.Message}");
                }
            }
        }
    }
}