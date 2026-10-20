using System.Drawing;
using System.Windows.Forms;

namespace Kicket.WinForms.Forms
{
    partial class FormEvento
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
            labelNombre = new Label();
            textBoxNombre = new TextBox();
            labelFecha = new Label();
            dateTimePickerFecha = new DateTimePicker();
            labelEstadio = new Label();
            comboBoxEstadio = new ComboBox();
            labelClubLocal = new Label();
            comboBoxClubLocal = new ComboBox();
            labelClubVisitante = new Label();
            comboBoxClubVisitante = new ComboBox();
            checkBoxCancelado = new CheckBox();
            btnGuardar = new Button();
            btnModificar = new Button();
            btnEliminar = new Button();
            btnLimpiar = new Button();
            btnVerDisponibilidad = new Button();
            labelListado = new Label();
            dataGridViewEventos = new DataGridView();
            ColumnIdEvento = new DataGridViewTextBoxColumn();
            ColumnNombre = new DataGridViewTextBoxColumn();
            ColumnFecha = new DataGridViewTextBoxColumn();
            ColumnEstadio = new DataGridViewTextBoxColumn();
            ColumnLocal = new DataGridViewTextBoxColumn();
            ColumnVisitante = new DataGridViewTextBoxColumn();
            ColumnEstado = new DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)dataGridViewEventos).BeginInit();
            SuspendLayout();
            // 
            // labelTitulo
            // 
            labelTitulo.AutoSize = true;
            labelTitulo.Font = new Font("Segoe UI", 16F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelTitulo.Location = new Point(20, 14);
            labelTitulo.Name = "labelTitulo";
            labelTitulo.Size = new Size(340, 37);
            labelTitulo.TabIndex = 0;
            labelTitulo.Text = "Gestión de Eventos / Partidos";
            // 
            // labelNombre
            // 
            labelNombre.AutoSize = true;
            labelNombre.Location = new Point(22, 65);
            labelNombre.Name = "labelNombre";
            labelNombre.Size = new Size(140, 20);
            labelNombre.TabIndex = 1;
            labelNombre.Text = "Nombre del Evento:";
            // 
            // textBoxNombre
            // 
            textBoxNombre.Location = new Point(165, 62);
            textBoxNombre.Name = "textBoxNombre";
            textBoxNombre.Size = new Size(270, 27);
            textBoxNombre.TabIndex = 2;
            // 
            // labelFecha
            // 
            labelFecha.AutoSize = true;
            labelFecha.Location = new Point(460, 65);
            labelFecha.Name = "labelFecha";
            labelFecha.Size = new Size(105, 20);
            labelFecha.TabIndex = 3;
            labelFecha.Text = "Fecha y Hora:";
            // 
            // dateTimePickerFecha
            // 
            dateTimePickerFecha.CustomFormat = "dd/MM/yyyy HH:mm";
            dateTimePickerFecha.Format = DateTimePickerFormat.Custom;
            dateTimePickerFecha.Location = new Point(575, 62);
            dateTimePickerFecha.Name = "dateTimePickerFecha";
            dateTimePickerFecha.Size = new Size(210, 27);
            dateTimePickerFecha.TabIndex = 4;
            // 
            // labelEstadio
            // 
            labelEstadio.AutoSize = true;
            labelEstadio.Location = new Point(22, 108);
            labelEstadio.Name = "labelEstadio";
            labelEstadio.Size = new Size(65, 20);
            labelEstadio.TabIndex = 5;
            labelEstadio.Text = "Estadio:";
            // 
            // comboBoxEstadio
            // 
            comboBoxEstadio.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxEstadio.FormattingEnabled = true;
            comboBoxEstadio.Location = new Point(165, 105);
            comboBoxEstadio.Name = "comboBoxEstadio";
            comboBoxEstadio.Size = new Size(270, 28);
            comboBoxEstadio.TabIndex = 6;
            // 
            // labelClubLocal
            // 
            labelClubLocal.AutoSize = true;
            labelClubLocal.Location = new Point(22, 151);
            labelClubLocal.Name = "labelClubLocal";
            labelClubLocal.Size = new Size(85, 20);
            labelClubLocal.TabIndex = 7;
            labelClubLocal.Text = "Club Local:";
            // 
            // comboBoxClubLocal
            // 
            comboBoxClubLocal.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxClubLocal.FormattingEnabled = true;
            comboBoxClubLocal.Location = new Point(165, 148);
            comboBoxClubLocal.Name = "comboBoxClubLocal";
            comboBoxClubLocal.Size = new Size(270, 28);
            comboBoxClubLocal.TabIndex = 8;
            // 
            // labelClubVisitante
            // 
            labelClubVisitante.AutoSize = true;
            labelClubVisitante.Location = new Point(460, 151);
            labelClubVisitante.Name = "labelClubVisitante";
            labelClubVisitante.Size = new Size(108, 20);
            labelClubVisitante.TabIndex = 9;
            labelClubVisitante.Text = "Club Visitante:";
            // 
            // comboBoxClubVisitante
            // 
            comboBoxClubVisitante.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxClubVisitante.FormattingEnabled = true;
            comboBoxClubVisitante.Location = new Point(575, 148);
            comboBoxClubVisitante.Name = "comboBoxClubVisitante";
            comboBoxClubVisitante.Size = new Size(270, 28);
            comboBoxClubVisitante.TabIndex = 10;
            // 
            // checkBoxCancelado
            // 
            checkBoxCancelado.AutoSize = true;
            checkBoxCancelado.Location = new Point(575, 107);
            checkBoxCancelado.Name = "checkBoxCancelado";
            checkBoxCancelado.Size = new Size(155, 24);
            checkBoxCancelado.TabIndex = 11;
            checkBoxCancelado.Text = "Evento Cancelado";
            checkBoxCancelado.UseVisualStyleBackColor = true;
            // 
            // btnGuardar
            // 
            btnGuardar.Location = new Point(22, 198);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(120, 34);
            btnGuardar.TabIndex = 12;
            btnGuardar.Text = "Guardar Nuevo";
            btnGuardar.UseVisualStyleBackColor = true;
            btnGuardar.Click += btnGuardar_Click;
            // 
            // btnModificar
            // 
            btnModificar.Location = new Point(152, 198);
            btnModificar.Name = "btnModificar";
            btnModificar.Size = new Size(120, 34);
            btnModificar.TabIndex = 13;
            btnModificar.Text = "Modificar";
            btnModificar.UseVisualStyleBackColor = true;
            btnModificar.Click += btnModificar_Click;
            // 
            // btnEliminar
            // 
            btnEliminar.Location = new Point(282, 198);
            btnEliminar.Name = "btnEliminar";
            btnEliminar.Size = new Size(120, 34);
            btnEliminar.TabIndex = 14;
            btnEliminar.Text = "Eliminar";
            btnEliminar.UseVisualStyleBackColor = true;
            btnEliminar.Click += btnEliminar_Click;
            // 
            // btnLimpiar
            // 
            btnLimpiar.Location = new Point(412, 198);
            btnLimpiar.Name = "btnLimpiar";
            btnLimpiar.Size = new Size(120, 34);
            btnLimpiar.TabIndex = 15;
            btnLimpiar.Text = "Limpiar";
            btnLimpiar.UseVisualStyleBackColor = true;
            btnLimpiar.Click += btnLimpiar_Click;
            // 
            // btnVerDisponibilidad
            // 
            btnVerDisponibilidad.Location = new Point(645, 198);
            btnVerDisponibilidad.Name = "btnVerDisponibilidad";
            btnVerDisponibilidad.Size = new Size(200, 34);
            btnVerDisponibilidad.TabIndex = 16;
            btnVerDisponibilidad.Text = "Ver Disponibilidad Sectores";
            btnVerDisponibilidad.UseVisualStyleBackColor = true;
            btnVerDisponibilidad.Click += btnVerDisponibilidad_Click;
            // 
            // labelListado
            // 
            labelListado.AutoSize = true;
            labelListado.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelListado.Location = new Point(22, 246);
            labelListado.Name = "labelListado";
            labelListado.Size = new Size(230, 23);
            labelListado.TabIndex = 17;
            labelListado.Text = "Listado de Eventos Cargados:";
            // 
            // dataGridViewEventos
            // 
            dataGridViewEventos.AllowUserToAddRows = false;
            dataGridViewEventos.AllowUserToDeleteRows = false;
            dataGridViewEventos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewEventos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewEventos.Columns.AddRange(new DataGridViewColumn[]
            {
                ColumnIdEvento,
                ColumnNombre,
                ColumnFecha,
                ColumnEstadio,
                ColumnLocal,
                ColumnVisitante,
                ColumnEstado
            });
            dataGridViewEventos.Location = new Point(22, 275);
            dataGridViewEventos.MultiSelect = false;
            dataGridViewEventos.Name = "dataGridViewEventos";
            dataGridViewEventos.ReadOnly = true;
            dataGridViewEventos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewEventos.Size = new Size(823, 285);
            dataGridViewEventos.TabIndex = 18;
            dataGridViewEventos.CellClick += dataGridViewEventos_CellClick;
            // 
            // ColumnIdEvento
            // 
            ColumnIdEvento.DataPropertyName = "IdEvento";
            ColumnIdEvento.FillWeight = 45F;
            ColumnIdEvento.HeaderText = "ID";
            ColumnIdEvento.Name = "ColumnIdEvento";
            ColumnIdEvento.ReadOnly = true;
            // 
            // ColumnNombre
            // 
            ColumnNombre.DataPropertyName = "Nombre";
            ColumnNombre.FillWeight = 130F;
            ColumnNombre.HeaderText = "Evento";
            ColumnNombre.Name = "ColumnNombre";
            ColumnNombre.ReadOnly = true;
            // 
            // ColumnFecha
            // 
            ColumnFecha.DataPropertyName = "FechaTexto";
            ColumnFecha.FillWeight = 100F;
            ColumnFecha.HeaderText = "Fecha y Hora";
            ColumnFecha.Name = "ColumnFecha";
            ColumnFecha.ReadOnly = true;
            // 
            // ColumnEstadio
            // 
            ColumnEstadio.DataPropertyName = "EstadioNombre";
            ColumnEstadio.FillWeight = 110F;
            ColumnEstadio.HeaderText = "Estadio";
            ColumnEstadio.Name = "ColumnEstadio";
            ColumnEstadio.ReadOnly = true;
            // 
            // ColumnLocal
            // 
            ColumnLocal.DataPropertyName = "ClubLocalNombre";
            ColumnLocal.FillWeight = 100F;
            ColumnLocal.HeaderText = "Club Local";
            ColumnLocal.Name = "ColumnLocal";
            ColumnLocal.ReadOnly = true;
            // 
            // ColumnVisitante
            // 
            ColumnVisitante.DataPropertyName = "ClubVisitanteNombre";
            ColumnVisitante.FillWeight = 100F;
            ColumnVisitante.HeaderText = "Club Visitante";
            ColumnVisitante.Name = "ColumnVisitante";
            ColumnVisitante.ReadOnly = true;
            // 
            // ColumnEstado
            // 
            ColumnEstado.DataPropertyName = "EstadoTexto";
            ColumnEstado.FillWeight = 80F;
            ColumnEstado.HeaderText = "Estado";
            ColumnEstado.Name = "ColumnEstado";
            ColumnEstado.ReadOnly = true;
            // 
            // FormEvento
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(870, 580);
            Controls.Add(dataGridViewEventos);
            Controls.Add(labelListado);
            Controls.Add(btnVerDisponibilidad);
            Controls.Add(btnLimpiar);
            Controls.Add(btnEliminar);
            Controls.Add(btnModificar);
            Controls.Add(btnGuardar);
            Controls.Add(checkBoxCancelado);
            Controls.Add(comboBoxClubVisitante);
            Controls.Add(labelClubVisitante);
            Controls.Add(comboBoxClubLocal);
            Controls.Add(labelClubLocal);
            Controls.Add(comboBoxEstadio);
            Controls.Add(labelEstadio);
            Controls.Add(dateTimePickerFecha);
            Controls.Add(labelFecha);
            Controls.Add(textBoxNombre);
            Controls.Add(labelNombre);
            Controls.Add(labelTitulo);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormEvento";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Administración de Eventos";
            Load += FormEvento_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridViewEventos).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label labelTitulo = null!;
        private Label labelNombre = null!;
        private TextBox textBoxNombre = null!;
        private Label labelFecha = null!;
        private DateTimePicker dateTimePickerFecha = null!;
        private Label labelEstadio = null!;
        private ComboBox comboBoxEstadio = null!;
        private Label labelClubLocal = null!;
        private ComboBox comboBoxClubLocal = null!;
        private Label labelClubVisitante = null!;
        private ComboBox comboBoxClubVisitante = null!;
        private CheckBox checkBoxCancelado = null!;
        private Button btnGuardar = null!;
        private Button btnModificar = null!;
        private Button btnEliminar = null!;
        private Button btnLimpiar = null!;
        private Button btnVerDisponibilidad = null!;
        private Label labelListado = null!;
        private DataGridView dataGridViewEventos = null!;
        private DataGridViewTextBoxColumn ColumnIdEvento = null!;
        private DataGridViewTextBoxColumn ColumnNombre = null!;
        private DataGridViewTextBoxColumn ColumnFecha = null!;
        private DataGridViewTextBoxColumn ColumnEstadio = null!;
        private DataGridViewTextBoxColumn ColumnLocal = null!;
        private DataGridViewTextBoxColumn ColumnVisitante = null!;
        private DataGridViewTextBoxColumn ColumnEstado = null!;
    }
}
