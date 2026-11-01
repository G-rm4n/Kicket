using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Kicket.ApiClient.Abstracciones;
using Kicket.Contracts.Compras;
using Kicket.Contracts.Eventos;
using Kicket.Contracts.Sectores;
using Kicket.Contracts.Usuarios;
using Microsoft.Extensions.DependencyInjection;

namespace Kicket.WinForms.Forms
{
    // Pantalla de compras del administrador: lista todas las compras con filtros, muestra sus entradas,
    // permite ver el QR de cada entrada y anular (borrar) una compra.
    public partial class FormCompras : Form
    {
        // Clientes HTTP necesarios (inyectados).
        private readonly ICompraApiClient _compraApiClient;
        private readonly IEventoApiClient _eventoApiClient;
        private readonly IUsuarioApiClient _usuarioApiClient;
        private readonly ISectorApiClient _sectorApiClient;

        // Todas las compras traídas de la API y diccionarios (por id) para mostrar nombres.
        private IReadOnlyList<CompraDto> _todasLasCompras = Array.Empty<CompraDto>();
        private Dictionary<int, EventoDto> _eventosMap = new();
        private Dictionary<int, UsuarioDto> _usuariosMap = new();
        private Dictionary<int, SectorDto> _sectoresMap = new();
        // Id de la compra seleccionada en la grilla superior.
        private int? _compraIdSeleccionada = null;

        // Ítem del combo de filtro por evento (IdEvento null = "Todos").
        private sealed class EventoFiltroItem
        {
            public int? IdEvento { get; set; }
            public string Descripcion { get; set; } = string.Empty;
        }

        // Fila "lista para mostrar" de la grilla de compras (textos ya formateados).
        private sealed class CompraFilaVista
        {
            public int CompraId { get; set; }
            public string FechaTexto { get; set; } = string.Empty;
            public string UsuarioTexto { get; set; } = string.Empty;
            public string EventoTexto { get; set; } = string.Empty;
            public string SectorTexto { get; set; } = string.Empty;
            public int Cantidad { get; set; }
            public string MontoTotalTexto { get; set; } = string.Empty;
            public string MetodoPago { get; set; } = string.Empty;
            public string PaymentId { get; set; } = string.Empty;
            public string EstadoPago { get; set; } = string.Empty;
        }

        // Fila "lista para mostrar" de la grilla de entradas.
        private sealed class EntradaFilaVista
        {
            public int EntradaId { get; set; }
            public string EventoNombre { get; set; } = string.Empty;
            public string SectorNombre { get; set; } = string.Empty;
            public string TipoSector { get; set; } = string.Empty;
            public string Ubicacion { get; set; } = string.Empty;
            public string CodigoQr { get; set; } = string.Empty;
        }

        // Constructor: recibe los clientes HTTP por inyección.
        public FormCompras(
            ICompraApiClient compraApiClient,
            IEventoApiClient eventoApiClient,
            IUsuarioApiClient usuarioApiClient,
            ISectorApiClient sectorApiClient)
        {
            InitializeComponent();
            _compraApiClient = compraApiClient;
            _eventoApiClient = eventoApiClient;
            _usuarioApiClient = usuarioApiClient;
            _sectorApiClient = sectorApiClient;
        }

        // Al cargar: configura las grillas, pone el rango de fechas del último mes y carga los datos.
        private async void FormCompras_Load(object sender, EventArgs e)
        {
            dataGridViewCompras.AutoGenerateColumns = false;
            dataGridViewEntradas.AutoGenerateColumns = false;
            dateTimePickerDesde.Value = DateTime.Today.AddMonths(-1);
            dateTimePickerHasta.Value = DateTime.Today;

            await CargarDatosCompletosAsync();
        }

