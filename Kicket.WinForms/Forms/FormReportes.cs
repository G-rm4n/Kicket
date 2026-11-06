using System;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Kicket.ApiClient.Abstracciones;
using Kicket.Contracts.Eventos;
using Kicket.Contracts.Reportes;
using Kicket.WinForms.Controles;

namespace Kicket.WinForms.Forms
{
    /// <summary>
    /// Reportes de ventas en el escritorio. Muestra los indicadores y las tablas, y exporta a PDF (con gráficos)
    /// o Excel. No calcula nada: usa el mismo endpoint que la web, así los números son siempre los mismos.
    /// </summary>
    [System.ComponentModel.DesignerCategory("Code")]
    public class FormReportes : Form
    {
        private static readonly CultureInfo Ar = CultureInfo.GetCultureInfo("es-AR");

        private readonly IReporteApiClient _reporteApi;
        private readonly IEventoApiClient _eventoApi;

        private readonly DateTimePicker _desde = new() { Format = DateTimePickerFormat.Short, ShowCheckBox = true, Checked = false, Width = 130 };
        private readonly DateTimePicker _hasta = new() { Format = DateTimePickerFormat.Short, ShowCheckBox = true, Checked = false, Width = 130 };
        private readonly ComboBox _evento = new() { DropDownStyle = ComboBoxStyle.DropDownList, Width = 260 };
        private readonly Button _generar = new() { Text = "Generar", AutoSize = true };
        private readonly Button _exportarPdf = new() { Text = "Exportar PDF", AutoSize = true, Enabled = false };
        private readonly Button _exportarExcel = new() { Text = "Exportar Excel", AutoSize = true, Enabled = false };
        private readonly Label _indicadores = new() { AutoSize = false, Height = 52, Dock = DockStyle.Top, Font = new Font("Segoe UI", 10F, FontStyle.Bold), ForeColor = Color.FromArgb(31, 56, 100), Padding = new Padding(8, 6, 8, 0) };
        private readonly Label _criterio = new() { AutoSize = false, Height = 36, Dock = DockStyle.Top, ForeColor = SystemColors.GrayText, Padding = new Padding(8, 0, 8, 0), Text = ReporteVentasDto.CriterioVentas };
        private readonly DataGridView _porEvento = Grilla();
        private readonly DataGridView _porClub = Grilla();
        private readonly DataGridView _porSector = Grilla();
        private readonly DataGridView _porDia = Grilla();

        // Ítem del combo de partidos (null = todos).
        private sealed record EventoItem(int? Id, string Texto)
        {
            public override string ToString() => Texto;
        }

        public FormReportes(IReporteApiClient reporteApi, IEventoApiClient eventoApi)
        {
            _reporteApi = reporteApi;
            _eventoApi = eventoApi;

            Text = "Kicket - Reportes de ventas";
            StartPosition = FormStartPosition.CenterParent;
            ClientSize = new Size(980, 640);
            MinimumSize = new Size(800, 500);
            Font = new Font("Segoe UI", 9F);

            var barra = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 44, Padding = new Padding(6, 8, 6, 0), WrapContents = false };
            barra.Controls.AddRange(new Control[]
            {
                Etiqueta("Compras desde:"), _desde, Etiqueta("hasta:"), _hasta, Etiqueta("Partido:"), _evento,
                _generar, _exportarPdf, _exportarExcel
            });

            var pestanas = new TabControl { Dock = DockStyle.Fill };
            pestanas.TabPages.Add(Pestana("Por partido", _porEvento));
            pestanas.TabPages.Add(Pestana("Por club (local)", _porClub));
            pestanas.TabPages.Add(Pestana("Por sector", _porSector));
            pestanas.TabPages.Add(Pestana("Por día", _porDia));

            var aviso = new Label
            {
                Dock = DockStyle.Bottom,
                Height = 24,
                ForeColor = SystemColors.GrayText,
                Padding = new Padding(8, 4, 0, 0),
                Text = "Los gráficos se incluyen en el PDF exportado y en la pantalla de reportes de la web."
            };

            // El orden de Add importa con Dock: lo último agregado queda más arriba.
            Controls.Add(pestanas);
            Controls.Add(aviso);
            Controls.Add(_criterio);
            Controls.Add(_indicadores);
            Controls.Add(barra);

            _generar.Click += async (_, _) => await GenerarAsync();
            _exportarPdf.Click += async (_, _) => await ExportarAsync("pdf");
            _exportarExcel.Click += async (_, _) => await ExportarAsync("xlsx");
            Load += async (_, _) => await CargarInicialAsync();
        }

