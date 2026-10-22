namespace Kiketpropio.Escritorio.Formularios
{
    partial class EstadiosForm
    {
        private System.ComponentModel.IContainer? components = null;

        private Label lblTitulo;
        private DataGridView dgvEstadios;
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
            dgvEstadios = new DataGridView();
            btnNuevo = new Button();
            btnEditar = new Button();
            btnEliminar = new Button();
            btnActualizar = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvEstadios).BeginInit();
            SuspendLayout();

            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblTitulo.ForeColor = Color.FromArgb(21, 49, 75);
            lblTitulo.Location = new Point(18, 16);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(110, 25);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Estadios";

            dgvEstadios.AllowUserToAddRows = false;
            dgvEstadios.AllowUserToDeleteRows = false;
            dgvEstadios.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvEstadios.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvEstadios.BackgroundColor = Color.White;
            dgvEstadios.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvEstadios.Location = new Point(18, 100);
            dgvEstadios.MultiSelect = false;
            dgvEstadios.Name = "dgvEstadios";
            dgvEstadios.ReadOnly = true;
            dgvEstadios.RowHeadersVisible = false;
            dgvEstadios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvEstadios.Size = new Size(840, 420);
            dgvEstadios.TabIndex = 5;

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
            Controls.Add(dgvEstadios);
            Name = "EstadiosForm";
            Text = "Estadios";
            Load += EstadiosForm_Load;
            ((System.ComponentModel.ISupportInitialize)dgvEstadios).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
