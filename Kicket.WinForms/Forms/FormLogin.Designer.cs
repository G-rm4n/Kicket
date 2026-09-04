

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
            SuspendLayout();
            // 
            // LbTitulo
            // 
            LbTitulo.AutoSize = true;
            LbTitulo.Location = new Point(12, 9);
            LbTitulo.Name = "LbTitulo";
            LbTitulo.Size = new Size(72, 15);
            LbTitulo.TabIndex = 0;
            LbTitulo.Text = "Login Kicket";
            LbTitulo.TextAlign = ContentAlignment.MiddleCenter;
            LbTitulo.Click += label1_Click;
            // 
            // LbEmail
            // 
            LbEmail.AutoSize = true;
            LbEmail.Location = new Point(12, 59);
            LbEmail.Name = "LbEmail";
            LbEmail.Size = new Size(36, 15);
            LbEmail.TabIndex = 1;
            LbEmail.Text = "Email";
            LbEmail.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // txtEmail
            // 
            txtEmail.Location = new Point(99, 51);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(192, 23);
            txtEmail.TabIndex = 2;
            txtEmail.Text = "Ingrese email aqui";
            // 
            // LbPassword
            // 
            LbPassword.AutoSize = true;
            LbPassword.Location = new Point(12, 109);
            LbPassword.Name = "LbPassword";
            LbPassword.Size = new Size(67, 15);
            LbPassword.TabIndex = 3;
            LbPassword.Text = "Contraseña";
            // 
            // txtPassword
            // 
            txtPassword.Location = new Point(99, 101);
            txtPassword.Name = "txtPassword";
            txtPassword.Size = new Size(100, 23);
            txtPassword.TabIndex = 4;
            txtPassword.UseSystemPasswordChar = true;
            // 
            // BtnIngresar
            // 
            BtnIngresar.Location = new Point(12, 182);
            BtnIngresar.Name = "BtnIngresar";
            BtnIngresar.Size = new Size(100, 41);
            BtnIngresar.TabIndex = 5;
            BtnIngresar.Text = "Ingresar";
            BtnIngresar.UseVisualStyleBackColor = true;
            // 
            // FormLogin
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(922, 607);
            Controls.Add(BtnIngresar);
            Controls.Add(txtPassword);
            Controls.Add(LbPassword);
            Controls.Add(txtEmail);
            Controls.Add(LbEmail);
            Controls.Add(LbTitulo);
            Name = "FormLogin";
            Text = "FormLogin";
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
    }
}