namespace Kiketpropio.Escritorio.Formularios
{
    partial class ReporteEstadiosForm
    {
        private System.ComponentModel.IContainer? components = null;

        private Label lblTitulo;
        private Label lblSubtitulo;
        private Label lblTotales;
        private Button btnActualizar;
        private DataGridView dgvReporte;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            lblTitulo = new Label();
            lblSubtitulo = new Label();
            lblTotales = new Label();
            btnActualizar = new Button();
            dgvReporte = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)dgvReporte).BeginInit();
            SuspendLayout();

            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblTitulo.ForeColor = Color.FromArgb(21, 49, 75);
            lblTitulo.Location = new Point(18, 16);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(340, 25);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Reporte de estadios por ciudad";

            lblSubtitulo.AutoSize = true;
            lblSubtitulo.ForeColor = Color.FromArgb(103, 114, 124);
            lblSubtitulo.Location = new Point(20, 45);
            lblSubtitulo.Name = "lblSubtitulo";
            lblSubtitulo.Size = new Size(420, 15);
            lblSubtitulo.TabIndex = 1;
            lblSubtitulo.Text = "Distribucion geografica de los estadios habilitados en el sistema";

            btnActualizar.BackColor = Color.FromArgb(107, 120, 132);
            btnActualizar.FlatStyle = FlatStyle.Flat;
            btnActualizar.ForeColor = Color.White;
            btnActualizar.Location = new Point(18, 72);
            btnActualizar.Name = "btnActualizar";
            btnActualizar.Size = new Size(120, 32);
            btnActualizar.TabIndex = 2;
            btnActualizar.Text = "Actualizar";
            btnActualizar.UseVisualStyleBackColor = false;
            btnActualizar.Click += btnActualizar_Click;

            lblTotales.AutoSize = true;
            lblTotales.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblTotales.Location = new Point(150, 81);
            lblTotales.Name = "lblTotales";
            lblTotales.Size = new Size(0, 15);
            lblTotales.TabIndex = 3;

            dgvReporte.AllowUserToAddRows = false;
            dgvReporte.AllowUserToDeleteRows = false;
            dgvReporte.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvReporte.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvReporte.BackgroundColor = Color.White;
            dgvReporte.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvReporte.Location = new Point(18, 116);
            dgvReporte.MultiSelect = false;
            dgvReporte.Name = "dgvReporte";
            dgvReporte.ReadOnly = true;
            dgvReporte.RowHeadersVisible = false;
            dgvReporte.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvReporte.Size = new Size(840, 472);
            dgvReporte.TabIndex = 4;

            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(876, 608);
            Controls.Add(lblTitulo);
            Controls.Add(lblSubtitulo);
            Controls.Add(btnActualizar);
            Controls.Add(lblTotales);
            Controls.Add(dgvReporte);
            Name = "ReporteEstadiosForm";
            Text = "Reporte de estadios por ciudad";
            Load += ReporteEstadiosForm_Load;
            ((System.ComponentModel.ISupportInitialize)dgvReporte).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
