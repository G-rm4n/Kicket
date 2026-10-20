using System.Drawing;
using System.Windows.Forms;

namespace Kicket.WinForms.Forms
{
    partial class FormPrincipal
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            lblBienvenida = new Label();
            lblSubtitulo = new Label();
            buttonGestionClubes = new Button();
            buttonGestionEstadios = new Button();
            buttonGestionEventos = new Button();
            buttonGestionUsuarios = new Button();
            buttonGestionCompras = new Button();
            btnCerrarSesion = new Button();
            SuspendLayout();
            // 
            // lblBienvenida
            // 
            lblBienvenida.AutoSize = true;
            lblBienvenida.Font = new Font("Segoe UI Black", 16F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblBienvenida.Location = new Point(28, 18);
            lblBienvenida.Name = "lblBienvenida";
            lblBienvenida.Size = new Size(185, 37);
            lblBienvenida.TabIndex = 0;
            lblBienvenida.Text = "¡Bienvenido!";
            // 
            // lblSubtitulo
            // 
            lblSubtitulo.AutoSize = true;
            lblSubtitulo.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblSubtitulo.ForeColor = SystemColors.ControlDarkDark;
            lblSubtitulo.Location = new Point(32, 58);
            lblSubtitulo.Name = "lblSubtitulo";
            lblSubtitulo.Size = new Size(275, 23);
            lblSubtitulo.TabIndex = 1;
            lblSubtitulo.Text = "Panel de Administración - Kicket";
            // 
            // buttonGestionClubes
            // 
            buttonGestionClubes.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            buttonGestionClubes.Location = new Point(85, 100);
            buttonGestionClubes.Name = "buttonGestionClubes";
            buttonGestionClubes.Size = new Size(280, 40);
            buttonGestionClubes.TabIndex = 2;
            buttonGestionClubes.Text = "Gestión de Clubes";
            buttonGestionClubes.UseVisualStyleBackColor = true;
            buttonGestionClubes.Click += buttonClubes_Click;
            // 
            // buttonGestionEstadios
            // 
            buttonGestionEstadios.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            buttonGestionEstadios.Location = new Point(85, 152);
            buttonGestionEstadios.Name = "buttonGestionEstadios";
            buttonGestionEstadios.Size = new Size(280, 40);
            buttonGestionEstadios.TabIndex = 3;
            buttonGestionEstadios.Text = "Gestión de Estadios y Sectores";
            buttonGestionEstadios.UseVisualStyleBackColor = true;
            buttonGestionEstadios.Click += btnEstadios_Click;
            // 
            // buttonGestionEventos
            // 
            buttonGestionEventos.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            buttonGestionEventos.Location = new Point(85, 204);
            buttonGestionEventos.Name = "buttonGestionEventos";
            buttonGestionEventos.Size = new Size(280, 40);
            buttonGestionEventos.TabIndex = 4;
            buttonGestionEventos.Text = "Gestión de Eventos / Partidos";
            buttonGestionEventos.UseVisualStyleBackColor = true;
            buttonGestionEventos.Click += btnEventos_Click;
            // 
            // buttonGestionUsuarios
            // 
            buttonGestionUsuarios.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            buttonGestionUsuarios.Location = new Point(85, 256);
            buttonGestionUsuarios.Name = "buttonGestionUsuarios";
            buttonGestionUsuarios.Size = new Size(280, 40);
            buttonGestionUsuarios.TabIndex = 5;
            buttonGestionUsuarios.Text = "Gestión de Usuarios";
            buttonGestionUsuarios.UseVisualStyleBackColor = true;
            buttonGestionUsuarios.Click += btnUsuarios_Click;
            // 
            // buttonGestionCompras
            // 
            buttonGestionCompras.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            buttonGestionCompras.Location = new Point(85, 308);
            buttonGestionCompras.Name = "buttonGestionCompras";
            buttonGestionCompras.Size = new Size(280, 40);
            buttonGestionCompras.TabIndex = 6;
            buttonGestionCompras.Text = "Auditoría de Compras y Entradas";
            buttonGestionCompras.UseVisualStyleBackColor = true;
            buttonGestionCompras.Click += btnCompras_Click;
            // 
            // btnCerrarSesion
            // 
            btnCerrarSesion.Location = new Point(155, 375);
            btnCerrarSesion.Name = "btnCerrarSesion";
            btnCerrarSesion.Size = new Size(140, 36);
            btnCerrarSesion.TabIndex = 7;
            btnCerrarSesion.Text = "Cerrar Sesión";
            btnCerrarSesion.UseVisualStyleBackColor = true;
            btnCerrarSesion.Click += BtnCerrarSesion_Click;
            // 
            // FormPrincipal
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(450, 440);
            Controls.Add(buttonGestionCompras);
            Controls.Add(buttonGestionEventos);
            Controls.Add(buttonGestionUsuarios);
            Controls.Add(buttonGestionEstadios);
            Controls.Add(buttonGestionClubes);
            Controls.Add(btnCerrarSesion);
            Controls.Add(lblSubtitulo);
            Controls.Add(lblBienvenida);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "FormPrincipal";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Kicket - Panel de Administración";
            Load += FormPrincipal_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblBienvenida = null!;
        private Label lblSubtitulo = null!;
        private Button btnCerrarSesion = null!;
        private Button buttonGestionClubes = null!;
        private Button buttonGestionEstadios = null!;
        private Button buttonGestionEventos = null!;
        private Button buttonGestionUsuarios = null!;
        private Button buttonGestionCompras = null!;
    }
}
