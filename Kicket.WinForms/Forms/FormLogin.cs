using System;
using System.Windows.Forms;
using Kicket.ApiClient.Abstracciones;
using Kicket.ApiClient.Http;
using Kicket.Contracts.Auth;
using Kicket.Contracts.Usuarios;
using Kicket.WinForms.Forms;
using Microsoft.Extensions.DependencyInjection;

namespace Kicket.WinForms
{
    public partial class FormLogin : Form
    {
        private readonly IAuthApiClient _authApiClient;
        private readonly IServiceProvider _serviceProvider;

        public FormLogin(IAuthApiClient authApiClient, IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _authApiClient = authApiClient;
            _serviceProvider = serviceProvider;
        }

        private async void BtnIngresar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtEmail.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Por favor, ingrese usuario y contraseña.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                BtnIngresar.Enabled = false;
                var request = new LoginRequest
                {
                    Email = txtEmail.Text.Trim(),
                    Pass = txtPassword.Text
                };

                var response = await _authApiClient.LoginAsync(request);

                if (!string.Equals(response.Usuario.Rol, Roles.Admin, StringComparison.OrdinalIgnoreCase))
                {
                    _authApiClient.Logout();
                    MessageBox.Show(
                        "Acceso denegado. La aplicación de escritorio es exclusiva para Administradores.\nLos clientes deben ingresar desde la aplicación Web.",
                        "Permisos Insuficientes",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                Hide();
                using (var formPrincipal = _serviceProvider.GetRequiredService<FormPrincipal>())
                {
                    var dialogResult = formPrincipal.ShowDialog();
                    if (dialogResult == DialogResult.Retry)
                    {
                        txtPassword.Clear();
                        Show();
                        return;
                    }
                }
                Close();
            }
            catch (ApiException ex)
            {
                MessageBox.Show($"Error al iniciar sesión: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error inesperado: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (!IsDisposed)
                {
                    BtnIngresar.Enabled = true;
                }
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Hide();

            using (var registerForm = _serviceProvider.GetRequiredService<FormRegistro>())
            {
                DialogResult resultado = registerForm.ShowDialog();
                Show();

                if (resultado == DialogResult.OK)
                {
                    MessageBox.Show(this,
                        "¡Registro exitoso! Ya puedes ingresar con tu cuenta.",
                        "Bienvenido",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }
        }

        private void FormLogin_Load(object sender, EventArgs e)
        {
        }

        private void ModoLogin()
        {
        }
    }
}
