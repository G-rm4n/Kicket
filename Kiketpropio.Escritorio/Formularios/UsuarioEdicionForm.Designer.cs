namespace Kiketpropio.Escritorio.Formularios
{
    partial class UsuarioEdicionForm
    {
        private System.ComponentModel.IContainer? components = null;

        private Label lblTitulo;
        private Label lblDni;
        private TextBox txtDni;
        private Label lblNombre;
        private TextBox txtNombre;
        private Label lblApellido;
        private TextBox txtApellido;
        private Label lblEmail;
        private TextBox txtEmail;
        private Label lblPass;
        private TextBox txtPass;
        private Label lblFechaNacimiento;
        private DateTimePicker dtpFechaNacimiento;
        private Label lblRol;
        private ComboBox cboRol;
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
            lblDni = new Label();
            txtDni = new TextBox();
            lblNombre = new Label();
            txtNombre = new TextBox();
            lblApellido = new Label();
            txtApellido = new TextBox();
            lblEmail = new Label();
            txtEmail = new TextBox();
            lblPass = new Label();
            txtPass = new TextBox();
            lblFechaNacimiento = new Label();
            dtpFechaNacimiento = new DateTimePicker();
            lblRol = new Label();
            cboRol = new ComboBox();
            lblError = new Label();
            btnGuardar = new Button();
            btnCancelar = new Button();
            SuspendLayout();

            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblTitulo.ForeColor = Color.FromArgb(21, 49, 75);
            lblTitulo.Location = new Point(22, 18);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(120, 21);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Usuario";

            lblDni.AutoSize = true;
            lblDni.Location = new Point(22, 58);
            lblDni.Name = "lblDni";
            lblDni.Size = new Size(28, 15);
            lblDni.TabIndex = 1;
            lblDni.Text = "DNI";

            txtDni.Location = new Point(22, 76);
            txtDni.MaxLength = 10;
            txtDni.Name = "txtDni";
            txtDni.Size = new Size(185, 23);
            txtDni.TabIndex = 2;

            lblFechaNacimiento.AutoSize = true;
            lblFechaNacimiento.Location = new Point(217, 58);
            lblFechaNacimiento.Name = "lblFechaNacimiento";
            lblFechaNacimiento.Size = new Size(130, 15);
            lblFechaNacimiento.TabIndex = 3;
            lblFechaNacimiento.Text = "Fecha de nacimiento";

            dtpFechaNacimiento.Format = DateTimePickerFormat.Short;
            dtpFechaNacimiento.Location = new Point(217, 76);
            dtpFechaNacimiento.Name = "dtpFechaNacimiento";
            dtpFechaNacimiento.Size = new Size(185, 23);
            dtpFechaNacimiento.TabIndex = 4;

            lblNombre.AutoSize = true;
            lblNombre.Location = new Point(22, 111);
            lblNombre.Name = "lblNombre";
            lblNombre.Size = new Size(51, 15);
            lblNombre.TabIndex = 5;
            lblNombre.Text = "Nombre";

            txtNombre.Location = new Point(22, 129);
            txtNombre.MaxLength = 60;
            txtNombre.Name = "txtNombre";
            txtNombre.Size = new Size(185, 23);
            txtNombre.TabIndex = 6;

            lblApellido.AutoSize = true;
            lblApellido.Location = new Point(217, 111);
            lblApellido.Name = "lblApellido";
            lblApellido.Size = new Size(54, 15);
            lblApellido.TabIndex = 7;
            lblApellido.Text = "Apellido";

            txtApellido.Location = new Point(217, 129);
            txtApellido.MaxLength = 60;
            txtApellido.Name = "txtApellido";
            txtApellido.Size = new Size(185, 23);
            txtApellido.TabIndex = 8;

            lblEmail.AutoSize = true;
            lblEmail.Location = new Point(22, 164);
            lblEmail.Name = "lblEmail";
            lblEmail.Size = new Size(39, 15);
            lblEmail.TabIndex = 9;
            lblEmail.Text = "Email";

            txtEmail.Location = new Point(22, 182);
            txtEmail.MaxLength = 120;
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(380, 23);
            txtEmail.TabIndex = 10;

            lblPass.AutoSize = true;
            lblPass.Location = new Point(22, 217);
            lblPass.Name = "lblPass";
            lblPass.Size = new Size(70, 15);
            lblPass.TabIndex = 11;
            lblPass.Text = "Contraseña";

            txtPass.Location = new Point(22, 235);
            txtPass.MaxLength = 40;
            txtPass.Name = "txtPass";
            txtPass.Size = new Size(185, 23);
            txtPass.TabIndex = 12;

            lblRol.AutoSize = true;
            lblRol.Location = new Point(217, 217);
            lblRol.Name = "lblRol";
            lblRol.Size = new Size(35, 15);
            lblRol.TabIndex = 13;
            lblRol.Text = "Perfil";

            cboRol.DropDownStyle = ComboBoxStyle.DropDownList;
            cboRol.Items.AddRange(new object[] { "Admin", "Vendedor", "User" });
            cboRol.Location = new Point(217, 235);
            cboRol.Name = "cboRol";
            cboRol.Size = new Size(185, 23);
            cboRol.TabIndex = 14;

            lblError.AutoSize = true;
            lblError.ForeColor = Color.FromArgb(192, 57, 43);
            lblError.Location = new Point(22, 268);
            lblError.Name = "lblError";
            lblError.Size = new Size(0, 15);
            lblError.TabIndex = 15;

            btnGuardar.BackColor = Color.FromArgb(31, 157, 85);
            btnGuardar.FlatStyle = FlatStyle.Flat;
            btnGuardar.ForeColor = Color.White;
            btnGuardar.Location = new Point(22, 296);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(185, 34);
            btnGuardar.TabIndex = 16;
            btnGuardar.Text = "Guardar";
            btnGuardar.UseVisualStyleBackColor = false;
            btnGuardar.Click += btnGuardar_Click;

            btnCancelar.BackColor = Color.FromArgb(107, 120, 132);
            btnCancelar.FlatStyle = FlatStyle.Flat;
            btnCancelar.ForeColor = Color.White;
            btnCancelar.Location = new Point(217, 296);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(185, 34);
            btnCancelar.TabIndex = 17;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = false;
            btnCancelar.Click += btnCancelar_Click;

            AcceptButton = btnGuardar;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(424, 357);
            Controls.Add(lblTitulo);
            Controls.Add(lblDni);
            Controls.Add(txtDni);
            Controls.Add(lblFechaNacimiento);
            Controls.Add(dtpFechaNacimiento);
            Controls.Add(lblNombre);
            Controls.Add(txtNombre);
            Controls.Add(lblApellido);
            Controls.Add(txtApellido);
            Controls.Add(lblEmail);
            Controls.Add(txtEmail);
            Controls.Add(lblPass);
            Controls.Add(txtPass);
            Controls.Add(lblRol);
            Controls.Add(cboRol);
            Controls.Add(lblError);
            Controls.Add(btnGuardar);
            Controls.Add(btnCancelar);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "UsuarioEdicionForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Kicket - Usuario";
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
