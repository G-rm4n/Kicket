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
    public partial class FormRegistro : Form
    {
        private readonly IUsuarioApiClient _usuarioApiClient;
        public FormRegistro(IUsuarioApiClient usuarioApiClient)
        {
            InitializeComponent();
            this._usuarioApiClient = usuarioApiClient;
        }

        private async void btnRegistrar_Click(object sender, EventArgs args)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Nombre, Email y Contraseña son campos obligatorios.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            btnRegistro.Enabled = false;
            try
            {
                var requestRegistro = new UsuarioRequest()
                {
                    Apellido = txtApellido.Text,
                    Nombre = txtNombre.Text,
                    Email = txtEmail.Text,
                    Pass = txtPassword.Text,/*Posteriormente encriptar*/
                    Rol = "Usuario"
                };


                var newUsuario = await _usuarioApiClient.CreateAsync(requestRegistro);
                if (newUsuario is not null)
                {
                    this.DialogResult = DialogResult.OK;
                }
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al registrar el usuario: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnRegistro.Enabled = true;
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void FormRegistro_Load(object sender, EventArgs e)
        {

        }

        private void lnkLogin_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
