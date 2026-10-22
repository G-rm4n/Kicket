namespace Kiketpropio.Escritorio.Formularios
{
    partial class EstadioEdicionForm
    {
        private System.ComponentModel.IContainer? components = null;

        private Label lblTitulo;
        private Label lblNombre;
        private TextBox txtNombre;
        private Label lblCalle;
        private TextBox txtCalle;
        private Label lblNro;
        private NumericUpDown nudNro;
        private Label lblCiudad;
        private TextBox txtCiudad;
        private Label lblError;
        private Button btnGuardar;
        private Button btnCancelar;

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
            lblNombre = new Label();
            txtNombre = new TextBox();
            lblCalle = new Label();
            txtCalle = new TextBox();
            lblNro = new Label();
            nudNro = new NumericUpDown();
            lblCiudad = new Label();
            txtCiudad = new TextBox();
            lblError = new Label();
            btnGuardar = new Button();
            btnCancelar = new Button();
            ((System.ComponentModel.ISupportInitialize)nudNro).BeginInit();
            SuspendLayout();

            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblTitulo.ForeColor = Color.FromArgb(21, 49, 75);
            lblTitulo.Location = new Point(22, 18);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(110, 21);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Estadio";

            lblNombre.AutoSize = true;
            lblNombre.Location = new Point(22, 58);
            lblNombre.Name = "lblNombre";
            lblNombre.Size = new Size(51, 15);
            lblNombre.TabIndex = 1;
            lblNombre.Text = "Nombre";

            txtNombre.Location = new Point(22, 76);
            txtNombre.MaxLength = 80;
            txtNombre.Name = "txtNombre";
            txtNombre.Size = new Size(380, 23);
            txtNombre.TabIndex = 2;

            lblCalle.AutoSize = true;
            lblCalle.Location = new Point(22, 111);
            lblCalle.Name = "lblCalle";
            lblCalle.Size = new Size(34, 15);
            lblCalle.TabIndex = 3;
            lblCalle.Text = "Calle";

            txtCalle.Location = new Point(22, 129);
            txtCalle.MaxLength = 80;
            txtCalle.Name = "txtCalle";
            txtCalle.Size = new Size(250, 23);
            txtCalle.TabIndex = 4;

            lblNro.AutoSize = true;
            lblNro.Location = new Point(288, 111);
            lblNro.Name = "lblNro";
            lblNro.Size = new Size(50, 15);
            lblNro.TabIndex = 5;
            lblNro.Text = "Numero";

            nudNro.Location = new Point(288, 129);
            nudNro.Maximum = 99999;
            nudNro.Name = "nudNro";
            nudNro.Size = new Size(114, 23);
            nudNro.TabIndex = 6;

            lblCiudad.AutoSize = true;
            lblCiudad.Location = new Point(22, 164);
            lblCiudad.Name = "lblCiudad";
            lblCiudad.Size = new Size(45, 15);
            lblCiudad.TabIndex = 7;
            lblCiudad.Text = "Ciudad";

            txtCiudad.Location = new Point(22, 182);
            txtCiudad.MaxLength = 80;
            txtCiudad.Name = "txtCiudad";
            txtCiudad.Size = new Size(380, 23);
            txtCiudad.TabIndex = 8;

            lblError.AutoSize = true;
            lblError.ForeColor = Color.FromArgb(192, 57, 43);
            lblError.Location = new Point(22, 215);
            lblError.Name = "lblError";
            lblError.Size = new Size(0, 15);
            lblError.TabIndex = 9;

            btnGuardar.BackColor = Color.FromArgb(31, 157, 85);
            btnGuardar.FlatStyle = FlatStyle.Flat;
            btnGuardar.ForeColor = Color.White;
            btnGuardar.Location = new Point(22, 243);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(185, 34);
            btnGuardar.TabIndex = 10;
            btnGuardar.Text = "Guardar";
            btnGuardar.UseVisualStyleBackColor = false;
            btnGuardar.Click += btnGuardar_Click;

            btnCancelar.BackColor = Color.FromArgb(107, 120, 132);
            btnCancelar.FlatStyle = FlatStyle.Flat;
            btnCancelar.ForeColor = Color.White;
            btnCancelar.Location = new Point(217, 243);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(185, 34);
            btnCancelar.TabIndex = 11;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = false;
            btnCancelar.Click += btnCancelar_Click;

            AcceptButton = btnGuardar;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(424, 304);
            Controls.Add(lblTitulo);
            Controls.Add(lblNombre);
            Controls.Add(txtNombre);
            Controls.Add(lblCalle);
            Controls.Add(txtCalle);
            Controls.Add(lblNro);
            Controls.Add(nudNro);
            Controls.Add(lblCiudad);
            Controls.Add(txtCiudad);
            Controls.Add(lblError);
            Controls.Add(btnGuardar);
            Controls.Add(btnCancelar);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "EstadioEdicionForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Kicket - Estadio";
            ((System.ComponentModel.ISupportInitialize)nudNro).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
