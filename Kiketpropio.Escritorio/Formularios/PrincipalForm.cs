using Kiketpropio.Escritorio.Servicios;

namespace Kiketpropio.Escritorio.Formularios
{
    public partial class PrincipalForm : Form
    {
        public PrincipalForm()
        {
            InitializeComponent();
        }

        private void PrincipalForm_Load(object? sender, EventArgs e)
        {
            lblUsuario.Text = $"Usuario: {Sesion.Nombre}";
            lblRol.Text = $"Perfil: {Sesion.Rol}";

            menuUsuarios.Visible = Sesion.EsAdmin;
            menuReportes.Visible = Sesion.PuedeVerReportes;
        }

        private void AbrirFormulario(Form formulario)
        {
            var abierto = MdiChildren.FirstOrDefault(f => f.GetType() == formulario.GetType());
            if (abierto is not null)
            {
                formulario.Dispose();
                abierto.Activate();
                return;
            }

            formulario.MdiParent = this;
            formulario.WindowState = FormWindowState.Maximized;
            formulario.Show();
        }

        private void menuClubes_Click(object? sender, EventArgs e)
        {
            AbrirFormulario(new ClubesForm());
        }

        private void menuEstadios_Click(object? sender, EventArgs e)
        {
            AbrirFormulario(new EstadiosForm());
        }

        private void menuUsuarios_Click(object? sender, EventArgs e)
        {
            AbrirFormulario(new UsuariosForm());
        }

        private void menuReporteUsuarios_Click(object? sender, EventArgs e)
        {
            AbrirFormulario(new ReporteUsuariosForm());
        }

        private void menuReporteEstadios_Click(object? sender, EventArgs e)
        {
            AbrirFormulario(new ReporteEstadiosForm());
        }

        private void menuSalir_Click(object? sender, EventArgs e)
        {
            Sesion.Limpiar();
            Application.Exit();
        }
    }
}
