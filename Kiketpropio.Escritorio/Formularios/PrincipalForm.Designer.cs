namespace Kiketpropio.Escritorio.Formularios
{
    partial class PrincipalForm
    {
        private System.ComponentModel.IContainer? components = null;

        private MenuStrip menuPrincipal;
        private ToolStripMenuItem menuAdministracion;
        private ToolStripMenuItem menuClubes;
        private ToolStripMenuItem menuEstadios;
        private ToolStripMenuItem menuUsuarios;
        private ToolStripMenuItem menuReportes;
        private ToolStripMenuItem menuReporteUsuarios;
        private ToolStripMenuItem menuReporteEstadios;
        private ToolStripMenuItem menuSesion;
        private ToolStripMenuItem menuSalir;
        private StatusStrip barraEstado;
        private ToolStripStatusLabel lblUsuario;
        private ToolStripStatusLabel lblRol;

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
            menuPrincipal = new MenuStrip();
            menuAdministracion = new ToolStripMenuItem();
            menuClubes = new ToolStripMenuItem();
            menuEstadios = new ToolStripMenuItem();
            menuUsuarios = new ToolStripMenuItem();
            menuReportes = new ToolStripMenuItem();
            menuReporteUsuarios = new ToolStripMenuItem();
            menuReporteEstadios = new ToolStripMenuItem();
            menuSesion = new ToolStripMenuItem();
            menuSalir = new ToolStripMenuItem();
            barraEstado = new StatusStrip();
            lblUsuario = new ToolStripStatusLabel();
            lblRol = new ToolStripStatusLabel();
            menuPrincipal.SuspendLayout();
            barraEstado.SuspendLayout();
            SuspendLayout();

            menuPrincipal.Items.AddRange(new ToolStripItem[] { menuAdministracion, menuReportes, menuSesion });
            menuPrincipal.Location = new Point(0, 0);
            menuPrincipal.Name = "menuPrincipal";
            menuPrincipal.Size = new Size(984, 24);
            menuPrincipal.TabIndex = 0;

            menuAdministracion.DropDownItems.AddRange(new ToolStripItem[] { menuClubes, menuEstadios, menuUsuarios });
            menuAdministracion.Name = "menuAdministracion";
            menuAdministracion.Size = new Size(109, 20);
            menuAdministracion.Text = "Administracion";

            menuClubes.Name = "menuClubes";
            menuClubes.Size = new Size(180, 22);
            menuClubes.Text = "Clubes";
            menuClubes.Click += menuClubes_Click;

            menuEstadios.Name = "menuEstadios";
            menuEstadios.Size = new Size(180, 22);
            menuEstadios.Text = "Estadios";
            menuEstadios.Click += menuEstadios_Click;

            menuUsuarios.Name = "menuUsuarios";
            menuUsuarios.Size = new Size(180, 22);
            menuUsuarios.Text = "Usuarios";
            menuUsuarios.Click += menuUsuarios_Click;

            menuReportes.DropDownItems.AddRange(new ToolStripItem[] { menuReporteUsuarios, menuReporteEstadios });
            menuReportes.Name = "menuReportes";
            menuReportes.Size = new Size(65, 20);
            menuReportes.Text = "Reportes";

            menuReporteUsuarios.Name = "menuReporteUsuarios";
            menuReporteUsuarios.Size = new Size(220, 22);
            menuReporteUsuarios.Text = "Usuarios por perfil";
            menuReporteUsuarios.Click += menuReporteUsuarios_Click;

            menuReporteEstadios.Name = "menuReporteEstadios";
            menuReporteEstadios.Size = new Size(220, 22);
            menuReporteEstadios.Text = "Estadios por ciudad";
            menuReporteEstadios.Click += menuReporteEstadios_Click;

            menuSesion.DropDownItems.AddRange(new ToolStripItem[] { menuSalir });
            menuSesion.Name = "menuSesion";
            menuSesion.Size = new Size(55, 20);
            menuSesion.Text = "Sesion";

            menuSalir.Name = "menuSalir";
            menuSalir.Size = new Size(180, 22);
            menuSalir.Text = "Salir";
            menuSalir.Click += menuSalir_Click;

            barraEstado.Items.AddRange(new ToolStripItem[] { lblUsuario, lblRol });
            barraEstado.Location = new Point(0, 639);
            barraEstado.Name = "barraEstado";
            barraEstado.Size = new Size(984, 22);
            barraEstado.TabIndex = 1;

            lblUsuario.Name = "lblUsuario";
            lblUsuario.Size = new Size(60, 17);
            lblUsuario.Text = "Usuario";

            lblRol.Name = "lblRol";
            lblRol.Size = new Size(60, 17);
            lblRol.Text = "Perfil";

            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(984, 661);
            Controls.Add(barraEstado);
            Controls.Add(menuPrincipal);
            IsMdiContainer = true;
            MainMenuStrip = menuPrincipal;
            Name = "PrincipalForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Kicket - Panel principal";
            Load += PrincipalForm_Load;
            menuPrincipal.ResumeLayout(false);
            menuPrincipal.PerformLayout();
            barraEstado.ResumeLayout(false);
            barraEstado.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
