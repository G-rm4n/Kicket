namespace Kicket.WinForms.Forms
{
    partial class FormRegistro
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            LbTitulo = new Label();
            txtNombre = new TextBox();
            txtApellido = new TextBox();
            txtEmail = new TextBox();
            txtPassword = new TextBox();
            lblNombre = new Label();
            lblApellido = new Label();
            lblEmail = new Label();
            lblPassword = new Label();
            btnRegistro = new Button();
            lnkLogin = new LinkLabel();
            SuspendLayout();
            // 
            // LbTitulo
            // 
            LbTitulo.AutoSize = true;
            LbTitulo.Font = new Font("Segoe UI", 15F, FontStyle.Bold);
            LbTitulo.Location = new Point(62, 20);
            LbTitulo.Name = "LbTitulo";
            LbTitulo.Size = new Size(156, 28);
            LbTitulo.TabIndex = 1;
            LbTitulo.Text = "Registro Kicket";
            LbTitulo.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // txtNombre
            // 
            txtNombre.Location = new Point(78, 84);
            txtNombre.Margin = new Padding(3, 2, 3, 2);
            txtNombre.Name = "txtNombre";
            txtNombre.Size = new Size(153, 23);
            txtNombre.TabIndex = 2;
            // 
            // txtApellido
            // 
            txtApellido.Location = new Point(78, 127);
            txtApellido.Margin = new Padding(3, 2, 3, 2);
            txtApellido.Name = "txtApellido";
            txtApellido.Size = new Size(153, 23);
            txtApellido.TabIndex = 3;
            // 
            // txtEmail
            // 
            txtEmail.Location = new Point(78, 170);
            txtEmail.Margin = new Padding(3, 2, 3, 2);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(153, 23);
            txtEmail.TabIndex = 4;
            // 
            // txtPassword
            // 
            txtPassword.Location = new Point(78, 212);
            txtPassword.Margin = new Padding(3, 2, 3, 2);
            txtPassword.Name = "txtPassword";
            txtPassword.Size = new Size(153, 23);
            txtPassword.TabIndex = 5;
            txtPassword.UseSystemPasswordChar = true;
            // 
            // lblNombre
            // 
            lblNombre.AutoSize = true;
            lblNombre.Location = new Point(78, 67);
            lblNombre.Name = "lblNombre";
            lblNombre.Size = new Size(51, 15);
            lblNombre.TabIndex = 6;
            lblNombre.Text = "Nombre";
            // 
            // lblApellido
            // 
            lblApellido.AutoSize = true;
            lblApellido.Location = new Point(78, 110);
            lblApellido.Name = "lblApellido";
            lblApellido.Size = new Size(51, 15);
            lblApellido.TabIndex = 7;
            lblApellido.Text = "Apellido";
            // 
            // lblEmail
            // 
            lblEmail.AutoSize = true;
            lblEmail.Location = new Point(78, 152);
            lblEmail.Name = "lblEmail";
            lblEmail.Size = new Size(41, 15);
            lblEmail.TabIndex = 8;
            lblEmail.Text = "E-mail";
            // 
            // lblPassword
            // 
            lblPassword.AutoSize = true;
            lblPassword.Location = new Point(78, 195);
            lblPassword.Name = "lblPassword";
            lblPassword.Size = new Size(67, 15);
            lblPassword.TabIndex = 9;
            lblPassword.Text = "Contraseña";
            // 
            // btnRegistro
            // 
            btnRegistro.Location = new Point(110, 287);
            btnRegistro.Margin = new Padding(3, 2, 3, 2);
            btnRegistro.Name = "btnRegistro";
            btnRegistro.Size = new Size(82, 22);
            btnRegistro.TabIndex = 10;
            btnRegistro.Text = "Registrarse";
            btnRegistro.UseVisualStyleBackColor = true;
            btnRegistro.Click += btnRegistrar_Click;
            // 
            // lnkLogin
            // 
            lnkLogin.AutoSize = true;
            lnkLogin.Location = new Point(78, 248);
            lnkLogin.Name = "lnkLogin";
            lnkLogin.Size = new Size(149, 15);
            lnkLogin.TabIndex = 11;
            lnkLogin.TabStop = true;
            lnkLogin.Text = "Ya posees usuario? Ingresa!";
            lnkLogin.LinkClicked += lnkLogin_LinkClicked;
            // 
            // FormRegistro
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(299, 340);
            Controls.Add(lnkLogin);
            Controls.Add(btnRegistro);
            Controls.Add(lblPassword);
            Controls.Add(lblEmail);
            Controls.Add(lblApellido);
            Controls.Add(lblNombre);
            Controls.Add(txtPassword);
            Controls.Add(txtEmail);
            Controls.Add(txtApellido);
            Controls.Add(txtNombre);
            Controls.Add(LbTitulo);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Margin = new Padding(3, 2, 3, 2);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormRegistro";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Registro";
            Load += FormRegistro_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label LbTitulo;
        private TextBox txtNombre;
        private TextBox txtApellido;
        private TextBox txtEmail;
        private TextBox txtPassword;
        private Label lblNombre;
        private Label lblApellido;
        private Label lblEmail;
        private Label lblPassword;
        private Button btnRegistro;
        private LinkLabel lnkLogin;
    }
}