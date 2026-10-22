using Kiketpropio.Dominio.Entidades;
using Kiketpropio.Escritorio.Servicios;

namespace Kiketpropio.Escritorio.Formularios
{
    public partial class ClubEdicionForm : Form
    {
        private readonly Club? _club;

        public ClubEdicionForm(Club? club)
        {
            InitializeComponent();

            _club = club;
            if (_club is null)
            {
                lblTitulo.Text = "Nuevo club";
                return;
            }

            lblTitulo.Text = $"Editar club {_club.IdClub}";
            txtNombre.Text = _club.Nombre;
            txtLogo.Text = _club.Logo_Url;
        }

        private async void btnGuardar_Click(object? sender, EventArgs e)
        {
            lblError.Text = string.Empty;

            if (string.IsNullOrWhiteSpace(txtNombre.Text) || string.IsNullOrWhiteSpace(txtLogo.Text))
            {
                lblError.Text = "El nombre o URL no pueden estar vacios";
                return;
            }

            btnGuardar.Enabled = false;

            try
            {
                var clubInput = new Club(_club?.IdClub ?? 0, txtNombre.Text.Trim(), txtLogo.Text.Trim());

                var resultado = _club is null
                    ? await ApiClient.Create("Club", clubInput)
                    : await ApiClient.Update("Club", _club.IdClub, clubInput);

                if (!resultado)
                {
                    lblError.Text = "No se pudo guardar el club";
                    return;
                }

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (HttpRequestException)
            {
                lblError.Text = "No se pudo conectar con la API de Kicket";
            }
            finally
            {
                btnGuardar.Enabled = true;
            }
        }

        private void btnCancelar_Click(object? sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
