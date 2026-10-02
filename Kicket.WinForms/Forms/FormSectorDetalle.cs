using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using Kicket.ApiClient.Abstracciones;
using Kicket.Contracts.Sectores;

namespace Kicket.WinForms.Forms
{
    public partial class FormSectorDetalle : Form
    {
        private readonly ISectorApiClient _sectorApiClient;
        private int _estadioId;
        private int? _sectorIdSeleccionado;

        public FormSectorDetalle(ISectorApiClient sectorApiClient)
        {
            InitializeComponent();
            _sectorApiClient = sectorApiClient;
        }

        /// <summary>
        /// Hay que llamarlo despues de resolver el formulario desde el contenedor de DI
        /// y antes de ShowDialog(): el estadio no se conoce hasta que FormEstadio lo pasa.
        /// </summary>
        public void Inicializar(int estadioId, string nombreEstadio)
        {
            _estadioId = estadioId;
            labelTitulo.Text = $"Sectores de: {nombreEstadio}";
            Text = $"Sectores - {nombreEstadio}";
        }

        protected override async void OnShown(EventArgs e)
        {
            base.OnShown(e);
            dataGridViewSectores.AutoGenerateColumns = false;
            await CargarSectores();
        }

        private async Task CargarSectores()
        {
            try
            {
                var sectores = await _sectorApiClient.GetByEstadioIdAsync(_estadioId);
                dataGridViewSectores.DataSource = sectores;
                LimpiarFormulario();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los sectores: {ex.Message}");
            }
        }

        private void dataGridSectores_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var fila = dataGridViewSectores.Rows[e.RowIndex];

                // Columnas visuales: SectorId (0), Nombre (1), CapacidadMaxima (2), PrecioBase (3)
                _sectorIdSeleccionado = Convert.ToInt32(fila.Cells[0].Value);
                textBoxNombre.Text = fila.Cells[1].Value?.ToString();
                textBoxCapacidadMaxima.Text = fila.Cells[2].Value?.ToString();
                textBoxPrecioBase.Text = fila.Cells[3].Value?.ToString();
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
        }

        private void LimpiarFormulario()
        {
            textBoxNombre.Clear();
            textBoxCapacidadMaxima.Clear();
            textBoxPrecioBase.Clear();
            _sectorIdSeleccionado = null;
        }

        private async void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!ValidarCampos(out int capacidadMaxima, out decimal precioBase))
            {
                return;
            }

            var nuevoSector = new SectorRequest
            {
                EstadioId = _estadioId,
                Nombre = textBoxNombre.Text,
                CapacidadMaxima = capacidadMaxima,
                PrecioBase = precioBase
            };

            try
            {
                await _sectorApiClient.CreateAsync(nuevoSector);
                await CargarSectores();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar el sector: {ex.Message}");
            }
        }

        private async void btnModificar_Click(object sender, EventArgs e)
        {
            if (_sectorIdSeleccionado is null)
            {
                MessageBox.Show("Seleccione un sector de la lista primero.", "Aviso");
                return;
            }

            if (!ValidarCampos(out int capacidadMaxima, out decimal precioBase))
            {
                return;
            }

            var sectorModificado = new SectorUpdateRequest
            {
                SectorId = _sectorIdSeleccionado.Value,
                EstadioId = _estadioId,
                Nombre = textBoxNombre.Text,
                CapacidadMaxima = capacidadMaxima,
                PrecioBase = precioBase
            };

            try
            {
                await _sectorApiClient.UpdateAsync(sectorModificado);
                await CargarSectores();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al modificar el sector: {ex.Message}");
            }
        }

        private async void btnEliminar_Click(object sender, EventArgs e)
        {
            if (_sectorIdSeleccionado is null) return;

            var respuesta = MessageBox.Show("¿Está seguro de eliminar este sector?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (respuesta == DialogResult.Yes)
            {
                try
                {
                    await _sectorApiClient.DeleteAsync(_sectorIdSeleccionado.Value);
                    await CargarSectores();
                }
                catch (Exception ex)
                {
                    // Cubre, por ejemplo, el 409 del servidor cuando el sector ya tiene entradas vendidas.
                    MessageBox.Show($"Error al eliminar el sector: {ex.Message}");
                }
            }
        }

        private void buttonCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private bool ValidarCampos(out int capacidadMaxima, out decimal precioBase)
        {
            capacidadMaxima = 0;
            precioBase = 0;

            if (string.IsNullOrWhiteSpace(textBoxNombre.Text))
            {
                MessageBox.Show("El nombre del sector es obligatorio.", "Aviso");
                return false;
            }

            if (!int.TryParse(textBoxCapacidadMaxima.Text, out capacidadMaxima) || capacidadMaxima <= 0)
            {
                MessageBox.Show("La capacidad máxima debe ser un número entero mayor a cero.", "Aviso");
                return false;
            }

            if (!decimal.TryParse(textBoxPrecioBase.Text, out precioBase) || precioBase <= 0)
            {
                MessageBox.Show("El precio base debe ser un número mayor a cero.", "Aviso");
                return false;
            }

            return true;
        }
    }
}