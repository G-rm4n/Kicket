using Kiketpropio.Dominio.DTOs;
using Kiketpropio.Escritorio.Servicios;

namespace Kiketpropio.Escritorio.Formularios
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private async void btnIngresar_Click(object? sender, EventArgs e)
        {
            lblError.Text = string.Empty;

            if (string.IsNullOrWhiteSpace(txtEmail.Text) || string.IsNullOrWhiteSpace(txtPass.Text))
            {
                lblError.Text = "El email y contraseña son obligatorios";
                return;
            }

            btnIngresar.Enabled = false;

            try
            {
                var resultado = await ApiClient.Login(new LoginRequest(txtEmail.Text.Trim(), txtPass.Text));
                if (resultado is null)
                {
                    lblError.Text = "Email o contraseña incorrectos";
                    return;
                }

                Sesion.Token = resultado.Token;
                Sesion.Nombre = $"{resultado.Nombre} {resultado.Apellido}";
                Sesion.Email = resultado.Email;
                Sesion.Rol = resultado.Rol;

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (HttpRequestException)
            {
                lblError.Text = "No se pudo conectar con la API de Kicket";
            }
            finally
            {
                btnIngresar.Enabled = true;
            }
        }

        private void btnCancelar_Click(object? sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
