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
            lblBienvenida.Location = new Point(109, 9);
            lblBienvenida.Name = "lblBienvenida";
            lblBienvenida.Size = new Size(154, 32);
            lblBienvenida.TabIndex = 0;
            lblBienvenida.Text = "Bienvenido!";
            // 
            // btnCerrarSesion
            // 
            btnCerrarSesion.Location = new Point(151, 255);
            btnCerrarSesion.Name = "btnCerrarSesion";
            btnCerrarSesion.Size = new Size(75, 23);
            btnCerrarSesion.TabIndex = 1;
            btnCerrarSesion.Text = "Cerrar \r\nSesion";
            btnCerrarSesion.UseVisualStyleBackColor = true;
            btnCerrarSesion.Click += BtnCerrarSesion_Click;
            // 
            // buttonGestionClubes
            // 
            buttonGestionClubes.Location = new Point(123, 80);
            buttonGestionClubes.Name = "buttonGestionClubes";
            buttonGestionClubes.Size = new Size(128, 23);
            buttonGestionClubes.TabIndex = 2;
            buttonGestionClubes.Text = "Gestion de Clubes";
            buttonGestionClubes.UseVisualStyleBackColor = true;
            buttonGestionClubes.Click += buttonClubes_Click;
            // 
            // buttonGestionEstadios
            // 
            buttonGestionEstadios.Location = new Point(123, 132);
            buttonGestionEstadios.Name = "buttonGestionEstadios";
            buttonGestionEstadios.Size = new Size(128, 23);
            buttonGestionEstadios.TabIndex = 3;
            buttonGestionEstadios.Text = "Gestion de Estadios";
            buttonGestionEstadios.UseVisualStyleBackColor = true;
            buttonGestionEstadios.Click += btnEstadios_Click;
            // 
            // buttonGestionUsuarios
            // 
            buttonGestionUsuarios.Location = new Point(123, 195);
            buttonGestionUsuarios.Name = "buttonGestionUsuarios";
            buttonGestionUsuarios.Size = new Size(128, 23);
            buttonGestionUsuarios.TabIndex = 4;
            buttonGestionUsuarios.Text = "Gestion de Usuarios";
            buttonGestionUsuarios.UseVisualStyleBackColor = true;
            buttonGestionUsuarios.Click += btnUsuarios_Click;
            // 
            // FormPrincipal
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(374, 325);
            Controls.Add(buttonGestionUsuarios);
            Controls.Add(buttonGestionEstadios);
            Controls.Add(buttonGestionClubes);
            Controls.Add(btnCerrarSesion);
            Controls.Add(lblBienvenida);
            Name = "FormPrincipal";
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