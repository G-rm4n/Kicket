namespace Kiketpropio.Escritorio.Formularios
{
    partial class ClubesForm
    {
        private System.ComponentModel.IContainer? components = null;

        private Label lblTitulo;
        private DataGridView dgvClubes;
        private Button btnNuevo;
        private Button btnEditar;
        private Button btnEliminar;
        private Button btnActualizar;

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
            dgvClubes = new DataGridView();
            btnNuevo = new Button();
            btnEditar = new Button();
            btnEliminar = new Button();
            btnActualizar = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvClubes).BeginInit();
            SuspendLayout();

            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblTitulo.ForeColor = Color.FromArgb(21, 49, 75);
            lblTitulo.Location = new Point(18, 16);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(96, 25);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Clubes";

            dgvClubes.AllowUserToAddRows = false;
            dgvClubes.AllowUserToDeleteRows = false;
            dgvClubes.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvClubes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvClubes.BackgroundColor = Color.White;
            dgvClubes.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvClubes.Location = new Point(18, 100);
            dgvClubes.MultiSelect = false;
            dgvClubes.Name = "dgvClubes";
            dgvClubes.ReadOnly = true;
            dgvClubes.RowHeadersVisible = false;
            dgvClubes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvClubes.Size = new Size(840, 420);
            dgvClubes.TabIndex = 5;

            btnNuevo.BackColor = Color.FromArgb(31, 157, 85);
            btnNuevo.FlatStyle = FlatStyle.Flat;
            btnNuevo.ForeColor = Color.White;
            btnNuevo.Location = new Point(18, 56);
            btnNuevo.Name = "btnNuevo";
            btnNuevo.Size = new Size(120, 32);
            btnNuevo.TabIndex = 1;
            btnNuevo.Text = "Nuevo";
            btnNuevo.UseVisualStyleBackColor = false;
            btnNuevo.Click += btnNuevo_Click;

            btnEditar.BackColor = Color.FromArgb(31, 77, 120);
            btnEditar.FlatStyle = FlatStyle.Flat;
            btnEditar.ForeColor = Color.White;
            btnEditar.Location = new Point(146, 56);
            btnEditar.Name = "btnEditar";
            btnEditar.Size = new Size(120, 32);
            btnEditar.TabIndex = 2;
            btnEditar.Text = "Editar";
            btnEditar.UseVisualStyleBackColor = false;
            btnEditar.Click += btnEditar_Click;

            btnEliminar.BackColor = Color.FromArgb(192, 57, 43);
            btnEliminar.FlatStyle = FlatStyle.Flat;
            btnEliminar.ForeColor = Color.White;
            btnEliminar.Location = new Point(274, 56);
            btnEliminar.Name = "btnEliminar";
            btnEliminar.Size = new Size(120, 32);
            btnEliminar.TabIndex = 3;
            btnEliminar.Text = "Eliminar";
            btnEliminar.UseVisualStyleBackColor = false;
            btnEliminar.Click += btnEliminar_Click;

            btnActualizar.BackColor = Color.FromArgb(107, 120, 132);
            btnActualizar.FlatStyle = FlatStyle.Flat;
            btnActualizar.ForeColor = Color.White;
            btnActualizar.Location = new Point(402, 56);
            btnActualizar.Name = "btnActualizar";
            btnActualizar.Size = new Size(120, 32);
            btnActualizar.TabIndex = 4;
            btnActualizar.Text = "Actualizar";
            btnActualizar.UseVisualStyleBackColor = false;
            btnActualizar.Click += btnActualizar_Click;

            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(876, 540);
            Controls.Add(lblTitulo);
            Controls.Add(btnNuevo);
            Controls.Add(btnEditar);
            Controls.Add(btnEliminar);
            Controls.Add(btnActualizar);
            Controls.Add(dgvClubes);
            Name = "ClubesForm";
            Text = "Clubes";
            Load += ClubesForm_Load;
            ((System.ComponentModel.ISupportInitialize)dgvClubes).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
