using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using Kicket.ApiClient.Abstracciones;
using Kicket.Contracts.Common;
using Kicket.Contracts.Usuarios;
using Kicket.WinForms.Controles;

namespace Kicket.WinForms.Forms
{
    // ABM de usuarios: lista, alta, modificación y baja usando la API.
    public partial class FormUsuario : Form
    {
        // Cliente HTTP de usuarios y el id del usuario seleccionado (null si no hay).
        private readonly IUsuarioApiClient _usuarioApiClient;
        private int? _usuarioIdSeleccionado = null;

        // Controles agregados en la versión 2: datos personales, filtros y paginador.
        private readonly TextBox textBoxDni = new() { MaxLength = 10 };
        private readonly TextBox textBoxTelefono = new() { MaxLength = 30 };
        private readonly DateTimePicker dateTimePickerNacimiento = new() { Format = DateTimePickerFormat.Short, ShowCheckBox = true, Checked = false };
        private readonly BarraFiltros _filtros = new();
        private readonly TextBox _filtroTexto;
        private readonly TextBox _filtroDni;
        private readonly ComboBox _filtroRol;
        private readonly ControlPaginacion _paginador = new();
        private PagedResult<UsuarioDto>? _pagina;

        // Constructor: recibe el cliente por inyección, carga el combo de roles y arma los controles nuevos.
        public FormUsuario(IUsuarioApiClient usuarioApiClient)
        {
            InitializeComponent();
            _usuarioApiClient = usuarioApiClient;

            // Opciones del combo de roles: Cliente, Usuario y Admin.
            comboBoxRol.DataSource = new[] { Roles.Cliente, Roles.Usuario, Roles.Admin };
            comboBoxRol.SelectedIndex = 0;

            // DNI al lado del rol; teléfono y fecha de nacimiento en una fila nueva.
            LayoutListado.AgregarCampo(this, "DNI:", textBoxDni, label6.Left, textBoxPassword.Left, comboBoxRol.Top, textBoxPassword.Width);
            int y = LayoutListado.FilaDebajoDe(this, comboBoxRol);
            LayoutListado.AgregarCampo(this, "Teléfono:", textBoxTelefono, labelRol.Left, comboBoxRol.Left, y, comboBoxRol.Width);
            LayoutListado.AgregarCampo(this, "Nacimiento:", dateTimePickerNacimiento, label6.Left, textBoxPassword.Left, y, textBoxPassword.Width);

            dataGridViewUsuarios.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColumnDni", DataPropertyName = "Dni", HeaderText = "DNI" });
            dataGridViewUsuarios.Columns.Add(new DataGridViewTextBoxColumn { Name = "ColumnTelefono", DataPropertyName = "Telefono", HeaderText = "Teléfono" });

            // Filtros: texto (nombre, apellido, email o DNI), DNI exacto y rol.
            _filtroTexto = _filtros.AgregarTexto("Buscar:", 115, "Nombre, email...");
            _filtroDni = _filtros.AgregarTexto("DNI:", 80);
            _filtroRol = _filtros.AgregarCombo("Rol:", 85, "Todos", Roles.Admin, Roles.Usuario, Roles.Cliente);
            _filtros.Buscar += async (_, _) => { _paginador.Reiniciar(); await CargarUsuarios(); };
            _filtros.Limpiar += async (_, _) => { _paginador.Reiniciar(); await CargarUsuarios(); };
            _paginador.PaginaCambiada += async (_, _) => await CargarUsuarios();
            LayoutListado.Envolver(this, dataGridViewUsuarios, _filtros, _paginador);
        }

        // Al cargar: usa las columnas del diseñador y carga los usuarios.
        private async void FormUsuario_Load(object sender, EventArgs e)
        {
            dataGridViewUsuarios.AutoGenerateColumns = false;
            await CargarUsuarios();
        }

        // Pide los usuarios a la API, los muestra en la grilla y limpia el formulario.
        private async Task CargarUsuarios()
        {
            try
            {
                _pagina = await _usuarioApiClient.GetPaginadoAsync(new UsuarioFiltro
                {
                    Pagina = _paginador.Pagina,
                    TamanoPagina = _paginador.TamanoPagina,
                    Busqueda = _filtroTexto.Text,
                    Dni = _filtroDni.Text,
                    Rol = _filtroRol.SelectedIndex > 0 ? _filtroRol.SelectedItem?.ToString() : null,
                    OrdenarPor = "apellido"
                });
                dataGridViewUsuarios.DataSource = _pagina.Items;
                _paginador.Actualizar(_pagina);
                LimpiarFormulario();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los usuarios: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Al hacer click en una fila, guarda el id y copia los datos a los campos (la contraseña queda vacía).
        private void dataGridViewUsuarios_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || _pagina is null || e.RowIndex >= _pagina.Items.Count) return;

            var usuario = _pagina.Items[e.RowIndex];
            _usuarioIdSeleccionado = usuario.IdUsuario;
            textBoxNombre.Text = usuario.Nombre;
            textBoxApellido.Text = usuario.Apellido;
            textBoxEmail.Text = usuario.Email;
            textBoxDni.Text = usuario.Dni;
            textBoxTelefono.Text = usuario.Telefono;
            dateTimePickerNacimiento.Checked = usuario.FechaNacimiento.HasValue;
            if (usuario.FechaNacimiento.HasValue) dateTimePickerNacimiento.Value = usuario.FechaNacimiento.Value;
            // Si el rol de la fila está en el combo, lo selecciona.
            if (comboBoxRol.Items.Contains(usuario.Rol))
            {
                comboBoxRol.SelectedItem = usuario.Rol;
            }
            textBoxPassword.Clear();
        }

