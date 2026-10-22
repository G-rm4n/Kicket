using Kiketpropio.Dominio.Entidades;
using Kiketpropio.Escritorio.Servicios;

namespace Kiketpropio.Escritorio.Formularios
{
    public partial class UsuarioEdicionForm : Form
    {
        private readonly Usuario? _usuario;

        public UsuarioEdicionForm(Usuario? usuario)
        {
            InitializeComponent();

            _usuario = usuario;
            if (_usuario is null)
            {
                lblTitulo.Text = "Nuevo usuario";
                dtpFechaNacimiento.Value = DateTime.Today.AddYears(-18);
                cboRol.SelectedItem = "User";
                return;
            }

            lblTitulo.Text = $"Editar usuario {_usuario.IdUsuario}";
            txtDni.Text = _usuario.DNI;
            txtNombre.Text = _usuario.Nombre;
            txtApellido.Text = _usuario.Apellido;
            txtEmail.Text = _usuario.Email;
            txtPass.Text = _usuario.Pass;
            dtpFechaNacimiento.Value = _usuario.FechaNacimiento;
            cboRol.SelectedItem = _usuario.Rol;
        }

        private async void btnGuardar_Click(object? sender, EventArgs e)
        {
            lblError.Text = string.Empty;

            if (string.IsNullOrWhiteSpace(txtEmail.Text) || string.IsNullOrWhiteSpace(txtPass.Text))
            {
                lblError.Text = "El email y contraseña son obligatorios";
                return;
            }

            if (string.IsNullOrWhiteSpace(txtDni.Text) || string.IsNullOrWhiteSpace(txtApellido.Text))
            {
                lblError.Text = "El DNI y el apellido son obligatorios";
                return;
            }

            if (cboRol.SelectedItem is null)
            {
                lblError.Text = "Seleccione el perfil del usuario";
                return;
            }

            btnGuardar.Enabled = false;

            try
            {
                var usuarioInput = new Usuario(
                    _usuario?.IdUsuario ?? 0,
                    txtDni.Text.Trim(),
                    txtNombre.Text.Trim(),
                    txtApellido.Text.Trim(),
                    txtEmail.Text.Trim(),
                    txtPass.Text,
                    dtpFechaNacimiento.Value.Date,
                    cboRol.SelectedItem.ToString()!);

                var resultado = _usuario is null
                    ? await ApiClient.Create("Usuario", usuarioInput)
                    : await ApiClient.Update("Usuario", _usuario.IdUsuario, usuarioInput);

                if (!resultado)
                {
                    lblError.Text = "No se pudo guardar el usuario";
                    return;
                }

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (HttpRequestException)
            {
                lblError.Text = "No se pudo conectar con la API de Kicket";
            }
            finally
            {
                btnGuardar.Enabled = true;
            }
        }

        private void btnCancelar_Click(object? sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
