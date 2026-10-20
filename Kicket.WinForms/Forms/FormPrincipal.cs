using System;
using System.Windows.Forms;
using Microsoft.Extensions.DependencyInjection;
using Kicket.ApiClient.Abstracciones;

namespace Kicket.WinForms.Forms
{
    public partial class FormPrincipal : Form
    {
        private readonly ISesionUsuario _sesion;

        public FormPrincipal(ISesionUsuario sesion)
        {
            InitializeComponent();
            _sesion = sesion;
        }

        private void FormPrincipal_Load(object sender, EventArgs e)
        {
            if (_sesion.Usuario != null)
            {
                lblBienvenida.Text = $"Bienvenido, {_sesion.Usuario.Nombre} {_sesion.Usuario.Apellido}";
            }
        }

        // --- NAVEGACIÓN A CRUDS DE ADMINISTRACIÓN ---

        private void buttonClubes_Click(object sender, EventArgs e)
        {
            using var formClub = Program.ServiceProvider.GetRequiredService<FormClub>();
            formClub.ShowDialog(this);
        }

        private void btnEstadios_Click(object sender, EventArgs e)
        {
            using var formEstadio = Program.ServiceProvider.GetRequiredService<FormEstadio>();
            formEstadio.ShowDialog(this);
        }

        private void btnEventos_Click(object sender, EventArgs e)
        {
            using var formEvento = Program.ServiceProvider.GetRequiredService<FormEvento>();
            formEvento.ShowDialog(this);
        }

        private void btnUsuarios_Click(object sender, EventArgs e)
        {
            using var formUsuario = Program.ServiceProvider.GetRequiredService<FormUsuario>();
            formUsuario.ShowDialog(this);
        }

        private void btnCompras_Click(object sender, EventArgs e)
        {
            using var formCompras = Program.ServiceProvider.GetRequiredService<FormCompras>();
            formCompras.ShowDialog(this);
        }

        // --- CERRAR SESIÓN ---

        private void BtnCerrarSesion_Click(object sender, EventArgs e)
        {
            var respuesta = MessageBox.Show(
                "¿Desea cerrar la sesión actual?",
                "Cerrar Sesión",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (respuesta == DialogResult.Yes)
            {
                _sesion.Cerrar();
                this.DialogResult = DialogResult.Retry;
                this.Close();
            }
        }
    }
}
