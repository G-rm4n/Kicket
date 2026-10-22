using Kiketpropio.Dominio.Entidades;
using Kiketpropio.Escritorio.Servicios;

namespace Kiketpropio.Escritorio.Formularios
{
    public partial class EstadioEdicionForm : Form
    {
        private readonly Estadio? _estadio;

        public EstadioEdicionForm(Estadio? estadio)
        {
            InitializeComponent();

            _estadio = estadio;
            if (_estadio is null)
            {
                lblTitulo.Text = "Nuevo estadio";
                return;
            }

            lblTitulo.Text = $"Editar estadio {_estadio.IdEstadio}";
            txtNombre.Text = _estadio.Nombre;
            txtCalle.Text = _estadio.Calle;
            nudNro.Value = _estadio.Nro;
            txtCiudad.Text = _estadio.Ciudad;
        }

        private async void btnGuardar_Click(object? sender, EventArgs e)
        {
            lblError.Text = string.Empty;

            if (string.IsNullOrWhiteSpace(txtNombre.Text) || string.IsNullOrWhiteSpace(txtCiudad.Text))
            {
                lblError.Text = "El nombre y la ciudad son obligatorios";
                return;
            }

            btnGuardar.Enabled = false;

            try
            {
                var estadioInput = new Estadio(_estadio?.IdEstadio ?? 0, txtNombre.Text.Trim(), txtCalle.Text.Trim(), (int)nudNro.Value, txtCiudad.Text.Trim());

                var resultado = _estadio is null
                    ? await ApiClient.Create("Estadio", estadioInput)
                    : await ApiClient.Update("Estadio", _estadio.IdEstadio, estadioInput);

                if (!resultado)
                {
                    lblError.Text = "No se pudo guardar el estadio";
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