        // Botón limpiar.
        private void buttonLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
        }

        // Vacía los campos, vuelve el rol al primero y quita la selección.
        private void LimpiarFormulario()
        {
            textBoxNombre.Clear();
            textBoxApellido.Clear();
            textBoxEmail.Clear();
            textBoxPassword.Clear();
            textBoxDni.Clear();
            textBoxTelefono.Clear();
            dateTimePickerNacimiento.Checked = false;
            if (comboBoxRol.Items.Count > 0) comboBoxRol.SelectedIndex = 0;
            _usuarioIdSeleccionado = null;
        }

        // Crea un usuario nuevo con los datos de los campos.
        private async void buttonGuardar_Click(object sender, EventArgs e)
        {
            // Validación manual: nombre, email y contraseña obligatorios.
            if (string.IsNullOrWhiteSpace(textBoxNombre.Text) || string.IsNullOrWhiteSpace(textBoxEmail.Text))
            {
                MessageBox.Show("Nombre y Email son campos obligatorios.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(textBoxPassword.Text))
            {
                MessageBox.Show("La contraseña es obligatoria para crear un usuario.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // El DNI es obligatorio en las altas (7 u 8 dígitos, con o sin puntos). La API vuelve a validarlo.
            if (!Dni.EsValido(textBoxDni.Text))
            {
                MessageBox.Show(Dni.MensajeInvalido, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Arma el pedido de alta con el rol elegido en el combo.
            var nuevoUsuario = new UsuarioRequest
            {
                Nombre = textBoxNombre.Text.Trim(),
                Apellido = textBoxApellido.Text.Trim(),
                Email = textBoxEmail.Text.Trim(),
                Pass = textBoxPassword.Text,
                Rol = comboBoxRol.SelectedItem?.ToString() ?? Roles.Cliente,
                Dni = textBoxDni.Text.Trim(),
                Telefono = string.IsNullOrWhiteSpace(textBoxTelefono.Text) ? null : textBoxTelefono.Text.Trim(),
                FechaNacimiento = dateTimePickerNacimiento.Checked ? dateTimePickerNacimiento.Value.Date : null
            };

            try
            {
                // POST /usuarios, avisa y recarga la grilla.
                await _usuarioApiClient.CreateAsync(nuevoUsuario);
                MessageBox.Show("Usuario guardado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                await CargarUsuarios();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar el usuario: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Modifica el usuario seleccionado.
        private async void btnModificar_Click(object sender, EventArgs e)
        {
            if (_usuarioIdSeleccionado == null)
            {
                MessageBox.Show("Selecciona un usuario de la grilla para modificar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Si la contraseña está vacía se manda null: la API conserva la contraseña actual.
            var usuarioModificado = new UsuarioUpdateRequest
            {
                IdUsuario = _usuarioIdSeleccionado.Value,
                Nombre = textBoxNombre.Text.Trim(),
                Apellido = textBoxApellido.Text.Trim(),
                Email = textBoxEmail.Text.Trim(),
                Pass = string.IsNullOrWhiteSpace(textBoxPassword.Text) ? null : textBoxPassword.Text,
                Rol = comboBoxRol.SelectedItem?.ToString() ?? Roles.Cliente,
                // DNI vacío: la API conserva el que tenía (no se inventan DNI para usuarios viejos).
                Dni = string.IsNullOrWhiteSpace(textBoxDni.Text) ? null : textBoxDni.Text.Trim(),
                Telefono = string.IsNullOrWhiteSpace(textBoxTelefono.Text) ? null : textBoxTelefono.Text.Trim(),
                FechaNacimiento = dateTimePickerNacimiento.Checked ? dateTimePickerNacimiento.Value.Date : null
            };

            if (usuarioModificado.Dni is not null && !Dni.EsValido(usuarioModificado.Dni))
            {
                MessageBox.Show(Dni.MensajeInvalido, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // PUT /usuarios, avisa y recarga la grilla.
                await _usuarioApiClient.UpdateAsync(usuarioModificado);
                MessageBox.Show("Usuario actualizado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                await CargarUsuarios();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al modificar el usuario: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Elimina el usuario seleccionado, previa confirmación.
        private async void btnEliminar_Click(object sender, EventArgs e)
        {
            if (_usuarioIdSeleccionado == null)
            {
                MessageBox.Show("Selecciona un usuario de la grilla para eliminar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirmacion = MessageBox.Show(
                "¿Estás seguro de que deseas eliminar este usuario?",
                "Confirmar baja",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (confirmacion == DialogResult.Yes)
            {
                try
                {
                    // DELETE /usuarios/{id}, avisa y recarga la grilla.
                    await _usuarioApiClient.DeleteAsync(_usuarioIdSeleccionado.Value);
                    MessageBox.Show("Usuario eliminado.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    await CargarUsuarios();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al eliminar el usuario: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
