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

        // --- NAVEGACIÓN A CRUDS ---

        private void buttonClubes_Click(object sender, EventArgs e)
        {
            var formClub = Program.ServiceProvider.GetRequiredService<FormClub>();
            formClub.ShowDialog();
        }

        private void btnEstadios_Click(object sender, EventArgs e)
        {
            var formEstadio = Program.ServiceProvider.GetRequiredService<FormEstadio>();
            formEstadio.ShowDialog();
        }

        private void btnUsuarios_Click(object sender, EventArgs e)
        {
            var formUsuario = Program.ServiceProvider.GetRequiredService<FormUsuario>();
            formUsuario.ShowDialog();
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
                // Si tu ISesionUsuario implementa un método como CerrarSesion() o Limpiar(), invócalo aquí:
                // _sesion.CerrarSesion();

                this.Close(); // Cierra el menú principal para retornar el control a FormLogin
            }
        }
    }
}