using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Kicket.ApiClient.Abstracciones;
using Kicket.Contracts.Clubes;
using Kicket.Contracts.Common;
using Kicket.WinForms.Controles;

namespace Kicket.WinForms.Forms
{
    // ABM de clubes: lista paginada con filtros, alta, modificación y baja usando la API.
    public partial class FormClub : Form
    {
        // Cliente HTTP de clubes y el id del club seleccionado en la grilla (null si no hay).
        private readonly IClubApiClient _clubApiClient;
        private int? _clubIdSeleccionado = null;

        // Controles agregados en la versión 2: campos nuevos, filtros y paginador.
        private readonly TextBox textBoxCiudad = new();
        private readonly TextBox textBoxFundacion = new() { MaxLength = 4 };
        private readonly BarraFiltros _filtros = new();
        private readonly TextBox _filtroTexto;
        private readonly TextBox _filtroCiudad;
        private readonly ControlPaginacion _paginador = new();
        private PagedResult<ClubDto>? _pagina;

        // Constructor: recibe el cliente de clubes por inyección y arma los controles nuevos.
        public FormClub(IClubApiClient clubApiClient)
        {
            InitializeComponent();
            _clubApiClient = clubApiClient;

            // Filas con Ciudad y Año de fundación debajo de Descripción (posiciones relativas a los campos existentes).
            int y = LayoutListado.FilaDebajoDe(this, textBoxDescripcion);
            LayoutListado.AgregarCampo(this, "Ciudad:", textBoxCiudad, labelDescripcion.Left, textBoxDescripcion.Left, y, textBoxDescripcion.Width);
            y = LayoutListado.FilaDebajoDe(this, textBoxCiudad);
            LayoutListado.AgregarCampo(this, "Fundación:", textBoxFundacion, labelDescripcion.Left, textBoxDescripcion.Left, y, textBoxAbreviatura.Width / 2);

            // Columnas nuevas en la grilla.
            dataGridClubes.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColumnCiudad", DataPropertyName = "Ciudad", HeaderText = "Ciudad" });
            dataGridClubes.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColumnFundacion", DataPropertyName = "AnioFundacion", HeaderText = "Fundación" });

            // Filtros arriba de la grilla y paginador abajo.
            _filtroTexto = _filtros.AgregarTexto("Buscar:", 150, "Nombre o sigla");
            _filtroCiudad = _filtros.AgregarTexto("Ciudad:", 90);
            _filtros.Buscar += async (_, _) => { _paginador.Reiniciar(); await CargarClubes(); };
            _filtros.Limpiar += async (_, _) => { _paginador.Reiniciar(); await CargarClubes(); };
            _paginador.PaginaCambiada += async (_, _) => await CargarClubes();
            LayoutListado.Envolver(this, dataGridClubes, _filtros, _paginador);
        }

        // Al cargar: usa las columnas definidas en el diseñador y carga la primera página.
        private async void FormClub_Load(object sender, EventArgs e)
        {
            dataGridClubes.AutoGenerateColumns = false;
            await CargarClubes();
        }

        // Pide a la API la página actual con los filtros, la muestra en la grilla y limpia el formulario.
        private async Task CargarClubes()
        {
            _filtros.Bloquear(true);
            _paginador.Bloquear(true);
            try
            {
                var filtro = new ClubFiltro
                {
                    Pagina = _paginador.Pagina,
                    TamanoPagina = _paginador.TamanoPagina,
                    Busqueda = _filtroTexto.Text,
                    Ciudad = _filtroCiudad.Text,
                    OrdenarPor = "nombre"
                };
                _pagina = await _clubApiClient.GetPaginadoAsync(filtro);
                dataGridClubes.DataSource = _pagina.Items;
                _paginador.Actualizar(_pagina);
                LimpiarFormulario();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los clubes: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _filtros.Bloquear(false);
                _paginador.Bloquear(false);
                if (_pagina is not null) _paginador.Actualizar(_pagina);
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
            textBoxCiudad.Clear();
            textBoxFundacion.Clear();
            _clubIdSeleccionado = null;
        }

        // Al hacer click en una fila, guarda el id y copia los datos del club a los campos.
        private void dataGridClubes_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || _pagina is null || e.RowIndex >= _pagina.Items.Count) return;

            var club = _pagina.Items[e.RowIndex];
            _clubIdSeleccionado = club.ClubId;
            textBoxNombreClub.Text = club.Nombre;
            textBoxAbreviatura.Text = club.Abreviatura;
            textBoxDescripcion.Text = club.Descripcion;
            textBoxCiudad.Text = club.Ciudad;
            textBoxFundacion.Text = club.AnioFundacion?.ToString();
        }

        // Lee el año de fundación (vacío = sin dato). Devuelve false si no es un número.
        private bool LeerFundacion(out int? anio)
        {
            anio = null;
            if (string.IsNullOrWhiteSpace(textBoxFundacion.Text)) return true;
            if (int.TryParse(textBoxFundacion.Text.Trim(), out var valor))
            {
                anio = valor;
                return true;
            }
            MessageBox.Show("El año de fundación tiene que ser un número (por ejemplo 1903).", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
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
            if (!LeerFundacion(out var anio)) return;

            // Si la descripción está vacía, usa el nombre como descripción (la API la exige).
            var nuevoClub = new ClubRequest
            {
                Nombre = textBoxNombreClub.Text.Trim(),
                Abreviatura = textBoxAbreviatura.Text.Trim(),
                Descripcion = string.IsNullOrWhiteSpace(textBoxDescripcion.Text)
                    ? textBoxNombreClub.Text.Trim()
                    : textBoxDescripcion.Text.Trim(),
                Ciudad = textBoxCiudad.Text.Trim(),
                AnioFundacion = anio
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
            if (!LeerFundacion(out var anio)) return;

            // Arma el pedido de modificación con el id del club seleccionado.
            var clubModificado = new ClubUpdateRequest
            {
                IdClub = _clubIdSeleccionado.Value,
                Nombre = textBoxNombreClub.Text.Trim(),
                Abreviatura = textBoxAbreviatura.Text.Trim(),
                Descripcion = string.IsNullOrWhiteSpace(textBoxDescripcion.Text)
                    ? textBoxNombreClub.Text.Trim()
                    : textBoxDescripcion.Text.Trim(),
                Ciudad = textBoxCiudad.Text.Trim(),
                AnioFundacion = anio
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
                    // DELETE /clubes/{id} y recarga la grilla. Si tiene eventos, la API responde 409.
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
