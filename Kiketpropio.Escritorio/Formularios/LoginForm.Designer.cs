namespace Kiketpropio.Escritorio.Formularios
{
    partial class LoginForm
    {
        private System.ComponentModel.IContainer? components = null;

        private Label lblTitulo;
        private Label lblSubtitulo;
        private Label lblEmail;
        private TextBox txtEmail;
        private Label lblPass;
        private TextBox txtPass;
        private Label lblError;
        private Button btnIngresar;
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
            lblSubtitulo = new Label();
            lblEmail = new Label();
            txtEmail = new TextBox();
            lblPass = new Label();
            txtPass = new TextBox();
            lblError = new Label();
            btnIngresar = new Button();
            btnCancelar = new Button();
            SuspendLayout();

            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblTitulo.ForeColor = Color.FromArgb(21, 49, 75);
            lblTitulo.Location = new Point(30, 25);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(110, 37);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Kicket";

            lblSubtitulo.AutoSize = true;
            lblSubtitulo.ForeColor = Color.FromArgb(103, 114, 124);
            lblSubtitulo.Location = new Point(33, 65);
            lblSubtitulo.Name = "lblSubtitulo";
            lblSubtitulo.Size = new Size(200, 15);
            lblSubtitulo.TabIndex = 1;
            lblSubtitulo.Text = "Sistema de gestion de entradas";

            lblEmail.AutoSize = true;
            lblEmail.Location = new Point(30, 105);
            lblEmail.Name = "lblEmail";
            lblEmail.Size = new Size(39, 15);
            lblEmail.TabIndex = 2;
            lblEmail.Text = "Email";

            txtEmail.Location = new Point(30, 123);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(320, 23);
            txtEmail.TabIndex = 3;

            lblPass.AutoSize = true;
            lblPass.Location = new Point(30, 158);
            lblPass.Name = "lblPass";
            lblPass.Size = new Size(70, 15);
            lblPass.TabIndex = 4;
            lblPass.Text = "Contraseña";

            txtPass.Location = new Point(30, 176);
            txtPass.Name = "txtPass";
            txtPass.Size = new Size(320, 23);
            txtPass.TabIndex = 5;
            txtPass.UseSystemPasswordChar = true;

            lblError.AutoSize = true;
            lblError.ForeColor = Color.FromArgb(192, 57, 43);
            lblError.Location = new Point(30, 208);
            lblError.Name = "lblError";
            lblError.Size = new Size(0, 15);
            lblError.TabIndex = 6;

            btnIngresar.BackColor = Color.FromArgb(31, 77, 120);
            btnIngresar.FlatStyle = FlatStyle.Flat;
            btnIngresar.ForeColor = Color.White;
            btnIngresar.Location = new Point(30, 235);
            btnIngresar.Name = "btnIngresar";
            btnIngresar.Size = new Size(155, 34);
            btnIngresar.TabIndex = 7;
            btnIngresar.Text = "Ingresar";
            btnIngresar.UseVisualStyleBackColor = false;
            btnIngresar.Click += btnIngresar_Click;

            btnCancelar.BackColor = Color.FromArgb(107, 120, 132);
            btnCancelar.FlatStyle = FlatStyle.Flat;
            btnCancelar.ForeColor = Color.White;
            btnCancelar.Location = new Point(195, 235);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(155, 34);
            btnCancelar.TabIndex = 8;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = false;
            btnCancelar.Click += btnCancelar_Click;

            AcceptButton = btnIngresar;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(384, 301);
            Controls.Add(lblTitulo);
            Controls.Add(lblSubtitulo);
            Controls.Add(lblEmail);
            Controls.Add(txtEmail);
            Controls.Add(lblPass);
            Controls.Add(txtPass);
            Controls.Add(lblError);
            Controls.Add(btnIngresar);
            Controls.Add(btnCancelar);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "LoginForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Kicket - Iniciar sesion";
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
