using System;
using System.Windows.Forms;
using Microsoft.Extensions.DependencyInjection;
using Kicket.ApiClient.Abstracciones;

namespace Kicket.WinForms.Forms
{
    // Menú principal del administrador: abre los ABM y permite cerrar sesión.
    public partial class FormPrincipal : Form
    {
        // Sesión actual (inyectada).
        private readonly ISesionUsuario _sesion;

        // Constructor: recibe la sesión por inyección de dependencias.
        public FormPrincipal(ISesionUsuario sesion)
        {
            InitializeComponent();
            _sesion = sesion;
        }

        // Al cargar, muestra el nombre del usuario logueado.
        private void FormPrincipal_Load(object sender, EventArgs e)
        {
            if (_sesion.Usuario != null)
            {
                lblBienvenida.Text = $"Bienvenido, {_sesion.Usuario.Nombre} {_sesion.Usuario.Apellido}";
            }
        }

        // --- NAVEGACIÓN A CRUDS DE ADMINISTRACIÓN ---

        // Abre el ABM de clubes como ventana modal.
        private void buttonClubes_Click(object sender, EventArgs e)
        {
            using var formClub = Program.ServiceProvider.GetRequiredService<FormClub>();
            formClub.ShowDialog(this);
        }

        // Abre el ABM de estadios.
        private void btnEstadios_Click(object sender, EventArgs e)
        {
            using var formEstadio = Program.ServiceProvider.GetRequiredService<FormEstadio>();
            formEstadio.ShowDialog(this);
        }

        // Abre el ABM de eventos.
        private void btnEventos_Click(object sender, EventArgs e)
        {
            using var formEvento = Program.ServiceProvider.GetRequiredService<FormEvento>();
            formEvento.ShowDialog(this);
        }

        // Abre el ABM de usuarios.
        private void btnUsuarios_Click(object sender, EventArgs e)
        {
            using var formUsuario = Program.ServiceProvider.GetRequiredService<FormUsuario>();
            formUsuario.ShowDialog(this);
        }

        // Abre la pantalla de compras.
        private void btnCompras_Click(object sender, EventArgs e)
        {
            using var formCompras = Program.ServiceProvider.GetRequiredService<FormCompras>();
            formCompras.ShowDialog(this);
        }

        // --- CERRAR SESIÓN ---

        // Pide confirmación, cierra la sesión y devuelve Retry para que el login se vuelva a mostrar.
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
