namespace Kiketpropio.Escritorio.Formularios
{
    partial class ClubEdicionForm
    {
        private System.ComponentModel.IContainer? components = null;

        private Label lblTitulo;
        private Label lblNombre;
        private TextBox txtNombre;
        private Label lblLogo;
        private TextBox txtLogo;
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
            lblLogo = new Label();
            txtLogo = new TextBox();
            lblError = new Label();
            btnGuardar = new Button();
            btnCancelar = new Button();
            SuspendLayout();

            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblTitulo.ForeColor = Color.FromArgb(21, 49, 75);
            lblTitulo.Location = new Point(22, 18);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(100, 21);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Club";

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

            lblLogo.AutoSize = true;
            lblLogo.Location = new Point(22, 111);
            lblLogo.Name = "lblLogo";
            lblLogo.Size = new Size(78, 15);
            lblLogo.TabIndex = 3;
            lblLogo.Text = "URL del logo";

            txtLogo.Location = new Point(22, 129);
            txtLogo.MaxLength = 200;
            txtLogo.Name = "txtLogo";
            txtLogo.Size = new Size(380, 23);
            txtLogo.TabIndex = 4;

            lblError.AutoSize = true;
            lblError.ForeColor = Color.FromArgb(192, 57, 43);
            lblError.Location = new Point(22, 162);
            lblError.Name = "lblError";
            lblError.Size = new Size(0, 15);
            lblError.TabIndex = 5;

            btnGuardar.BackColor = Color.FromArgb(31, 157, 85);
            btnGuardar.FlatStyle = FlatStyle.Flat;
            btnGuardar.ForeColor = Color.White;
            btnGuardar.Location = new Point(22, 190);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(185, 34);
            btnGuardar.TabIndex = 6;
            btnGuardar.Text = "Guardar";
            btnGuardar.UseVisualStyleBackColor = false;
            btnGuardar.Click += btnGuardar_Click;

            btnCancelar.BackColor = Color.FromArgb(107, 120, 132);
            btnCancelar.FlatStyle = FlatStyle.Flat;
            btnCancelar.ForeColor = Color.White;
            btnCancelar.Location = new Point(217, 190);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(185, 34);
            btnCancelar.TabIndex = 7;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = false;
            btnCancelar.Click += btnCancelar_Click;

            AcceptButton = btnGuardar;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(424, 251);
            Controls.Add(lblTitulo);
            Controls.Add(lblNombre);
            Controls.Add(txtNombre);
            Controls.Add(lblLogo);
            Controls.Add(txtLogo);
            Controls.Add(lblError);
            Controls.Add(btnGuardar);
            Controls.Add(btnCancelar);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "ClubEdicionForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Kicket - Club";
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
