using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using Kicket.ApiClient.Abstracciones;
using Kicket.Contracts.Usuarios;

namespace Kicket.WinForms.Forms
{
    // ABM de usuarios: lista, alta, modificación y baja usando la API.
    public partial class FormUsuario : Form
    {
        // Cliente HTTP de usuarios y el id del usuario seleccionado (null si no hay).
        private readonly IUsuarioApiClient _usuarioApiClient;
        private int? _usuarioIdSeleccionado = null;

        // Constructor: recibe el cliente por inyección y carga el combo de roles.
        public FormUsuario(IUsuarioApiClient usuarioApiClient)
        {
            InitializeComponent();
            _usuarioApiClient = usuarioApiClient;

            // Opciones del combo de roles: Cliente, Usuario y Admin.
            comboBoxRol.DataSource = new[] { Roles.Cliente, Roles.Usuario, Roles.Admin };
            comboBoxRol.SelectedIndex = 0;
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
                var usuarios = await _usuarioApiClient.GetAllAsync();
                dataGridViewUsuarios.DataSource = usuarios;
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
            if (e.RowIndex >= 0)
            {
                var fila = dataGridViewUsuarios.Rows[e.RowIndex];

                _usuarioIdSeleccionado = Convert.ToInt32(fila.Cells[0].Value);
                textBoxNombre.Text = fila.Cells[1].Value?.ToString();
                textBoxApellido.Text = fila.Cells[2].Value?.ToString();
                textBoxEmail.Text = fila.Cells[3].Value?.ToString();
                // Si el rol de la fila está en el combo, lo selecciona.
                string? rolFila = fila.Cells[4].Value?.ToString();
                if (!string.IsNullOrWhiteSpace(rolFila) && comboBoxRol.Items.Contains(rolFila))
                {
                    comboBoxRol.SelectedItem = rolFila;
                }
                textBoxPassword.Clear();
            }
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

            // Arma el pedido de alta con el rol elegido en el combo.
            var nuevoUsuario = new UsuarioRequest
            {
                Nombre = textBoxNombre.Text.Trim(),
                Apellido = textBoxApellido.Text.Trim(),
                Email = textBoxEmail.Text.Trim(),
                Pass = textBoxPassword.Text,
                Rol = comboBoxRol.SelectedItem?.ToString() ?? Roles.Cliente
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
                Rol = comboBoxRol.SelectedItem?.ToString() ?? Roles.Cliente
            };

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
