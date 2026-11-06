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
            menuPrincipal = new MenuStrip();
            menuAdministracion = new ToolStripMenuItem();
            menuClubes = new ToolStripMenuItem();
            menuEstadios = new ToolStripMenuItem();
            menuEventos = new ToolStripMenuItem();
            menuUsuarios = new ToolStripMenuItem();
            separadorAdministracion = new ToolStripSeparator();
            menuCompras = new ToolStripMenuItem();
            menuReportes = new ToolStripMenuItem();
            menuSesion = new ToolStripMenuItem();
            menuUsuarioActual = new ToolStripMenuItem();
            separadorSesion = new ToolStripSeparator();
            menuCerrarSesion = new ToolStripMenuItem();
            lblBienvenida = new Label();
            lblSubtitulo = new Label();
            lblAyuda = new Label();
            menuPrincipal.SuspendLayout();
            SuspendLayout();
            //
            // menuPrincipal
            //
            menuPrincipal.ImageScalingSize = new Size(20, 20);
            menuPrincipal.Items.AddRange(new ToolStripItem[] { menuAdministracion, menuSesion });
            menuPrincipal.Location = new Point(0, 0);
            menuPrincipal.Name = "menuPrincipal";
            menuPrincipal.Size = new Size(560, 28);
            menuPrincipal.TabIndex = 0;
            //
            // menuAdministracion
            //
            menuAdministracion.DropDownItems.AddRange(new ToolStripItem[] { menuClubes, menuEstadios, menuEventos, menuUsuarios, separadorAdministracion, menuCompras, menuReportes });
            menuAdministracion.Name = "menuAdministracion";
            menuAdministracion.Size = new Size(124, 24);
            menuAdministracion.Text = "&Administración";
            //
            // menuClubes
            //
            menuClubes.Name = "menuClubes";
            menuClubes.ShortcutKeys = Keys.Control | Keys.D1;
            menuClubes.Size = new Size(300, 26);
            menuClubes.Text = "Clubes";
            menuClubes.Click += buttonClubes_Click;
            //
            // menuEstadios
            //
            menuEstadios.Name = "menuEstadios";
            menuEstadios.ShortcutKeys = Keys.Control | Keys.D2;
            menuEstadios.Size = new Size(300, 26);
            menuEstadios.Text = "Estadios y sectores";
            menuEstadios.Click += btnEstadios_Click;
            //
            // menuEventos
            //
            menuEventos.Name = "menuEventos";
            menuEventos.ShortcutKeys = Keys.Control | Keys.D3;
            menuEventos.Size = new Size(300, 26);
            menuEventos.Text = "Eventos / Partidos";
            menuEventos.Click += btnEventos_Click;
            //
            // menuUsuarios
            //
            menuUsuarios.Name = "menuUsuarios";
            menuUsuarios.ShortcutKeys = Keys.Control | Keys.D4;
            menuUsuarios.Size = new Size(300, 26);
            menuUsuarios.Text = "Usuarios";
            menuUsuarios.Click += btnUsuarios_Click;
            //
            // separadorAdministracion
            //
            separadorAdministracion.Name = "separadorAdministracion";
            separadorAdministracion.Size = new Size(297, 6);
            //
            // menuCompras
            //
            menuCompras.Name = "menuCompras";
            menuCompras.ShortcutKeys = Keys.Control | Keys.D5;
            menuCompras.Size = new Size(300, 26);
            menuCompras.Text = "Compras y entradas";
            menuCompras.Click += btnCompras_Click;
            //
            // menuReportes
            //
            menuReportes.Name = "menuReportes";
            menuReportes.ShortcutKeys = Keys.Control | Keys.D6;
            menuReportes.Size = new Size(300, 26);
            menuReportes.Text = "Reportes de ventas";
            menuReportes.Click += btnReportes_Click;
            //
            // menuSesion
            //
            menuSesion.DropDownItems.AddRange(new ToolStripItem[] { menuUsuarioActual, separadorSesion, menuCerrarSesion });
            menuSesion.Name = "menuSesion";
            menuSesion.Size = new Size(68, 24);
            menuSesion.Text = "&Sesión";
            //
            // menuUsuarioActual
            //
            menuUsuarioActual.Enabled = false;
            menuUsuarioActual.Name = "menuUsuarioActual";
            menuUsuarioActual.Size = new Size(260, 26);
            menuUsuarioActual.Text = "Usuario";
            //
            // separadorSesion
            //
            separadorSesion.Name = "separadorSesion";
            separadorSesion.Size = new Size(257, 6);
            //
            // menuCerrarSesion
            //
            menuCerrarSesion.Name = "menuCerrarSesion";
            menuCerrarSesion.ShortcutKeys = Keys.Control | Keys.Q;
            menuCerrarSesion.Size = new Size(260, 26);
            menuCerrarSesion.Text = "Cerrar sesión";
            menuCerrarSesion.Click += BtnCerrarSesion_Click;
            //
            // lblBienvenida
            //
            lblBienvenida.AutoSize = true;
            lblBienvenida.Font = new Font("Segoe UI Black", 16F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblBienvenida.Location = new Point(28, 48);
            lblBienvenida.Name = "lblBienvenida";
            lblBienvenida.Size = new Size(185, 37);
            lblBienvenida.TabIndex = 1;
            lblBienvenida.Text = "¡Bienvenido!";
            //
            // lblSubtitulo
            //
            lblSubtitulo.AutoSize = true;
            lblSubtitulo.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblSubtitulo.ForeColor = SystemColors.ControlDarkDark;
            lblSubtitulo.Location = new Point(32, 88);
            lblSubtitulo.Name = "lblSubtitulo";
            lblSubtitulo.Size = new Size(275, 23);
            lblSubtitulo.TabIndex = 2;
            lblSubtitulo.Text = "Panel de Administración - Kicket";
            //
            // lblAyuda
            //
            lblAyuda.Location = new Point(32, 130);
            lblAyuda.Name = "lblAyuda";
            lblAyuda.Size = new Size(500, 60);
            lblAyuda.TabIndex = 3;
            lblAyuda.Text = "Usá el menú Administración para abrir cada sección (atajos Ctrl+1 a Ctrl+6). Para salir, Sesión > Cerrar sesión (Ctrl+Q).";
            //
            // FormPrincipal
            //
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(560, 210);
            Controls.Add(lblAyuda);
            Controls.Add(lblSubtitulo);
            Controls.Add(lblBienvenida);
            Controls.Add(menuPrincipal);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MainMenuStrip = menuPrincipal;
            MaximizeBox = false;
            Name = "FormPrincipal";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Kicket - Panel de Administración";
            Load += FormPrincipal_Load;
            menuPrincipal.ResumeLayout(false);
            menuPrincipal.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuPrincipal = null!;
        private ToolStripMenuItem menuAdministracion = null!;
        private ToolStripMenuItem menuClubes = null!;
        private ToolStripMenuItem menuEstadios = null!;
        private ToolStripMenuItem menuEventos = null!;
        private ToolStripMenuItem menuUsuarios = null!;
        private ToolStripSeparator separadorAdministracion = null!;
        private ToolStripMenuItem menuCompras = null!;
        private ToolStripMenuItem menuReportes = null!;
        private ToolStripMenuItem menuSesion = null!;
        private ToolStripMenuItem menuUsuarioActual = null!;
        private ToolStripSeparator separadorSesion = null!;
        private ToolStripMenuItem menuCerrarSesion = null!;
        private Label lblBienvenida = null!;
        private Label lblSubtitulo = null!;
        private Label lblAyuda = null!;
    }
}