        private ReporteFiltro FiltroActual() => new()
        {
            Desde = BarraFiltros.Valor(_desde),
            Hasta = BarraFiltros.Valor(_hasta),
            EventoId = (_evento.SelectedItem as EventoItem)?.Id
        };

        private async Task CargarInicialAsync()
        {
            try
            {
                var eventos = await _eventoApi.GetAllAsync();
                _evento.Items.Add(new EventoItem(null, "-- Todos los partidos --"));
                foreach (var ev in eventos.OrderByDescending(e => e.Fecha))
                    _evento.Items.Add(new EventoItem(ev.IdEvento, $"{ev.Nombre} ({ev.Fecha:dd/MM/yyyy})"));
                _evento.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los partidos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            await GenerarAsync();
        }

        // Pide el reporte a la API y llena indicadores y tablas.
        private async Task GenerarAsync()
        {
            Bloquear(true);
            try
            {
                var r = await _reporteApi.GetVentasAsync(FiltroActual());
                var res = r.Resumen;
                _indicadores.Text =
                    $"Recaudado: {M(res.TotalRecaudado)}    Entradas: {res.EntradasVendidas:N0}    Compras: {res.ComprasConfirmadas:N0}" +
                    $"    Ticket promedio: {M(res.TicketPromedio)}    Partidos con ventas: {res.EventosConVentas} de {res.EventosTotales}    Compradores: {res.CompradoresDistintos}";

                _porEvento.DataSource = r.PorEvento.Select(x => new { Partido = x.Evento, Fecha = x.Fecha.ToString("dd/MM/yyyy HH:mm"), x.Estadio, x.Compras, x.Entradas, Recaudado = M(x.Recaudado) }).ToList();
                _porClub.DataSource = r.PorClub.Select(x => new { x.Club, PartidosLocal = x.EventosComoLocal, PartidosVisitante = x.EventosComoVisitante, x.Entradas, Recaudado = M(x.Recaudado) }).ToList();
                _porSector.DataSource = r.PorSector.Select(x => new { x.Estadio, x.Sector, CapacidadPorPartido = x.CapacidadPorEvento, x.Entradas, Recaudado = M(x.Recaudado) }).ToList();
                _porDia.DataSource = r.PorPeriodo.Select(x => new { Dia = x.Dia.ToString("dd/MM/yyyy"), x.Compras, x.Entradas, Recaudado = M(x.Recaudado) }).ToList();
                _exportarPdf.Enabled = _exportarExcel.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al generar el reporte: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Bloquear(false);
            }
        }

        // Descarga el archivo de la API, lo guarda donde elija el usuario y ofrece abrirlo.
        private async Task ExportarAsync(string extension)
        {
            using var dialogo = new SaveFileDialog
            {
                FileName = $"kicket-ventas-{DateTime.Now:yyyyMMdd-HHmm}.{extension}",
                Filter = extension == "pdf" ? "Documento PDF (*.pdf)|*.pdf" : "Libro de Excel (*.xlsx)|*.xlsx"
            };
            if (dialogo.ShowDialog(this) != DialogResult.OK) return;

            Bloquear(true);
            try
            {
                var filtro = FiltroActual();
                byte[] archivo = extension == "pdf"
                    ? await _reporteApi.DescargarPdfAsync(filtro)
                    : await _reporteApi.DescargarExcelAsync(filtro);
                await File.WriteAllBytesAsync(dialogo.FileName, archivo);

                var abrir = MessageBox.Show($"Archivo guardado en:\n{dialogo.FileName}\n\n¿Querés abrirlo?", "Exportación", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (abrir == DialogResult.Yes)
                    Process.Start(new ProcessStartInfo(dialogo.FileName) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al exportar: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Bloquear(false);
            }
        }

        private void Bloquear(bool bloquear)
        {
            UseWaitCursor = bloquear;
            _generar.Enabled = !bloquear;
            if (bloquear) _exportarPdf.Enabled = _exportarExcel.Enabled = false;
            else _exportarPdf.Enabled = _exportarExcel.Enabled = _porEvento.DataSource is not null;
        }

        private static string M(decimal valor) => "$ " + valor.ToString("N2", Ar);

        private static Label Etiqueta(string texto) => new() { Text = texto, AutoSize = true, Padding = new Padding(0, 5, 0, 0) };

        private static DataGridView Grilla() => new()
        {
            Dock = DockStyle.Fill,
            ReadOnly = true,
            AllowUserToAddRows = false,
            AllowUserToDeleteRows = false,
            RowHeadersVisible = false,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            BackgroundColor = SystemColors.Window
        };

        private static TabPage Pestana(string titulo, Control contenido)
        {
            var pagina = new TabPage(titulo);
            pagina.Controls.Add(contenido);
            return pagina;
        }
    }
}
