using Kicket.ApiClient.Abstracciones;
using Kicket.ApiClient.Http;
using Kicket.Contracts.Auth;
using Kicket.WinForms.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kicket.WinForms
{
    public partial class FormLogin : Form
    {
        private readonly IAuthApiClient _authApiClient;
        private readonly FormPrincipal _formPrincipal;
        public FormLogin(IAuthApiClient authApiClient, FormPrincipal formPrincipal)
        {
            InitializeComponent();

            _authApiClient = authApiClient;
            _formPrincipal = formPrincipal;
        }

        private async Task BtnIngresar_Click(object sender, EventArgs e)
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
            catch(ApiException ex)
            {
                MessageBox.Show($"Error al iniciar sesión: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                BtnIngresar.Enabled = true;
            }
        }
    }
}