        // Trae eventos, usuarios, sectores y compras; arma los diccionarios y el combo de eventos, y muestra la grilla.
        private async Task CargarDatosCompletosAsync()
        {
            try
            {
                var eventos = await _eventoApiClient.GetAllAsync();
                var usuarios = await _usuarioApiClient.GetAllAsync();
                var sectores = await _sectorApiClient.GetAllAsync();
                _todasLasCompras = await _compraApiClient.GetAllAsync();

                _eventosMap = eventos.ToDictionary(ev => ev.IdEvento);
                _usuariosMap = usuarios.ToDictionary(u => u.IdUsuario);
                _sectoresMap = sectores.ToDictionary(s => s.SectorId);

                // El combo arranca con la opción "Todos los Eventos" y después un ítem por evento.
                var itemsFiltro = new List<EventoFiltroItem>
                {
                    new EventoFiltroItem { IdEvento = null, Descripcion = "-- Todos los Eventos --" }
                };
                itemsFiltro.AddRange(eventos.Select(ev => new EventoFiltroItem
                {
                    IdEvento = ev.IdEvento,
                    Descripcion = $"{ev.Nombre} ({ev.Fecha:dd/MM/yyyy})"
                }));

                comboBoxFiltroEvento.DataSource = itemsFiltro;
                comboBoxFiltroEvento.DisplayMember = nameof(EventoFiltroItem.Descripcion);
                comboBoxFiltroEvento.ValueMember = nameof(EventoFiltroItem.IdEvento);

                AplicarFiltrosYMostrar();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar las compras: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Habilita o deshabilita los selectores de fecha según el checkbox.
        private void checkBoxFiltrarFecha_CheckedChanged(object sender, EventArgs e)
        {
            dateTimePickerDesde.Enabled = checkBoxFiltrarFecha.Checked;
            dateTimePickerHasta.Enabled = checkBoxFiltrarFecha.Checked;
        }

        // Aplica los filtros elegidos.
        private void btnFiltrar_Click(object sender, EventArgs e)
        {
            AplicarFiltrosYMostrar();
        }

        // Quita los filtros y recarga todo desde la API.
        private async void btnRefrescar_Click(object sender, EventArgs e)
        {
            checkBoxFiltrarFecha.Checked = false;
            if (comboBoxFiltroEvento.Items.Count > 0)
            {
                comboBoxFiltroEvento.SelectedIndex = 0;
            }
            await CargarDatosCompletosAsync();
        }

        // Filtra las compras en memoria (por evento y/o rango de fechas) y las muestra en la grilla con el resumen.
        private void AplicarFiltrosYMostrar()
        {
            IEnumerable<CompraDto> filtradas = _todasLasCompras;

            // Filtro por evento: compras que tengan alguna entrada de ese evento.
            if (comboBoxFiltroEvento.SelectedItem is EventoFiltroItem itemEvento && itemEvento.IdEvento.HasValue)
            {
                int idEventoBuscado = itemEvento.IdEvento.Value;
                filtradas = filtradas.Where(c => c.Entradas.Any(ent => ent.EventoId == idEventoBuscado));
            }

            // Filtro por fecha: desde el inicio del día "desde" hasta el final del día "hasta".
            if (checkBoxFiltrarFecha.Checked)
            {
                DateTime desde = dateTimePickerDesde.Value.Date;
                DateTime hasta = dateTimePickerHasta.Value.Date.AddDays(1).AddTicks(-1);
                filtradas = filtradas.Where(c => c.FechaCompra >= desde && c.FechaCompra <= hasta);
            }

            var listaFiltrada = filtradas.OrderByDescending(c => c.FechaCompra).ToList();

            // Convierte cada compra en una fila con textos legibles (evento y sector de la primera entrada, usuario, monto...).
            var filasCompras = listaFiltrada.Select(c =>
            {
                var primeraEntrada = c.Entradas.FirstOrDefault();
                string nombreEvento = "Sin detalle";
                string nombreSector = "Sin detalle";

                if (primeraEntrada is not null)
                {
                    nombreEvento = _eventosMap.TryGetValue(primeraEntrada.EventoId, out var ev)
                        ? ev.Nombre
                        : $"Evento #{primeraEntrada.EventoId}";

                    nombreSector = _sectoresMap.TryGetValue(primeraEntrada.SectorId, out var sec)
                        ? sec.Nombre
                        : $"Sector #{primeraEntrada.SectorId}";
                }

                string usuarioTexto = _usuariosMap.TryGetValue(c.UsuarioId, out var usr)
                    ? $"{usr.Nombre} {usr.Apellido} ({usr.Email})"
                    : $"Usuario #{c.UsuarioId}";

                return new CompraFilaVista
                {
                    CompraId = c.CompraId,
                    FechaTexto = c.FechaCompra.ToString("dd/MM/yyyy HH:mm"),
                    UsuarioTexto = usuarioTexto,
                    EventoTexto = nombreEvento,
                    SectorTexto = nombreSector,
                    Cantidad = c.Cantidad,
                    MontoTotalTexto = $"${c.MontoTotal:N2}",
                    MetodoPago = string.IsNullOrWhiteSpace(c.MetodoPago) ? "No especificado" : c.MetodoPago,
                    PaymentId = string.IsNullOrWhiteSpace(c.PaymentId) ? "-" : c.PaymentId,
                    EstadoPago = string.IsNullOrWhiteSpace(c.EstadoPago) ? "Aprobado" : c.EstadoPago
                };
            }).ToList();

            // Muestra las compras y limpia la grilla de entradas y la selección.
            dataGridViewCompras.DataSource = filasCompras;
            dataGridViewEntradas.DataSource = null;
            _compraIdSeleccionada = null;

            // Resumen: cantidad de compras, entradas vendidas y total recaudado.
            decimal totalRecaudado = listaFiltrada.Sum(c => c.MontoTotal);
            int totalEntradas = listaFiltrada.Sum(c => c.Cantidad);
            labelResumen.Text = $"Compras: {listaFiltrada.Count} | Entradas Vendidas: {totalEntradas} | Total Recaudado: ${totalRecaudado:N2}";

            // Si hay compras, muestra las entradas de la primera.
            if (filasCompras.Count > 0)
            {
                MostrarEntradasDeCompra(filasCompras[0].CompraId);
            }
        }

        // Al hacer click en una compra, muestra sus entradas.
        private void dataGridViewCompras_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var fila = dataGridViewCompras.Rows[e.RowIndex];
            int compraId = Convert.ToInt32(fila.Cells[0].Value);
            MostrarEntradasDeCompra(compraId);
        }

        // Busca la compra y llena la grilla de entradas con sus datos legibles.
        private void MostrarEntradasDeCompra(int compraId)
        {
            _compraIdSeleccionada = compraId;
            var compra = _todasLasCompras.FirstOrDefault(c => c.CompraId == compraId);
            if (compra is null)
            {
                dataGridViewEntradas.DataSource = null;
                return;
            }

            var filasEntradas = compra.Entradas.Select(ent =>
            {
                string nombreEvento = _eventosMap.TryGetValue(ent.EventoId, out var ev)
                    ? ev.Nombre
                    : $"Evento #{ent.EventoId}";

                string nombreSector = _sectoresMap.TryGetValue(ent.SectorId, out var sec)
                    ? sec.Nombre
                    : $"Sector #{ent.SectorId}";

                bool esPlatea = nombreSector.Contains("Platea", StringComparison.OrdinalIgnoreCase);

                return new EntradaFilaVista
                {
                    EntradaId = ent.EntradaId,
                    EventoNombre = nombreEvento,
                    SectorNombre = nombreSector,
                    TipoSector = esPlatea ? "Platea (Numerada)" : "Popular (General)",
                    Ubicacion = string.IsNullOrWhiteSpace(ent.FilaAsiento)
                        ? "Sin numeración (Popular)"
                        : ent.FilaAsiento,
                    CodigoQr = ent.CodigoQr
                };
            }).ToList();

            dataGridViewEntradas.DataSource = filasEntradas;
        }

        // Doble click en una entrada: muestra su QR.
        private void dataGridViewEntradas_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            MostrarQrDeFila(e.RowIndex);
        }

