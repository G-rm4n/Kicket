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
    // Formulario de inicio de sesión. Solo deja entrar a usuarios con rol Admin.
    public partial class FormLogin : Form
    {
        // Cliente de autenticación y contenedor de servicios (para abrir otros formularios).
        private readonly IAuthApiClient _authApiClient;
        private readonly IServiceProvider _serviceProvider;

        // Constructor: recibe sus dependencias por inyección.
        public FormLogin(IAuthApiClient authApiClient, IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _authApiClient = authApiClient;
            _serviceProvider = serviceProvider;
        }

        // Valida los campos, hace el login contra la API y, si es Admin, abre el menú principal.
        private async void BtnIngresar_Click(object sender, EventArgs e)
        {
            // Email y contraseña obligatorios.
            if (string.IsNullOrWhiteSpace(txtEmail.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Por favor, ingrese usuario y contraseña.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                BtnIngresar.Enabled = false;
                // Arma el pedido de login.
                var request = new LoginRequest
                {
                    Email = txtEmail.Text.Trim(),
                    Pass = txtPassword.Text
                };

                // POST /auth/login; si sale bien la sesión queda guardada.
                var response = await _authApiClient.LoginAsync(request);

                // Si el usuario no es Admin, cierra la sesión y le niega el acceso.
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

                // Oculta el login y abre el menú principal como ventana modal.
                Hide();
                using (var formPrincipal = _serviceProvider.GetRequiredService<FormPrincipal>())
                {
                    var dialogResult = formPrincipal.ShowDialog();
                    // Si el menú devolvió Retry (cerró sesión), vuelve a mostrar el login.
                    if (dialogResult == DialogResult.Retry)
                    {
                        txtPassword.Clear();
                        Show();
                        return;
                    }
                }
                // Si se cerró el menú de otra forma, se cierra la aplicación.
                Close();
            }
            // Error devuelto por la API (por ejemplo credenciales incorrectas).
            catch (ApiException ex)
            {
                MessageBox.Show($"Error al iniciar sesión: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            // Cualquier otro error.
            catch (Exception ex)
            {
                MessageBox.Show($"Error inesperado: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            // Vuelve a habilitar el botón si el formulario sigue abierto.
            finally
            {
                if (!IsDisposed)
                {
                    BtnIngresar.Enabled = true;
                }
            }
        }

        // Link "registrarse": abre el formulario de registro y avisa si el registro salió bien.
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

        // Evento de carga (vacío).
        private void FormLogin_Load(object sender, EventArgs e)
        {
        }

        // Método vacío (sin uso).
        private void ModoLogin()
        {
        }
    }
}
