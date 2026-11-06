using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Kicket.ApiClient.Abstracciones;
using Kicket.Contracts.Common;
using Kicket.Contracts.Compras;
using Kicket.Contracts.Eventos;
using Kicket.Contracts.Reportes;
using Kicket.WinForms.Controles;
using Microsoft.Extensions.DependencyInjection;

namespace Kicket.WinForms.Forms
{
    // Pantalla de compras del administrador: lista paginada de compras con filtros (resueltos en la API),
    // muestra sus entradas, permite ver el QR de cada entrada y anular (borrar) una compra.
    public partial class FormCompras : Form
    {
        // Clientes HTTP necesarios (inyectados).
        private readonly ICompraApiClient _compraApiClient;
        private readonly IEventoApiClient _eventoApiClient;
        private readonly IReporteApiClient _reporteApiClient;

        // Página actual de compras (ya trae nombres de usuario, evento y sector) e id de la compra seleccionada.
        private PagedResult<CompraDto>? _pagina;
        private int? _compraIdSeleccionada = null;

        // Controles agregados en la versión 2: búsqueda por texto y paginador.
        private readonly TextBox _textoBusqueda = new() { PlaceholderText = "Email, apellido o id de pago" };
        private readonly ControlPaginacion _paginador = new();

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

        // Fila "lista para mostrar" de la grilla de entradas. Acá (administración) sí se muestra la ubicación.
        private sealed class EntradaFilaVista
        {
            public int EntradaId { get; set; }
            public string EventoNombre { get; set; } = string.Empty;
            public string SectorNombre { get; set; } = string.Empty;
            public string TipoSector { get; set; } = string.Empty;
            public string Ubicacion { get; set; } = string.Empty;
            public string CodigoQr { get; set; } = string.Empty;
        }

        // Constructor: recibe los clientes HTTP por inyección y agrega búsqueda y paginador.
        public FormCompras(
            ICompraApiClient compraApiClient,
            IEventoApiClient eventoApiClient,
            IReporteApiClient reporteApiClient)
        {
            InitializeComponent();
            _compraApiClient = compraApiClient;
            _eventoApiClient = eventoApiClient;
            _reporteApiClient = reporteApiClient;

            // Búsqueda por texto en la segunda fila del grupo de filtros (a la izquierda de las fechas).
            groupBoxFiltros.Controls.Add(new Label { Text = "Buscar:", AutoSize = true, Location = new Point(labelFiltroEvento.Left, labelDesde.Top) });
            _textoBusqueda.Location = new Point(comboBoxFiltroEvento.Left, dateTimePickerDesde.Top);
            _textoBusqueda.Width = comboBoxFiltroEvento.Width;
            _textoBusqueda.KeyDown += async (_, e) =>
            {
                if (e.KeyCode != Keys.Enter) return;
                e.SuppressKeyPress = true;
                _paginador.Reiniciar();
                await CargarComprasAsync();
            };
            groupBoxFiltros.Controls.Add(_textoBusqueda);

            // Resumen en dos líneas para que entre completo.
            labelResumen.AutoSize = false;
            labelResumen.Size = new Size(btnVerQrEntrada.Left - labelResumen.Left - 10, btnVerQrEntrada.Height * 2);

            _paginador.PaginaCambiada += async (_, _) => await CargarComprasAsync();
            LayoutListado.Envolver(this, dataGridViewCompras, null, _paginador);
        }

        // Al cargar: configura las grillas, pone el rango de fechas del último mes y carga los datos.
        private async void FormCompras_Load(object sender, EventArgs e)
        {
            dataGridViewCompras.AutoGenerateColumns = false;
            dataGridViewEntradas.AutoGenerateColumns = false;
            dateTimePickerDesde.Value = DateTime.Today.AddMonths(-1);
            dateTimePickerHasta.Value = DateTime.Today;

            await CargarEventosAsync();
            await CargarComprasAsync();
        }

