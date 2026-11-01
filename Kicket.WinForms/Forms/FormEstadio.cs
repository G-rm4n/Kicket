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
using Microsoft.Extensions.DependencyInjection;

namespace Kicket.WinForms.Forms
{
    // ABM de estadios: lista, alta, modificación, baja y acceso a los sectores de cada estadio.
    public partial class FormEstadio : Form
    {
        // Cliente HTTP de estadios y el id del estadio seleccionado (null si no hay).
        private readonly IEstadioApiClient _estadioApiClient;
        private int? _estadioIdSeleccionado = null;

        // Constructor: recibe el cliente de estadios por inyección.
        public FormEstadio(IEstadioApiClient estadioApiClient)
        {
            InitializeComponent();
            _estadioApiClient = estadioApiClient;
        }

        // --- 1. CARGA INICIAL ---
        // Al cargar: usa las columnas del diseñador y carga los estadios.
        private async void FormEstadio_Load(object sender, EventArgs e)
        {
            dataGridViewEstadios.AutoGenerateColumns = false;
            await CargarEstadios();
        }

        // Pide los estadios a la API, los muestra en la grilla y limpia el formulario.
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
        // Al hacer click en una fila, guarda el id y copia los datos a los campos.
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

        // --- 2b. BOTÓN "Sectores" DE LA FILA: abre el maestro-detalle en una ventana aparte ---
        // Si se hizo click en el botón "Sectores" de una fila, abre el formulario de sectores de ese estadio.
        private void dataGridEstadios_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex != ColumnSectores.Index)
            {
                return;
            }

            var fila = dataGridViewEstadios.Rows[e.RowIndex];
            int estadioId = Convert.ToInt32(fila.Cells[0].Value);
            string nombreEstadio = fila.Cells[1].Value?.ToString() ?? string.Empty;

            // Pide el formulario al contenedor, le pasa el estadio y lo abre como ventana modal.
            var formSectores = Program.ServiceProvider.GetRequiredService<FormSectorDetalle>();
            formSectores.Inicializar(estadioId, nombreEstadio);
            formSectores.ShowDialog(this);
        }

        // --- 3. BOTÓN LIMPIAR ---
        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
        }

        // Vacía los campos y quita la selección.
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
            // Validación manual: nombre obligatorio.
            if (string.IsNullOrWhiteSpace(textBoxNombre.Text))
            {
                MessageBox.Show("El nombre del estadio es obligatorio.", "Aviso");
                return;
            }

            // Arma el pedido de alta con los datos de los campos.
            var nuevoEstadio = new EstadioRequest
            {
                Nombre = textBoxNombre.Text,
                Direccion = textBoxDireccion.Text,
                Ciudad = textBoxCiudad.Text
            };

            try
            {
                // POST /estadios y recarga la grilla.
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
            // Tiene que haber un estadio seleccionado.
            if (_estadioIdSeleccionado == null)
            {
                MessageBox.Show("Seleccione un estadio de la lista primero.", "Aviso");
                return;
            }

            // Arma el pedido de modificación con el id seleccionado.
            var estadioModificado = new EstadioUpdateRequest
            {
                IdEstadio = _estadioIdSeleccionado.Value,
                Nombre = textBoxNombre.Text,
                Direccion = textBoxDireccion.Text,
                Ciudad = textBoxCiudad.Text
            };

            try
            {
                // PUT /estadios y recarga la grilla.
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
            // Sin selección no hace nada; con selección pide confirmación.
            if (_estadioIdSeleccionado == null) return;

            var respuesta = MessageBox.Show("¿Estás seguro de eliminar este estadio?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (respuesta == DialogResult.Yes)
            {
                try
                {
                    // DELETE /estadios/{id} y recarga la grilla. Si tiene sectores, la API devuelve error.
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