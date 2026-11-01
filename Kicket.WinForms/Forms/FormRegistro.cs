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
using Kicket.Contracts.Usuarios;

namespace Kicket.WinForms.Forms
{
    // Formulario de registro de un usuario nuevo desde el escritorio.
    public partial class FormRegistro : Form
    {
        // Cliente de autenticación de la API (inyectado).
        private readonly IAuthApiClient _ApiClient;
        public FormRegistro(IAuthApiClient ApiClient)
        {
            InitializeComponent();
            this._ApiClient = ApiClient;
        }

        // Valida los campos obligatorios, arma el UsuarioRequest y lo registra en la API.
        private async void btnRegistrar_Click(object sender, EventArgs args)
        {
            // Validación manual: nombre, email y contraseña obligatorios.
            if (string.IsNullOrWhiteSpace(txtNombre.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Nombre, Email y Contraseña son campos obligatorios.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // Deshabilita el botón para evitar dobles clicks mientras se espera la API.
            btnRegistro.Enabled = false;
            try
            {
                // Arma el pedido con rol "Usuario".
                var requestRegistro = new UsuarioRequest()
                {
                    Apellido = txtApellido.Text,
                    Nombre = txtNombre.Text,
                    Email = txtEmail.Text,
                    Pass = txtPassword.Text,
                    Rol = "Usuario"
                };


                // POST /auth/register; si responde, cierra el formulario con OK.
                var newUsuario = await _ApiClient.RegistAsync(requestRegistro);
                if (newUsuario is not null)
                {
                    this.DialogResult = DialogResult.OK;
                }
                this.Close();
            }
            // Si la API falla, muestra el mensaje de error.
            catch (Exception ex)
            {
                MessageBox.Show($"Error al registrar el usuario: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            // Siempre vuelve a habilitar el botón.
            finally
            {
                btnRegistro.Enabled = true;
            }
        }

        // Cancela el registro y cierra el formulario.
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        // Evento de carga (vacío).
        private void FormRegistro_Load(object sender, EventArgs e)
        {

        }

        // Link "volver al login": cierra el formulario con Cancel.
        private void lnkLogin_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
