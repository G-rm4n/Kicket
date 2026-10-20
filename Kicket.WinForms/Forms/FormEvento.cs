using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Kicket.ApiClient.Abstracciones;
using Kicket.Contracts.Clubes;
using Kicket.Contracts.Estadios;
using Kicket.Contracts.Eventos;

namespace Kicket.WinForms.Forms
{
    public partial class FormEvento : Form
    {
        private readonly IEventoApiClient _eventoApiClient;
        private readonly IClubApiClient _clubApiClient;
        private readonly IEstadioApiClient _estadioApiClient;

        private IReadOnlyList<ClubDto> _clubes = Array.Empty<ClubDto>();
        private IReadOnlyList<EstadioDto> _estadios = Array.Empty<EstadioDto>();
        private IReadOnlyList<EventoDto> _eventos = Array.Empty<EventoDto>();
        private int? _eventoIdSeleccionado = null;

        private sealed class EventoFilaVista
        {
            public int IdEvento { get; set; }
            public string Nombre { get; set; } = string.Empty;
            public string FechaTexto { get; set; } = string.Empty;
            public string EstadioNombre { get; set; } = string.Empty;
            public string ClubLocalNombre { get; set; } = string.Empty;
            public string ClubVisitanteNombre { get; set; } = string.Empty;
            public string EstadoTexto { get; set; } = string.Empty;
        }

        public FormEvento(
            IEventoApiClient eventoApiClient,
            IClubApiClient clubApiClient,
            IEstadioApiClient estadioApiClient)
        {
            InitializeComponent();
            _eventoApiClient = eventoApiClient;
            _clubApiClient = clubApiClient;
            _estadioApiClient = estadioApiClient;
        }

        private async void FormEvento_Load(object sender, EventArgs e)
        {
            dataGridViewEventos.AutoGenerateColumns = false;
            dateTimePickerFecha.Value = DateTime.Now.AddDays(7);
            await CargarCatalogosYEventosAsync();
        }

