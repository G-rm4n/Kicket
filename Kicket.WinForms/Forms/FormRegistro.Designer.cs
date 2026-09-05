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
            LbTitulo.Location = new Point(71, 27);
            LbTitulo.Name = "LbTitulo";
            LbTitulo.Size = new Size(192, 35);
            LbTitulo.TabIndex = 1;
            LbTitulo.Text = "Registro Kicket";
            LbTitulo.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // txtNombre
            // 
            txtNombre.Location = new Point(89, 112);
            txtNombre.Name = "txtNombre";
            txtNombre.Size = new Size(174, 27);
            txtNombre.TabIndex = 2;
            // 
            // txtApellido
            // 
            txtApellido.Location = new Point(89, 169);
            txtApellido.Name = "txtApellido";
            txtApellido.Size = new Size(174, 27);
            txtApellido.TabIndex = 3;
            // 
            // txtEmail
            // 
            txtEmail.Location = new Point(89, 226);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(174, 27);
            txtEmail.TabIndex = 4;
            // 
            // txtPassword
            // 
            txtPassword.Location = new Point(89, 283);
            txtPassword.Name = "txtPassword";
            txtPassword.Size = new Size(174, 27);
            txtPassword.TabIndex = 5;
            // 
            // lblNombre
            // 
            lblNombre.AutoSize = true;
            lblNombre.Location = new Point(89, 89);
            lblNombre.Name = "lblNombre";
            lblNombre.Size = new Size(64, 20);
            lblNombre.TabIndex = 6;
            lblNombre.Text = "Nombre";
            // 
            // lblApellido
            // 
            lblApellido.AutoSize = true;
            lblApellido.Location = new Point(89, 146);
            lblApellido.Name = "lblApellido";
            lblApellido.Size = new Size(66, 20);
            lblApellido.TabIndex = 7;
            lblApellido.Text = "Apellido";
            // 
            // lblEmail
            // 
            lblEmail.AutoSize = true;
            lblEmail.Location = new Point(89, 203);
            lblEmail.Name = "lblEmail";
            lblEmail.Size = new Size(52, 20);
            lblEmail.TabIndex = 8;
            lblEmail.Text = "E-mail";
            // 
            // lblPassword
            // 
            lblPassword.AutoSize = true;
            lblPassword.Location = new Point(89, 260);
            lblPassword.Name = "lblPassword";
            lblPassword.Size = new Size(83, 20);
            lblPassword.TabIndex = 9;
            lblPassword.Text = "Contraseña";
            // 
            // btnRegistro
            // 
            btnRegistro.Location = new Point(126, 383);
            btnRegistro.Name = "btnRegistro";
            btnRegistro.Size = new Size(94, 29);
            btnRegistro.TabIndex = 10;
            btnRegistro.Text = "Registrarse";
            btnRegistro.UseVisualStyleBackColor = true;
            btnRegistro.Click += btnAccion_click;
            // 
            // lnkLogin
            // 
            lnkLogin.AutoSize = true;
            lnkLogin.Location = new Point(89, 330);
            lnkLogin.Name = "lnkLogin";
            lnkLogin.Size = new Size(189, 20);
            lnkLogin.TabIndex = 11;
            lnkLogin.TabStop = true;
            lnkLogin.Text = "Ya posees usuario? Ingresa!";
            lnkLogin.LinkClicked += lnkLogin_LinkClicked;
            // 
            // FormRegistro
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(342, 453);
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