        // Llena el combo de eventos (solo para elegir el filtro).
        private async Task CargarEventosAsync()
        {
            try
            {
                var eventos = await _eventoApiClient.GetAllAsync();

                // El combo arranca con la opción "Todos los Eventos" y después un ítem por evento.
                var itemsFiltro = new List<EventoFiltroItem>
                {
                    new EventoFiltroItem { IdEvento = null, Descripcion = "-- Todos los Eventos --" }
                };
                itemsFiltro.AddRange(eventos.OrderByDescending(ev => ev.Fecha).Select(ev => new EventoFiltroItem
                {
                    IdEvento = ev.IdEvento,
                    Descripcion = $"{ev.Nombre} ({ev.Fecha:dd/MM/yyyy})"
                }));

                comboBoxFiltroEvento.DataSource = itemsFiltro;
                comboBoxFiltroEvento.DisplayMember = nameof(EventoFiltroItem.Descripcion);
                comboBoxFiltroEvento.ValueMember = nameof(EventoFiltroItem.IdEvento);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los eventos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Habilita o deshabilita los selectores de fecha según el checkbox.
        private void checkBoxFiltrarFecha_CheckedChanged(object sender, EventArgs e)
        {
            dateTimePickerDesde.Enabled = checkBoxFiltrarFecha.Checked;
            dateTimePickerHasta.Enabled = checkBoxFiltrarFecha.Checked;
        }

        // Aplica los filtros elegidos (vuelve a la página 1).
        private async void btnFiltrar_Click(object sender, EventArgs e)
        {
            _paginador.Reiniciar();
            await CargarComprasAsync();
        }

        // Quita los filtros y recarga desde la API.
        private async void btnRefrescar_Click(object sender, EventArgs e)
        {
            checkBoxFiltrarFecha.Checked = false;
            _textoBusqueda.Clear();
            if (comboBoxFiltroEvento.Items.Count > 0)
            {
                comboBoxFiltroEvento.SelectedIndex = 0;
            }
            _paginador.Reiniciar();
            await CargarComprasAsync();
        }

        // Arma el filtro con lo elegido en pantalla. Fechas = días completos.
        private CompraFiltro FiltroActual() => new()
        {
            Pagina = _paginador.Pagina,
            TamanoPagina = _paginador.TamanoPagina,
            Busqueda = _textoBusqueda.Text,
            EventoId = (comboBoxFiltroEvento.SelectedItem as EventoFiltroItem)?.IdEvento,
            Desde = checkBoxFiltrarFecha.Checked ? dateTimePickerDesde.Value.Date : null,
            Hasta = checkBoxFiltrarFecha.Checked ? dateTimePickerHasta.Value.Date : null,
            OrdenarPor = "fecha",
            Descendente = true
        };

        // Pide a la API la página de compras con los filtros (no se traen todas para filtrar en memoria)
        // y los totales del filtro desde el reporte de ventas.
        private async Task CargarComprasAsync()
        {
            _paginador.Bloquear(true);
            btnFiltrar.Enabled = btnRefrescar.Enabled = false;
            try
            {
                var filtro = FiltroActual();
                _pagina = await _compraApiClient.GetPaginadoAsync(filtro);

                // Convierte cada compra en una fila con textos legibles (los nombres ya vienen de la API).
                var filasCompras = _pagina.Items.Select(c => new CompraFilaVista
                {
                    CompraId = c.CompraId,
                    FechaTexto = c.FechaCompra.ToString("dd/MM/yyyy HH:mm"),
                    UsuarioTexto = c.UsuarioNombre is null ? $"Usuario #{c.UsuarioId}" : $"{c.UsuarioNombre} ({c.UsuarioEmail})",
                    EventoTexto = c.EventoNombre ?? "Sin detalle",
                    SectorTexto = c.SectorNombre ?? "Sin detalle",
                    Cantidad = c.Cantidad,
                    MontoTotalTexto = $"${c.MontoTotal:N2}",
                    MetodoPago = string.IsNullOrWhiteSpace(c.MetodoPago) ? "No especificado" : c.MetodoPago,
                    PaymentId = string.IsNullOrWhiteSpace(c.PaymentId) ? "-" : c.PaymentId,
                    EstadoPago = string.IsNullOrWhiteSpace(c.EstadoPago) ? "Aprobado" : c.EstadoPago
                }).ToList();

                // Muestra las compras y limpia la grilla de entradas y la selección.
                dataGridViewCompras.DataSource = filasCompras;
                dataGridViewEntradas.DataSource = null;
                _compraIdSeleccionada = null;
                _paginador.Actualizar(_pagina);

                // Resumen: cantidad de compras encontradas y totales de ventas confirmadas del mismo filtro
                // (los calcula la API con ADO.NET, no se suman solo las filas de esta página).
                var totales = await _reporteApiClient.GetVentasAsync(new ReporteFiltro { Desde = filtro.Desde, Hasta = filtro.Hasta, EventoId = filtro.EventoId });
                labelResumen.Text =
                    $"Compras encontradas: {_pagina.TotalRegistros}\n" +
                    $"Ventas confirmadas: {totales.Resumen.EntradasVendidas} entradas | ${totales.Resumen.TotalRecaudado:N2}";

                // Si hay compras, muestra las entradas de la primera.
                if (filasCompras.Count > 0)
                {
                    MostrarEntradasDeCompra(filasCompras[0].CompraId);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar las compras: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnFiltrar.Enabled = btnRefrescar.Enabled = true;
                _paginador.Bloquear(false);
                if (_pagina is not null) _paginador.Actualizar(_pagina);
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

        // Busca la compra en la página actual y llena la grilla de entradas con sus datos legibles.
        private void MostrarEntradasDeCompra(int compraId)
        {
            _compraIdSeleccionada = compraId;
            var compra = _pagina?.Items.FirstOrDefault(c => c.CompraId == compraId);
            if (compra is null)
            {
                dataGridViewEntradas.DataSource = null;
                return;
            }

            // Todas las entradas de una compra son del mismo evento y sector.
            string nombreEvento = compra.EventoNombre ?? "Sin detalle";
            string nombreSector = compra.SectorNombre ?? "Sin detalle";
            bool esPlatea = nombreSector.Contains("Platea", StringComparison.OrdinalIgnoreCase);

            var filasEntradas = compra.Entradas.OrderBy(ent => ent.EntradaId).Select(ent => new EntradaFilaVista
            {
                EntradaId = ent.EntradaId,
                EventoNombre = nombreEvento,
                SectorNombre = nombreSector,
                TipoSector = esPlatea ? "Platea (Numerada)" : "Popular (General)",
                Ubicacion = string.IsNullOrWhiteSpace(ent.FilaAsiento)
                    ? "Sin numeración (Popular)"
                    : ent.FilaAsiento,
                CodigoQr = ent.CodigoQr
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
                    await CargarComprasAsync();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al anular la compra: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
