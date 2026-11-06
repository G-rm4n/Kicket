using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using Kicket.ApiClient.Abstracciones;
using Kicket.Contracts.Common;
using Kicket.Contracts.Sectores;
using Kicket.WinForms.Controles;

namespace Kicket.WinForms.Forms
{
    // Formulario maestro-detalle: ABM de los sectores de un estadio.
    public partial class FormSectorDetalle : Form
    {
        // Cliente HTTP de sectores, estadio actual y sector seleccionado (null si no hay).
        private readonly ISectorApiClient _sectorApiClient;
        private int _estadioId;
        private int? _sectorIdSeleccionado;

        // Filtros y paginador (versión 2).
        private readonly BarraFiltros _filtros = new();
        private readonly TextBox _filtroNombre;
        private readonly ComboBox _filtroTipo;
        private readonly ControlPaginacion _paginador = new();
        private PagedResult<SectorDto>? _pagina;

        // Constructor: recibe el cliente de sectores por inyección y arma filtros y paginador.
        public FormSectorDetalle(ISectorApiClient sectorApiClient)
        {
            InitializeComponent();
            _sectorApiClient = sectorApiClient;

            // Tipo: no es una columna; la API lo deduce del nombre ("Platea" = numerado).
            dataGridViewSectores.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColumnTipo", DataPropertyName = "Tipo", HeaderText = "Tipo" });

            _filtroNombre = _filtros.AgregarTexto("Nombre:", 110);
            _filtroTipo = _filtros.AgregarCombo("Tipo:", 90, "Todos", SectorFiltro.TipoPlatea, SectorFiltro.TipoPopular);
            _filtros.Buscar += async (_, _) => { _paginador.Reiniciar(); await CargarSectores(); };
            _filtros.Limpiar += async (_, _) => { _paginador.Reiniciar(); await CargarSectores(); };
            _paginador.PaginaCambiada += async (_, _) => await CargarSectores();
            LayoutListado.Envolver(this, dataGridViewSectores, _filtros, _paginador);
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

        // Cuando el formulario ya se mostró: usa las columnas del diseñador y carga los sectores.
        protected override async void OnShown(EventArgs e)
        {
            base.OnShown(e);
            dataGridViewSectores.AutoGenerateColumns = false;
            await CargarSectores();
        }

        // Pide a la API una página de sectores de este estadio (con los filtros) y la muestra en la grilla.
        private async Task CargarSectores()
        {
            try
            {
                _pagina = await _sectorApiClient.GetPaginadoAsync(new SectorFiltro
                {
                    EstadioId = _estadioId,
                    Pagina = _paginador.Pagina,
                    TamanoPagina = _paginador.TamanoPagina,
                    Busqueda = _filtroNombre.Text,
                    Tipo = _filtroTipo.SelectedIndex > 0 ? _filtroTipo.SelectedItem?.ToString() : null,
                    OrdenarPor = "nombre"
                });
                dataGridViewSectores.DataSource = _pagina.Items;
                _paginador.Actualizar(_pagina);
                LimpiarFormulario();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los sectores: {ex.Message}");
            }
        }

        // Al hacer click en una fila, guarda el id y copia los datos a los campos.
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

        // Botón limpiar.
        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
        }

        // Vacía los campos y quita la selección.
        private void LimpiarFormulario()
        {
            textBoxNombre.Clear();
            textBoxCapacidadMaxima.Clear();
            textBoxPrecioBase.Clear();
            _sectorIdSeleccionado = null;
        }

        // Crea un sector nuevo para el estadio actual.
        private async void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!ValidarCampos(out int capacidadMaxima, out decimal precioBase))
            {
                return;
            }

            // Arma el pedido de alta con los valores ya validados.
            var nuevoSector = new SectorRequest
            {
                EstadioId = _estadioId,
                Nombre = textBoxNombre.Text,
                CapacidadMaxima = capacidadMaxima,
                PrecioBase = precioBase
            };

            try
            {
                // POST /sectores y recarga la grilla.
                await _sectorApiClient.CreateAsync(nuevoSector);
                await CargarSectores();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar el sector: {ex.Message}");
            }
        }

        // Modifica el sector seleccionado.
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

            // Arma el pedido de modificación con el id seleccionado.
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
                // PUT /sectores y recarga la grilla.
                await _sectorApiClient.UpdateAsync(sectorModificado);
                await CargarSectores();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al modificar el sector: {ex.Message}");
            }
        }

        // Elimina el sector seleccionado, previa confirmación.
        private async void btnEliminar_Click(object sender, EventArgs e)
        {
            if (_sectorIdSeleccionado is null) return;

            var respuesta = MessageBox.Show("¿Está seguro de eliminar este sector?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (respuesta == DialogResult.Yes)
            {
                try
                {
                    // DELETE /sectores/{id} y recarga la grilla.
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

        // Cierra el formulario.
        private void buttonCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }

        // Validación manual: nombre obligatorio, capacidad entera mayor a cero y precio decimal mayor a cero.
        // Devuelve los valores convertidos por parámetros out.
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