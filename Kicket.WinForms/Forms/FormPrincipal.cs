using System;
using System.Windows.Forms;
using Microsoft.Extensions.DependencyInjection;
using Kicket.ApiClient.Abstracciones;

namespace Kicket.WinForms.Forms
{
    // Menú principal del administrador: un MenuStrip "Administración" abre los ABM y "Sesión" permite cerrar sesión.
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

        // Al cargar, muestra el nombre del usuario logueado en el saludo y en el menú Sesión.
        private void FormPrincipal_Load(object sender, EventArgs e)
        {
            if (_sesion.Usuario != null)
            {
                lblBienvenida.Text = $"Bienvenido, {_sesion.Usuario.Nombre} {_sesion.Usuario.Apellido}";
                menuUsuarioActual.Text = $"{_sesion.Usuario.Email} ({_sesion.Usuario.Rol})";
            }
        }

        // --- NAVEGACIÓN A CRUDS DE ADMINISTRACIÓN ---

        // Abre el ABM de clubes como ventana modal.
        private void buttonClubes_Click(object sender, EventArgs e) => Abrir<FormClub>();

        // Abre el ABM de estadios (y desde ahí, sus sectores).
        private void btnEstadios_Click(object sender, EventArgs e) => Abrir<FormEstadio>();

        // Abre el ABM de eventos.
        private void btnEventos_Click(object sender, EventArgs e) => Abrir<FormEvento>();

        // Abre el ABM de usuarios.
        private void btnUsuarios_Click(object sender, EventArgs e) => Abrir<FormUsuario>();

        // Abre la pantalla de compras.
        private void btnCompras_Click(object sender, EventArgs e) => Abrir<FormCompras>();

        // Abre los reportes de ventas.
        private void btnReportes_Click(object sender, EventArgs e) => Abrir<FormReportes>();

        // Abre un formulario como diálogo modal (una sola ventana a la vez, se libera al cerrarse).
        // Antes controla que la sesión siga vigente: si el token venció o fue cerrado, vuelve al login.
        private void Abrir<TFormulario>() where TFormulario : Form
        {
            if (!_sesion.EstaAutenticado)
            {
                MessageBox.Show("Tu sesión venció. Volvé a iniciar sesión.", "Sesión", MessageBoxButtons.OK, MessageBoxIcon.Information);
                FinalizarSesion();
                return;
            }

            using var formulario = Program.ServiceProvider.GetRequiredService<TFormulario>();
            formulario.ShowDialog(this);

            // Si durante el uso la API respondió 401, AuthTokenHandler ya cerró la sesión local.
            if (!_sesion.EstaAutenticado)
            {
                MessageBox.Show("La sesión terminó. Volvé a iniciar sesión.", "Sesión", MessageBoxButtons.OK, MessageBoxIcon.Information);
                FinalizarSesion();
            }
        }

        // --- CERRAR SESIÓN ---

        // Pide confirmación y cierra la sesión.
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
                FinalizarSesion();
            }
        }

        // Borra token y usuario de la sesión (las siguientes requests salen sin credenciales y la API las rechaza)
        // y devuelve Retry para que FormLogin se vuelva a mostrar. No queda ninguna otra ventana abierta:
        // todas las pantallas se abren como diálogos modales de este formulario.
        private void FinalizarSesion()
        {
            _sesion.Cerrar();
            DialogResult = DialogResult.Retry;
            Close();
        }
    }
}