        private async Task CargarCatalogosYEventosAsync()
        {
            try
            {
                _estadios = await _estadioApiClient.GetAllAsync();
                _clubes = await _clubApiClient.GetAllAsync();

                comboBoxEstadio.DataSource = _estadios.ToList();
                comboBoxEstadio.DisplayMember = nameof(EstadioDto.Nombre);
                comboBoxEstadio.ValueMember = nameof(EstadioDto.IdEstadio);

                // Dos listas independientes para que ambos ComboBox no compartan el mismo CurrencyManager
                comboBoxClubLocal.DataSource = _clubes.ToList();
                comboBoxClubLocal.DisplayMember = nameof(ClubDto.Nombre);
                comboBoxClubLocal.ValueMember = nameof(ClubDto.ClubId);

                comboBoxClubVisitante.DataSource = _clubes.ToList();
                comboBoxClubVisitante.DisplayMember = nameof(ClubDto.Nombre);
                comboBoxClubVisitante.ValueMember = nameof(ClubDto.ClubId);

                await CargarEventosAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los datos iniciales: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task CargarEventosAsync()
        {
            try
            {
                _eventos = await _eventoApiClient.GetAllAsync();

                var estadiosMap = _estadios.ToDictionary(e => e.IdEstadio, e => $"{e.Nombre} ({e.Ciudad})");
                var clubesMap = _clubes.ToDictionary(c => c.ClubId, c => $"{c.Nombre} ({c.Abreviatura})");

                var filas = _eventos
                    .OrderByDescending(e => e.Fecha)
                    .Select(e => new EventoFilaVista
                    {
                        IdEvento = e.IdEvento,
                        Nombre = e.Nombre,
                        FechaTexto = e.Fecha.ToString("dd/MM/yyyy HH:mm"),
                        EstadioNombre = estadiosMap.TryGetValue(e.IdEstadio, out var estNombre) ? estNombre : $"ID {e.IdEstadio}",
                        ClubLocalNombre = clubesMap.TryGetValue(e.IdEquipoLocal, out var locNombre) ? locNombre : $"ID {e.IdEquipoLocal}",
                        ClubVisitanteNombre = clubesMap.TryGetValue(e.IdEquipoVisitante, out var visNombre) ? visNombre : $"ID {e.IdEquipoVisitante}",
                        EstadoTexto = e.EstaCancelado
                            ? "Cancelado"
                            : (e.Fecha > DateTime.Now ? "Disponible" : "Finalizado")
                    })
                    .ToList();

                dataGridViewEventos.DataSource = filas;
                LimpiarFormulario();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los eventos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridViewEventos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var fila = dataGridViewEventos.Rows[e.RowIndex];
            int idEvento = Convert.ToInt32(fila.Cells[0].Value);
            var evento = _eventos.FirstOrDefault(ev => ev.IdEvento == idEvento);
            if (evento is null) return;

            _eventoIdSeleccionado = evento.IdEvento;
            textBoxNombre.Text = evento.Nombre;
            dateTimePickerFecha.Value = evento.Fecha;
            comboBoxEstadio.SelectedValue = evento.IdEstadio;
            comboBoxClubLocal.SelectedValue = evento.IdEquipoLocal;
            comboBoxClubVisitante.SelectedValue = evento.IdEquipoVisitante;
            checkBoxCancelado.Checked = evento.EstaCancelado;
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
        }

        private void LimpiarFormulario()
        {
            _eventoIdSeleccionado = null;
            textBoxNombre.Clear();
            dateTimePickerFecha.Value = DateTime.Now.AddDays(7);
            checkBoxCancelado.Checked = false;
            if (comboBoxEstadio.Items.Count > 0) comboBoxEstadio.SelectedIndex = 0;
            if (comboBoxClubLocal.Items.Count > 0) comboBoxClubLocal.SelectedIndex = 0;
            if (comboBoxClubVisitante.Items.Count > 1) comboBoxClubVisitante.SelectedIndex = 1;
        }

        private bool ValidarSeleccion(out int estadioId, out int clubLocalId, out int clubVisitanteId)
        {
            estadioId = 0;
            clubLocalId = 0;
            clubVisitanteId = 0;

            if (string.IsNullOrWhiteSpace(textBoxNombre.Text))
            {
                MessageBox.Show("El nombre del evento es obligatorio.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (comboBoxEstadio.SelectedValue is not int estId)
            {
                MessageBox.Show("Seleccione un estadio válido.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (comboBoxClubLocal.SelectedValue is not int locId)
            {
                MessageBox.Show("Seleccione un club local válido.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (comboBoxClubVisitante.SelectedValue is not int visId)
            {
                MessageBox.Show("Seleccione un club visitante válido.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (locId == visId)
            {
                MessageBox.Show("El club local y el club visitante no pueden ser el mismo.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            estadioId = estId;
            clubLocalId = locId;
            clubVisitanteId = visId;
            return true;
        }

        private async void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!ValidarSeleccion(out int estadioId, out int clubLocalId, out int clubVisitanteId))
            {
                return;
            }

            if (dateTimePickerFecha.Value <= DateTime.Now)
            {
                MessageBox.Show("La fecha del evento debe ser futura.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var request = new EventoRequest
            {
                Nombre = textBoxNombre.Text.Trim(),
                Fecha = dateTimePickerFecha.Value,
                EstadioId = estadioId,
                ClubLocalId = clubLocalId,
                ClubVisitanteId = clubVisitanteId
            };

            try
            {
                await _eventoApiClient.CreateAsync(request);
                MessageBox.Show("Evento creado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                await CargarEventosAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al crear el evento: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnModificar_Click(object sender, EventArgs e)
        {
            if (_eventoIdSeleccionado is null)
            {
                MessageBox.Show("Seleccione un evento de la lista para modificar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!ValidarSeleccion(out int estadioId, out int clubLocalId, out int clubVisitanteId))
            {
                return;
            }

            var request = new EventoUpdateRequest
            {
                IdEvento = _eventoIdSeleccionado.Value,
                Nombre = textBoxNombre.Text.Trim(),
                Fecha = dateTimePickerFecha.Value,
                EstadioId = estadioId,
                ClubLocalId = clubLocalId,
                ClubVisitanteId = clubVisitanteId,
                EstaCancelado = checkBoxCancelado.Checked
            };

            try
            {
                await _eventoApiClient.UpdateAsync(request);
                MessageBox.Show("Evento actualizado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                await CargarEventosAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al modificar el evento: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnEliminar_Click(object sender, EventArgs e)
        {
            if (_eventoIdSeleccionado is null)
            {
                MessageBox.Show("Seleccione un evento de la lista para eliminar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirmacion = MessageBox.Show(
                "¿Está seguro de eliminar este evento?",
                "Confirmar eliminación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirmacion == DialogResult.Yes)
            {
                try
                {
                    await _eventoApiClient.DeleteAsync(_eventoIdSeleccionado.Value);
                    MessageBox.Show("Evento eliminado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    await CargarEventosAsync();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al eliminar el evento: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private async void btnVerDisponibilidad_Click(object sender, EventArgs e)
        {
            if (_eventoIdSeleccionado is null)
            {
                MessageBox.Show("Seleccione un evento de la grilla para consultar su disponibilidad por sector.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                var disponibilidad = await _eventoApiClient.GetDisponibilidadAsync(_eventoIdSeleccionado.Value);
                if (disponibilidad.Count == 0)
                {
                    MessageBox.Show("El estadio asignado a este evento aún no tiene sectores configurados.", "Disponibilidad", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                var sb = new StringBuilder();
                sb.AppendLine($"Disponibilidad para '{textBoxNombre.Text}':");
                sb.AppendLine(new string('-', 55));

                foreach (var sec in disponibilidad)
                {
                    string tipo = sec.EsPlatea ? "Platea (Numerada)" : "Popular (Sin numerar)";
                    sb.AppendLine($"• {sec.Nombre} [{tipo}]");
                    sb.AppendLine($"   Precio: ${sec.PrecioBase:N2} | Total: {sec.CapacidadTotal} | Vendidas: {sec.EntradasVendidas} | Disponibles: {sec.StockDisponible}");
                }

                MessageBox.Show(sb.ToString(), "Disponibilidad de Sectores", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al consultar disponibilidad: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
