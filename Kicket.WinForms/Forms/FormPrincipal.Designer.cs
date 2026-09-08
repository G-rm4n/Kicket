namespace Kicket.WinForms.Forms
{
    partial class FormPrincipal
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
            lblBienvenida = new Label();
            btnCerrarSesion = new Button();
            buttonGestionClubes = new Button();
            buttonGestionEstadios = new Button();
            buttonGestionUsuarios = new Button();
            SuspendLayout();
            // 
            // lblBienvenida
            // 
            lblBienvenida.AutoSize = true;
            lblBienvenida.Font = new Font("Segoe UI Black", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblBienvenida.Location = new Point(12, 9);
            lblBienvenida.Name = "lblBienvenida";
            lblBienvenida.Size = new Size(196, 41);
            lblBienvenida.TabIndex = 0;
            lblBienvenida.Text = "Bienvenido!";
            // 
            // btnCerrarSesion
            // 
            btnCerrarSesion.Location = new Point(173, 340);
            btnCerrarSesion.Margin = new Padding(3, 4, 3, 4);
            btnCerrarSesion.Name = "btnCerrarSesion";
            btnCerrarSesion.Size = new Size(86, 31);
            btnCerrarSesion.TabIndex = 1;
            btnCerrarSesion.Text = "Cerrar \r\nSesion";
            btnCerrarSesion.UseVisualStyleBackColor = true;
            btnCerrarSesion.Click += BtnCerrarSesion_Click;
            // 
            // buttonGestionClubes
            // 
            buttonGestionClubes.Location = new Point(123, 97);
            buttonGestionClubes.Margin = new Padding(3, 4, 3, 4);
            buttonGestionClubes.Name = "buttonGestionClubes";
            buttonGestionClubes.Size = new Size(198, 31);
            buttonGestionClubes.TabIndex = 2;
            buttonGestionClubes.Text = "Gestion de Clubes";
            buttonGestionClubes.UseVisualStyleBackColor = true;
            buttonGestionClubes.Click += buttonClubes_Click;
            // 
            // buttonGestionEstadios
            // 
            buttonGestionEstadios.Location = new Point(123, 166);
            buttonGestionEstadios.Margin = new Padding(3, 4, 3, 4);
            buttonGestionEstadios.Name = "buttonGestionEstadios";
            buttonGestionEstadios.Size = new Size(198, 31);
            buttonGestionEstadios.TabIndex = 3;
            buttonGestionEstadios.Text = "Gestion de Estadios";
            buttonGestionEstadios.UseVisualStyleBackColor = true;
            buttonGestionEstadios.Click += btnEstadios_Click;
            // 
            // buttonGestionUsuarios
            // 
            buttonGestionUsuarios.Location = new Point(123, 250);
            buttonGestionUsuarios.Margin = new Padding(3, 4, 3, 4);
            buttonGestionUsuarios.Name = "buttonGestionUsuarios";
            buttonGestionUsuarios.Size = new Size(198, 31);
            buttonGestionUsuarios.TabIndex = 4;
            buttonGestionUsuarios.Text = "Gestion de Usuarios";
            buttonGestionUsuarios.UseVisualStyleBackColor = true;
            buttonGestionUsuarios.Click += btnUsuarios_Click;
            // 
            // FormPrincipal
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            ClientSize = new Size(427, 433);
            Controls.Add(buttonGestionUsuarios);
            Controls.Add(buttonGestionEstadios);
            Controls.Add(buttonGestionClubes);
            Controls.Add(btnCerrarSesion);
            Controls.Add(lblBienvenida);
            Margin = new Padding(3, 4, 3, 4);
            MaximizeBox = false;
            Name = "FormPrincipal";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "FormPrincipal";
            Load += FormPrincipal_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblBienvenida;
        private Button btnCerrarSesion;
        private Button buttonGestionClubes;
        private Button buttonGestionEstadios;
        private Button buttonGestionUsuarios;
    }
}