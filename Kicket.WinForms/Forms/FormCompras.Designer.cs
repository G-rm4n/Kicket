using System.Drawing;
using System.Windows.Forms;

namespace Kicket.WinForms.Forms
{
    partial class FormCompras
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            labelTitulo = new Label();
            groupBoxFiltros = new GroupBox();
            labelFiltroEvento = new Label();
            comboBoxFiltroEvento = new ComboBox();
            checkBoxFiltrarFecha = new CheckBox();
            labelDesde = new Label();
            dateTimePickerDesde = new DateTimePicker();
            labelHasta = new Label();
            dateTimePickerHasta = new DateTimePicker();
            btnFiltrar = new Button();
            btnRefrescar = new Button();
            labelCompras = new Label();
            dataGridViewCompras = new DataGridView();
            ColumnCompraId = new DataGridViewTextBoxColumn();
            ColumnFecha = new DataGridViewTextBoxColumn();
            ColumnUsuario = new DataGridViewTextBoxColumn();
            ColumnEvento = new DataGridViewTextBoxColumn();
            ColumnSector = new DataGridViewTextBoxColumn();
            ColumnCantidad = new DataGridViewTextBoxColumn();
            ColumnMontoTotal = new DataGridViewTextBoxColumn();
            labelEntradas = new Label();
            dataGridViewEntradas = new DataGridView();
            ColumnEntradaId = new DataGridViewTextBoxColumn();
            ColumnEntradaEvento = new DataGridViewTextBoxColumn();
            ColumnEntradaSector = new DataGridViewTextBoxColumn();
            ColumnEntradaTipo = new DataGridViewTextBoxColumn();
            ColumnEntradaUbicacion = new DataGridViewTextBoxColumn();
            labelResumen = new Label();
            btnAnularCompra = new Button();
            groupBoxFiltros.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewCompras).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridViewEntradas).BeginInit();
            SuspendLayout();
            // 
            // labelTitulo
            // 
            labelTitulo.AutoSize = true;
            labelTitulo.Font = new Font("Segoe UI", 16F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelTitulo.Location = new Point(20, 12);
            labelTitulo.Name = "labelTitulo";
            labelTitulo.Size = new Size(420, 37);
            labelTitulo.TabIndex = 0;
            labelTitulo.Text = "Auditoría de Compras y Entradas";
            // 
            // groupBoxFiltros
            // 
            groupBoxFiltros.Controls.Add(btnRefrescar);
            groupBoxFiltros.Controls.Add(btnFiltrar);
            groupBoxFiltros.Controls.Add(dateTimePickerHasta);
            groupBoxFiltros.Controls.Add(labelHasta);
            groupBoxFiltros.Controls.Add(dateTimePickerDesde);
            groupBoxFiltros.Controls.Add(labelDesde);
            groupBoxFiltros.Controls.Add(checkBoxFiltrarFecha);
            groupBoxFiltros.Controls.Add(comboBoxFiltroEvento);
            groupBoxFiltros.Controls.Add(labelFiltroEvento);
            groupBoxFiltros.Location = new Point(20, 55);
            groupBoxFiltros.Name = "groupBoxFiltros";
            groupBoxFiltros.Size = new Size(880, 95);
            groupBoxFiltros.TabIndex = 1;
            groupBoxFiltros.TabStop = false;
            groupBoxFiltros.Text = "Filtros de Búsqueda";
            // 
            // labelFiltroEvento
            // 
            labelFiltroEvento.AutoSize = true;
            labelFiltroEvento.Location = new Point(15, 28);
            labelFiltroEvento.Name = "labelFiltroEvento";
            labelFiltroEvento.Size = new Size(60, 20);
            labelFiltroEvento.TabIndex = 0;
            labelFiltroEvento.Text = "Evento:";
            // 
            // comboBoxFiltroEvento
            // 
            comboBoxFiltroEvento.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxFiltroEvento.FormattingEnabled = true;
            comboBoxFiltroEvento.Location = new Point(80, 25);
            comboBoxFiltroEvento.Name = "comboBoxFiltroEvento";
            comboBoxFiltroEvento.Size = new Size(270, 28);
            comboBoxFiltroEvento.TabIndex = 1;
            // 
            // checkBoxFiltrarFecha
            // 
            checkBoxFiltrarFecha.AutoSize = true;
            checkBoxFiltrarFecha.Location = new Point(370, 27);
            checkBoxFiltrarFecha.Name = "checkBoxFiltrarFecha";
            checkBoxFiltrarFecha.Size = new Size(140, 24);
            checkBoxFiltrarFecha.TabIndex = 2;
            checkBoxFiltrarFecha.Text = "Filtrar por Fecha";
            checkBoxFiltrarFecha.UseVisualStyleBackColor = true;
            checkBoxFiltrarFecha.CheckedChanged += checkBoxFiltrarFecha_CheckedChanged;
            // 
            // labelDesde
            // 
            labelDesde.AutoSize = true;
            labelDesde.Location = new Point(370, 58);
            labelDesde.Name = "labelDesde";
            labelDesde.Size = new Size(53, 20);
            labelDesde.TabIndex = 3;
            labelDesde.Text = "Desde:";
            // 
            // dateTimePickerDesde
            // 
            dateTimePickerDesde.Enabled = false;
            dateTimePickerDesde.Format = DateTimePickerFormat.Short;
            dateTimePickerDesde.Location = new Point(428, 54);
            dateTimePickerDesde.Name = "dateTimePickerDesde";
            dateTimePickerDesde.Size = new Size(125, 27);
            dateTimePickerDesde.TabIndex = 4;
            // 
            // labelHasta
            // 
            labelHasta.AutoSize = true;
            labelHasta.Location = new Point(565, 58);
            labelHasta.Name = "labelHasta";
            labelHasta.Size = new Size(49, 20);
            labelHasta.TabIndex = 5;
            labelHasta.Text = "Hasta:";
            // 
            // dateTimePickerHasta
            // 
            dateTimePickerHasta.Enabled = false;
            dateTimePickerHasta.Format = DateTimePickerFormat.Short;
            dateTimePickerHasta.Location = new Point(620, 54);
            dateTimePickerHasta.Name = "dateTimePickerHasta";
            dateTimePickerHasta.Size = new Size(125, 27);
            dateTimePickerHasta.TabIndex = 6;
            // 
            // btnFiltrar
            // 
            btnFiltrar.Location = new Point(760, 20);
            btnFiltrar.Name = "btnFiltrar";
            btnFiltrar.Size = new Size(105, 30);
            btnFiltrar.TabIndex = 7;
            btnFiltrar.Text = "Filtrar";
            btnFiltrar.UseVisualStyleBackColor = true;
            btnFiltrar.Click += btnFiltrar_Click;
            // 
            // btnRefrescar
            // 
            btnRefrescar.Location = new Point(760, 54);
            btnRefrescar.Name = "btnRefrescar";
            btnRefrescar.Size = new Size(105, 30);
            btnRefrescar.TabIndex = 8;
            btnRefrescar.Text = "Ver Todas";
            btnRefrescar.UseVisualStyleBackColor = true;
            btnRefrescar.Click += btnRefrescar_Click;
            // 
            // labelCompras
            // 
            labelCompras.AutoSize = true;
            labelCompras.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelCompras.Location = new Point(20, 160);
            labelCompras.Name = "labelCompras";
            labelCompras.Size = new Size(185, 23);
            labelCompras.TabIndex = 2;
            labelCompras.Text = "Compras Realizadas:";
            // 
            // dataGridViewCompras
            // 
            dataGridViewCompras.AllowUserToAddRows = false;
            dataGridViewCompras.AllowUserToDeleteRows = false;
            dataGridViewCompras.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCompras.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCompras.Columns.AddRange(new DataGridViewColumn[]
            {
                ColumnCompraId,
                ColumnFecha,
                ColumnUsuario,
                ColumnEvento,
                ColumnSector,
                ColumnCantidad,
                ColumnMontoTotal
            });
            dataGridViewCompras.Location = new Point(20, 187);
            dataGridViewCompras.MultiSelect = false;
            dataGridViewCompras.Name = "dataGridViewCompras";
            dataGridViewCompras.ReadOnly = true;
            dataGridViewCompras.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewCompras.Size = new Size(880, 220);
            dataGridViewCompras.TabIndex = 3;
            dataGridViewCompras.CellClick += dataGridViewCompras_CellClick;
            // 
            // ColumnCompraId
            // 
            ColumnCompraId.DataPropertyName = "CompraId";
            ColumnCompraId.FillWeight = 45F;
            ColumnCompraId.HeaderText = "ID";
            ColumnCompraId.Name = "ColumnCompraId";
            ColumnCompraId.ReadOnly = true;
            // 
            // ColumnFecha
            // 
            ColumnFecha.DataPropertyName = "FechaTexto";
            ColumnFecha.FillWeight = 95F;
            ColumnFecha.HeaderText = "Fecha Compra";
            ColumnFecha.Name = "ColumnFecha";
            ColumnFecha.ReadOnly = true;
            // 
            // ColumnUsuario
            // 
            ColumnUsuario.DataPropertyName = "UsuarioTexto";
            ColumnUsuario.FillWeight = 130F;
            ColumnUsuario.HeaderText = "Cliente";
            ColumnUsuario.Name = "ColumnUsuario";
            ColumnUsuario.ReadOnly = true;
            // 
            // ColumnEvento
            // 
            ColumnEvento.DataPropertyName = "EventoTexto";
            ColumnEvento.FillWeight = 125F;
            ColumnEvento.HeaderText = "Evento";
            ColumnEvento.Name = "ColumnEvento";
            ColumnEvento.ReadOnly = true;
            // 
            // ColumnSector
            // 
            ColumnSector.DataPropertyName = "SectorTexto";
            ColumnSector.FillWeight = 100F;
            ColumnSector.HeaderText = "Sector";
            ColumnSector.Name = "ColumnSector";
            ColumnSector.ReadOnly = true;
            // 
            // ColumnCantidad
            // 
            ColumnCantidad.DataPropertyName = "Cantidad";
            ColumnCantidad.FillWeight = 60F;
            ColumnCantidad.HeaderText = "Cantidad";
            ColumnCantidad.Name = "ColumnCantidad";
            ColumnCantidad.ReadOnly = true;
            // 
            // ColumnMontoTotal
            // 
            ColumnMontoTotal.DataPropertyName = "MontoTotalTexto";
            ColumnMontoTotal.FillWeight = 85F;
            ColumnMontoTotal.HeaderText = "Monto Total";
            ColumnMontoTotal.Name = "ColumnMontoTotal";
            ColumnMontoTotal.ReadOnly = true;
            // 
            // labelEntradas
            // 
            labelEntradas.AutoSize = true;
            labelEntradas.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelEntradas.Location = new Point(20, 418);
            labelEntradas.Name = "labelEntradas";
            labelEntradas.Size = new Size(340, 23);
            labelEntradas.TabIndex = 4;
            labelEntradas.Text = "Entradas Emitidas de la Compra Seleccionada:";
            // 
            // dataGridViewEntradas
            // 
            dataGridViewEntradas.AllowUserToAddRows = false;
            dataGridViewEntradas.AllowUserToDeleteRows = false;
            dataGridViewEntradas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewEntradas.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewEntradas.Columns.AddRange(new DataGridViewColumn[]
            {
                ColumnEntradaId,
                ColumnEntradaEvento,
                ColumnEntradaSector,
                ColumnEntradaTipo,
                ColumnEntradaUbicacion
            });
            dataGridViewEntradas.Location = new Point(20, 445);
            dataGridViewEntradas.MultiSelect = false;
            dataGridViewEntradas.Name = "dataGridViewEntradas";
            dataGridViewEntradas.ReadOnly = true;
            dataGridViewEntradas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewEntradas.Size = new Size(880, 155);
            dataGridViewEntradas.TabIndex = 5;
            // 
            // ColumnEntradaId
            // 
            ColumnEntradaId.DataPropertyName = "EntradaId";
            ColumnEntradaId.FillWeight = 55F;
            ColumnEntradaId.HeaderText = "N° Entrada";
            ColumnEntradaId.Name = "ColumnEntradaId";
            ColumnEntradaId.ReadOnly = true;
            // 
            // ColumnEntradaEvento
            // 
            ColumnEntradaEvento.DataPropertyName = "EventoNombre";
            ColumnEntradaEvento.FillWeight = 130F;
            ColumnEntradaEvento.HeaderText = "Evento";
            ColumnEntradaEvento.Name = "ColumnEntradaEvento";
            ColumnEntradaEvento.ReadOnly = true;
            // 
            // ColumnEntradaSector
            // 
            ColumnEntradaSector.DataPropertyName = "SectorNombre";
            ColumnEntradaSector.FillWeight = 110F;
            ColumnEntradaSector.HeaderText = "Sector";
            ColumnEntradaSector.Name = "ColumnEntradaSector";
            ColumnEntradaSector.ReadOnly = true;
            // 
            // ColumnEntradaTipo
            // 
            ColumnEntradaTipo.DataPropertyName = "TipoSector";
            ColumnEntradaTipo.FillWeight = 90F;
            ColumnEntradaTipo.HeaderText = "Modalidad";
            ColumnEntradaTipo.Name = "ColumnEntradaTipo";
            ColumnEntradaTipo.ReadOnly = true;
            // 
            // ColumnEntradaUbicacion
            // 
            ColumnEntradaUbicacion.DataPropertyName = "Ubicacion";
            ColumnEntradaUbicacion.FillWeight = 130F;
            ColumnEntradaUbicacion.HeaderText = "Ubicación / Asiento";
            ColumnEntradaUbicacion.Name = "ColumnEntradaUbicacion";
            ColumnEntradaUbicacion.ReadOnly = true;
            // 
            // labelResumen
            // 
            labelResumen.AutoSize = true;
            labelResumen.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelResumen.Location = new Point(20, 616);
            labelResumen.Name = "labelResumen";
            labelResumen.Size = new Size(260, 23);
            labelResumen.TabIndex = 6;
            labelResumen.Text = "Compras: 0 | Total Recaudado: $0,00";
            // 
            // btnAnularCompra
            // 
            btnAnularCompra.BackColor = Color.Firebrick;
            btnAnularCompra.ForeColor = Color.White;
            btnAnularCompra.Location = new Point(700, 610);
            btnAnularCompra.Name = "btnAnularCompra";
            btnAnularCompra.Size = new Size(200, 35);
            btnAnularCompra.TabIndex = 7;
            btnAnularCompra.Text = "Anular Compra Seleccionada";
            btnAnularCompra.UseVisualStyleBackColor = false;
            btnAnularCompra.Click += btnAnularCompra_Click;
            // 
            // FormCompras
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(920, 660);
            Controls.Add(btnAnularCompra);
            Controls.Add(labelResumen);
            Controls.Add(dataGridViewEntradas);
            Controls.Add(labelEntradas);
            Controls.Add(dataGridViewCompras);
            Controls.Add(labelCompras);
            Controls.Add(groupBoxFiltros);
            Controls.Add(labelTitulo);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormCompras";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Auditoría de Compras y Entradas";
            Load += FormCompras_Load;
            groupBoxFiltros.ResumeLayout(false);
            groupBoxFiltros.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewCompras).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridViewEntradas).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label labelTitulo = null!;
        private GroupBox groupBoxFiltros = null!;
        private Label labelFiltroEvento = null!;
        private ComboBox comboBoxFiltroEvento = null!;
        private CheckBox checkBoxFiltrarFecha = null!;
        private Label labelDesde = null!;
        private DateTimePicker dateTimePickerDesde = null!;
        private Label labelHasta = null!;
        private DateTimePicker dateTimePickerHasta = null!;
        private Button btnFiltrar = null!;
        private Button btnRefrescar = null!;
        private Label labelCompras = null!;
        private DataGridView dataGridViewCompras = null!;
        private DataGridViewTextBoxColumn ColumnCompraId = null!;
        private DataGridViewTextBoxColumn ColumnFecha = null!;
        private DataGridViewTextBoxColumn ColumnUsuario = null!;
        private DataGridViewTextBoxColumn ColumnEvento = null!;
        private DataGridViewTextBoxColumn ColumnSector = null!;
        private DataGridViewTextBoxColumn ColumnCantidad = null!;
        private DataGridViewTextBoxColumn ColumnMontoTotal = null!;
        private Label labelEntradas = null!;
        private DataGridView dataGridViewEntradas = null!;
        private DataGridViewTextBoxColumn ColumnEntradaId = null!;
        private DataGridViewTextBoxColumn ColumnEntradaEvento = null!;
        private DataGridViewTextBoxColumn ColumnEntradaSector = null!;
        private DataGridViewTextBoxColumn ColumnEntradaTipo = null!;
        private DataGridViewTextBoxColumn ColumnEntradaUbicacion = null!;
        private Label labelResumen = null!;
        private Button btnAnularCompra = null!;
    }
}
