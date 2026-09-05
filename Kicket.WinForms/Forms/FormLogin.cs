using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Kicket.ApiClient.Abstracciones;
using Kicket.ApiClient.Http;
using Kicket.Contracts.Auth;
using Kicket.WinForms.Forms;
using Microsoft.Extensions.DependencyInjection;

namespace Kicket.WinForms
{
    public partial class FormLogin : Form
    {
        private readonly IAuthApiClient _authApiClient;
        private readonly IServiceProvider _serviceProvider;
        private readonly FormPrincipal _formPrincipal;
        public FormLogin(IAuthApiClient authApiClient,IServiceProvider serviceProvider, FormPrincipal formPrincipal)
        {
            InitializeComponent();

            _authApiClient = authApiClient;
            _formPrincipal = formPrincipal;
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
                    Email = txtEmail.Text,
                    Pass = txtPassword.Text
                };
                var response = await _authApiClient.LoginAsync(request);
                Hide();
                _formPrincipal.ShowDialog();
                Close();
            }
            catch (ApiException ex)
            {
                MessageBox.Show($"Error al iniciar sesión: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                BtnIngresar.Enabled = true;
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();

            using (var registerForm = _serviceProvider.GetRequiredService<FormRegistro>())
            {
                DialogResult resultado = registerForm.ShowDialog();
                this.Show();

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
