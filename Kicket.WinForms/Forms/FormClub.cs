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
using Kicket.Contracts.Clubes;

namespace Kicket.WinForms.Forms
{
    public partial class FormClub : Form
    {
        private readonly IClubApiClient _clubApiClient;
        private int? _clubIdSeleccionado = null; // Variable para almacenar el ID del club seleccionado
        public FormClub(IClubApiClient clubApiClient)
        {
            InitializeComponent();
            _clubApiClient = clubApiClient;
        }

        private async void FormClub_Load(object sender, EventArgs e)
        {
            dataGridClubes.AutoGenerateColumns = false;
            await CargarClubes();
        }

        private async Task CargarClubes()
        {
            try
            {
                var clubes = await _clubApiClient.GetAllAsync();
                dataGridClubes.DataSource = clubes;
                LimpiarFormulario();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los clubes: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
        }

        private void LimpiarFormulario()
        {
            textBoxNombreClub.Clear();
            textBoxAbreviatura.Clear();
            _clubIdSeleccionado = null; // "Olvidamos" el club seleccionado
        }

        private void dataGridClubes_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Aseguramos que no sea el encabezado
            {
                var fila = dataGridClubes.Rows[e.RowIndex];
                textBoxNombreClub.Text = fila.Cells["Nombre"].Value?.ToString();
                textBoxAbreviatura.Text = fila.Cells["Abreviatura"].Value?.ToString();
            }
        }

        private async void btnGuardar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxNombreClub.Text)) return;

            var nuevoClub = new ClubRequest
            {
                Nombre = textBoxNombreClub.Text,
                Abreviatura = textBoxAbreviatura.Text
            };

            await _clubApiClient.CreateAsync(nuevoClub);
            await CargarClubes(); // Refresca la grilla
        }

        private async void btnModificar_Click(object sender, EventArgs e)
        {
            if (_clubIdSeleccionado == null)
            {
                MessageBox.Show("Seleccione un club de la lista primero.", "Aviso");
                return;
            }

            // Armamos el paquete incluyendo el ID seleccionado
            var clubModificado = new ClubUpdateRequest
            {
                IdClub = _clubIdSeleccionado.Value, // <-- Agregamos el ID aquí adentro
                Nombre = textBoxNombreClub.Text,
                Abreviatura = textBoxAbreviatura.Text
            };

            // Le pasamos UN SOLO argumento al método (el paquete completo)
            await _clubApiClient.UpdateAsync(clubModificado);

            await CargarClubes();
            LimpiarFormulario(); // Opcional: limpiamos las cajas de texto tras modificar
        }

        private async void btnEliminar_Click(object sender, EventArgs e)
        {
            if (_clubIdSeleccionado == null) return;

            var respuesta = MessageBox.Show("¿Estás seguro de eliminar este club?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (respuesta == DialogResult.Yes)
            {
                await _clubApiClient.DeleteAsync(_clubIdSeleccionado.Value);
                await CargarClubes();
            }
        }
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void FormClub_Load_1(object sender, EventArgs e)
        {

        }

        private void dataGridClubes_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void buttonGuardar_Click(object sender, EventArgs e)
        {

        }

        private void dataGridClubes_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