        // Botón "Ver QR": muestra el QR de la entrada seleccionada.
        private void btnVerQrEntrada_Click(object sender, EventArgs e)
        {
            if (dataGridViewEntradas.CurrentRow is null || dataGridViewEntradas.CurrentRow.Index < 0)
            {
                MessageBox.Show("Seleccione una entrada de la grilla inferior para visualizar su código QR.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            MostrarQrDeFila(dataGridViewEntradas.CurrentRow.Index);
        }

        // Abre el formulario del ticket QR con los datos de la entrada de esa fila.
        private void MostrarQrDeFila(int rowIndex)
        {
            if (dataGridViewEntradas.DataSource is not List<EntradaFilaVista> entradas || rowIndex >= entradas.Count)
            {
                return;
            }

            var entrada = entradas[rowIndex];
            using var formQr = Program.ServiceProvider.GetRequiredService<FormTicketQr>();
            formQr.CargarDatos(entrada.EntradaId, entrada.EventoNombre, entrada.SectorNombre, entrada.Ubicacion, entrada.CodigoQr);
            formQr.ShowDialog(this);
        }

        // Anula la compra seleccionada (DELETE /compras/{id}), previa confirmación, y recarga los datos.
        private async void btnAnularCompra_Click(object sender, EventArgs e)
        {
            if (_compraIdSeleccionada is null)
            {
                MessageBox.Show("Seleccione una compra de la grilla superior para anularla.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirmacion = MessageBox.Show(
                $"¿Está seguro de anular y eliminar la compra #{_compraIdSeleccionada.Value}?\nSe liberarán las entradas asociadas.",
                "Confirmar Anulación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirmacion == DialogResult.Yes)
            {
                try
                {
                    await _compraApiClient.DeleteAsync(_compraIdSeleccionada.Value);
                    MessageBox.Show("Compra anulada correctamente. El stock del sector fue restituido.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    await CargarDatosCompletosAsync();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al anular la compra: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
