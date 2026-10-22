using Kiketpropio.Dominio.Entidades;
using Kiketpropio.Escritorio.Servicios;

namespace Kiketpropio.Escritorio.Formularios
{
    public partial class UsuariosForm : Form
    {
        public UsuariosForm()
        {
            InitializeComponent();
        }

        private async void UsuariosForm_Load(object? sender, EventArgs e)
        {
            btnNuevo.Enabled = Sesion.EsAdmin;
            btnEditar.Enabled = Sesion.EsAdmin;
            btnEliminar.Enabled = Sesion.EsAdmin;

            await CargarUsuarios();
        }

        private async Task CargarUsuarios()
        {
            try
            {
                var usuarios = await ApiClient.GetAll<Usuario>("Usuario");
                dgvUsuarios.DataSource = usuarios;
                ConfigurarColumnas();
            }
            catch (HttpRequestException)
            {
                MessageBox.Show("No se pudo conectar con la API de Kicket", "Kicket", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void ConfigurarColumnas()
        {
            if (dgvUsuarios.Columns.Count == 0) return;

            dgvUsuarios.Columns["Pass"]!.Visible = false;
            dgvUsuarios.Columns["IdUsuario"]!.HeaderText = "ID";
            dgvUsuarios.Columns["IdUsuario"]!.FillWeight = 12;
            dgvUsuarios.Columns["DNI"]!.HeaderText = "DNI";
            dgvUsuarios.Columns["DNI"]!.FillWeight = 20;
            dgvUsuarios.Columns["Nombre"]!.HeaderText = "Nombre";
            dgvUsuarios.Columns["Nombre"]!.FillWeight = 25;
            dgvUsuarios.Columns["Apellido"]!.HeaderText = "Apellido";
            dgvUsuarios.Columns["Apellido"]!.FillWeight = 25;
            dgvUsuarios.Columns["Email"]!.HeaderText = "Email";
            dgvUsuarios.Columns["Email"]!.FillWeight = 40;
            dgvUsuarios.Columns["FechaNacimiento"]!.HeaderText = "Nacimiento";
            dgvUsuarios.Columns["FechaNacimiento"]!.FillWeight = 22;
            dgvUsuarios.Columns["FechaNacimiento"]!.DefaultCellStyle.Format = "dd/MM/yyyy";
            dgvUsuarios.Columns["Rol"]!.HeaderText = "Perfil";
            dgvUsuarios.Columns["Rol"]!.FillWeight = 18;
        }

        private Usuario? GetUsuarioSeleccionado()
        {
            if (dgvUsuarios.CurrentRow is null) return null;

            return dgvUsuarios.CurrentRow.DataBoundItem as Usuario;
        }

        private async void btnNuevo_Click(object? sender, EventArgs e)
        {
            using var edicionForm = new UsuarioEdicionForm(null);
            if (edicionForm.ShowDialog() == DialogResult.OK) await CargarUsuarios();
        }

        private async void btnEditar_Click(object? sender, EventArgs e)
        {
            var usuarioSeleccionado = GetUsuarioSeleccionado();
            if (usuarioSeleccionado is null)
            {
                MessageBox.Show("Seleccione un usuario de la grilla", "Kicket", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using var edicionForm = new UsuarioEdicionForm(usuarioSeleccionado);
            if (edicionForm.ShowDialog() == DialogResult.OK) await CargarUsuarios();
        }

        private async void btnEliminar_Click(object? sender, EventArgs e)
        {
            var usuarioSeleccionado = GetUsuarioSeleccionado();
            if (usuarioSeleccionado is null)
            {
                MessageBox.Show("Seleccione un usuario de la grilla", "Kicket", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (usuarioSeleccionado.Email == Sesion.Email)
            {
                MessageBox.Show("No es posible eliminar el usuario con el que inicio sesion", "Kicket", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirmacion = MessageBox.Show($"¿Confirma la baja del usuario {usuarioSeleccionado.Apellido}, {usuarioSeleccionado.Nombre}?", "Kicket", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirmacion != DialogResult.Yes) return;

            var resultado = await ApiClient.Delete("Usuario", usuarioSeleccionado.IdUsuario);
            if (!resultado)
            {
                MessageBox.Show("No se pudo eliminar el usuario", "Kicket", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            await CargarUsuarios();
        }

        private async void btnActualizar_Click(object? sender, EventArgs e)
        {
            await CargarUsuarios();
        }
    }
}
