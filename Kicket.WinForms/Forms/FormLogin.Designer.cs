

namespace Kicket.WinForms
{
    partial class FormLogin
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
            LbEmail = new Label();
            txtEmail = new TextBox();
            LbPassword = new Label();
            txtPassword = new TextBox();
            BtnIngresar = new Button();
            lnkRegistro = new LinkLabel();
            SuspendLayout();
            // 
            // LbTitulo
            // 
            LbTitulo.AutoSize = true;
            LbTitulo.Font = new Font("Segoe UI", 15F, FontStyle.Bold);
            LbTitulo.Location = new Point(83, 20);
            LbTitulo.Name = "LbTitulo";
            LbTitulo.Size = new Size(160, 35);
            LbTitulo.TabIndex = 0;
            LbTitulo.Text = "Login Kicket";
            LbTitulo.TextAlign = ContentAlignment.MiddleCenter;
            LbTitulo.Click += label1_Click;
            // 
            // LbEmail
            // 
            LbEmail.AutoSize = true;
            LbEmail.Location = new Point(51, 98);
            LbEmail.Name = "LbEmail";
            LbEmail.Size = new Size(46, 20);
            LbEmail.TabIndex = 1;
            LbEmail.Text = "Email";
            LbEmail.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // txtEmail
            // 
            txtEmail.Location = new Point(51, 122);
            txtEmail.Margin = new Padding(3, 4, 3, 4);
            txtEmail.Name = "txtEmail";
            txtEmail.PlaceholderText = "Email@domain.com";
            txtEmail.Size = new Size(219, 27);
            txtEmail.TabIndex = 2;
            // 
            // LbPassword
            // 
            LbPassword.AutoSize = true;
            LbPassword.Location = new Point(51, 153);
            LbPassword.Name = "LbPassword";
            LbPassword.Size = new Size(83, 20);
            LbPassword.TabIndex = 3;
            LbPassword.Text = "Contraseña";
            // 
            // txtPassword
            // 
            txtPassword.Location = new Point(51, 177);
            txtPassword.Margin = new Padding(3, 4, 3, 4);
            txtPassword.Name = "txtPassword";
            txtPassword.PasswordChar = '*';
            txtPassword.PlaceholderText = "Pass1234";
            txtPassword.Size = new Size(219, 27);
            txtPassword.TabIndex = 4;
            txtPassword.UseSystemPasswordChar = true;
            // 
            // BtnIngresar
            // 
            BtnIngresar.Location = new Point(110, 253);
            BtnIngresar.Margin = new Padding(3, 4, 3, 4);
            BtnIngresar.Name = "BtnIngresar";
            BtnIngresar.Size = new Size(114, 55);
            BtnIngresar.TabIndex = 5;
            BtnIngresar.Text = "Ingresar";
            BtnIngresar.UseVisualStyleBackColor = true;
            BtnIngresar.Click += this.BtnIngresar_Click;
            // 
            // lnkRegistro
            // 
            lnkRegistro.AutoSize = true;
            lnkRegistro.Location = new Point(51, 218);
            lnkRegistro.Name = "lnkRegistro";
            lnkRegistro.Size = new Size(206, 20);
            lnkRegistro.TabIndex = 6;
            lnkRegistro.TabStop = true;
            lnkRegistro.Text = "No tienes usuario? Registrate!";
            lnkRegistro.LinkClicked += linkLabel1_LinkClicked;
            // 
            // FormLogin
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(342, 373);
            Controls.Add(lnkRegistro);
            Controls.Add(BtnIngresar);
            Controls.Add(txtPassword);
            Controls.Add(LbPassword);
            Controls.Add(txtEmail);
            Controls.Add(LbEmail);
            Controls.Add(LbTitulo);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Margin = new Padding(3, 4, 3, 4);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormLogin";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "FormLogin";
            Load += FormLogin_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        #endregion

        private Label LbTitulo;
        private Label LbEmail;
        private TextBox txtEmail;
        private Label LbPassword;
        private TextBox txtPassword;
        private Button BtnIngresar;
        private LinkLabel lnkRegistro;
    }
}