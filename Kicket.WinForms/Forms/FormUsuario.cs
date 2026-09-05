using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using Kicket.ApiClient.Abstracciones;
using Kicket.Contracts.Usuarios;

namespace Kicket.WinForms.Forms
{
    public partial class FormUsuario : Form
    {
        private readonly IUsuarioApiClient _usuarioApiClient;
        private int? _usuarioIdSeleccionado = null;

        public FormUsuario(IUsuarioApiClient usuarioApiClient)
        {
            InitializeComponent();
            _usuarioApiClient = usuarioApiClient;
        }

        // --- CARGA INICIAL ---
        private async void FormUsuario_Load(object sender, EventArgs e)
        {
            dataGridViewUsuarios.AutoGenerateColumns = false;
            await CargarUsuarios();
        }

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

        // --- SELECCIÓN EN TABLA ---
        private void dataGridUsuarios_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var fila = dataGridViewUsuarios.Rows[e.RowIndex];

                _usuarioIdSeleccionado = Convert.ToInt32(fila.Cells[0].Value);
                textBoxNombre.Text = fila.Cells[1].Value?.ToString();
                textBoxApellido.Text = fila.Cells[2].Value?.ToString();
                textBoxEmail.Text = fila.Cells[3].Value?.ToString();
            }
        }

        // --- LIMPIAR FORMULARIO ---
        private void buttonLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
        }

        private void LimpiarFormulario()
        {
            textBoxNombre.Clear();
            textBoxApellido.Clear();
            textBoxEmail.Clear();
            // textBoxPassword.Clear(); // Descomentar si usas campo de contraseña
            _usuarioIdSeleccionado = null;
        }

        // --- CREAR (POST) ---
        private async void buttonGuardar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxNombre.Text) || string.IsNullOrWhiteSpace(textBoxEmail.Text))
            {
                MessageBox.Show("Nombre y Email son campos obligatorios.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var nuevoUsuario = new UsuarioRequest
            {
                Nombre = textBoxNombre.Text.Trim(),
                Apellido = textBoxApellido.Text.Trim(),
                Email = textBoxEmail.Text.Trim()
                // Password = textBoxPassword.Text // Descomentar si UsuarioRequest exige Password
            };

            try
            {
                await _usuarioApiClient.CreateAsync(nuevoUsuario);
                MessageBox.Show("Usuario guardado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                await CargarUsuarios();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar el usuario: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // --- ACTUALIZAR (PUT) ---
        private async void btnModificar_Click(object sender, EventArgs e)
        {
            if (_usuarioIdSeleccionado == null)
            {
                MessageBox.Show("Selecciona un usuario de la grilla para modificar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var usuarioModificado = new UsuarioUpdateRequest
            {
                IdUsuario = _usuarioIdSeleccionado.Value,
                Nombre = textBoxNombre.Text.Trim(),
                Apellido = textBoxApellido.Text.Trim(),
                Email = textBoxEmail.Text.Trim()
            };

            try
            {
                await _usuarioApiClient.UpdateAsync(usuarioModificado);
                MessageBox.Show("Usuario actualizado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                await CargarUsuarios();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al modificar el usuario: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // --- ELIMINAR (DELETE) ---
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