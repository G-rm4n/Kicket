using System.Drawing;
using System.Windows.Forms;

namespace Kicket.WinForms.Forms
{
    partial class FormTicketQr
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
            panelHeader = new Panel();
            labelHeaderSub = new Label();
            labelHeaderTitle = new Label();
            pictureBoxQr = new PictureBox();
            labelEvento = new Label();
            labelSectorUbicacion = new Label();
            labelCodigoTexto = new Label();
            textBoxCodigoQr = new TextBox();
            btnCopiarCodigo = new Button();
            btnCerrar = new Button();
            labelAviso = new Label();
            panelHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxQr).BeginInit();
            SuspendLayout();
            // 
            // panelHeader
            // 
            panelHeader.BackColor = Color.FromArgb(24, 28, 36);
            panelHeader.Controls.Add(labelHeaderSub);
            panelHeader.Controls.Add(labelHeaderTitle);
            panelHeader.Dock = DockStyle.Top;
            panelHeader.Location = new Point(0, 0);
            panelHeader.Name = "panelHeader";
            panelHeader.Size = new Size(460, 75);
            panelHeader.TabIndex = 0;
            // 
            // labelHeaderSub
            // 
            labelHeaderSub.AutoSize = true;
            labelHeaderSub.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            labelHeaderSub.ForeColor = Color.FromArgb(46, 204, 113);
            labelHeaderSub.Location = new Point(20, 14);
            labelHeaderSub.Name = "labelHeaderSub";
            labelHeaderSub.Size = new Size(164, 19);
            labelHeaderSub.TabIndex = 0;
            labelHeaderSub.Text = "KICKET ACCESS PASS";
            // 
            // labelHeaderTitle
            // 
            labelHeaderTitle.AutoSize = true;
            labelHeaderTitle.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            labelHeaderTitle.ForeColor = Color.White;
            labelHeaderTitle.Location = new Point(18, 33);
            labelHeaderTitle.Name = "labelHeaderTitle";
            labelHeaderTitle.Size = new Size(270, 30);
            labelHeaderTitle.TabIndex = 1;
            labelHeaderTitle.Text = "Ticket Digital de Entrada";
            // 
            // pictureBoxQr
            // 
            pictureBoxQr.BackColor = Color.White;
            pictureBoxQr.BorderStyle = BorderStyle.FixedSingle;
            pictureBoxQr.Location = new Point(110, 95);
            pictureBoxQr.Name = "pictureBoxQr";
            pictureBoxQr.Size = new Size(240, 240);
            pictureBoxQr.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxQr.TabIndex = 1;
            pictureBoxQr.TabStop = false;
            // 
            // labelEvento
            // 
            labelEvento.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            labelEvento.ForeColor = Color.FromArgb(20, 20, 20);
            labelEvento.Location = new Point(20, 350);
            labelEvento.Name = "labelEvento";
            labelEvento.Size = new Size(420, 28);
            labelEvento.TabIndex = 2;
            labelEvento.Text = "Nombre del Evento";
            labelEvento.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // labelSectorUbicacion
            // 
            labelSectorUbicacion.Font = new Font("Segoe UI", 9.5F);
            labelSectorUbicacion.ForeColor = Color.FromArgb(80, 80, 80);
            labelSectorUbicacion.Location = new Point(20, 380);
            labelSectorUbicacion.Name = "labelSectorUbicacion";
            labelSectorUbicacion.Size = new Size(420, 25);
            labelSectorUbicacion.TabIndex = 3;
            labelSectorUbicacion.Text = "Sector | Ubicación";
            labelSectorUbicacion.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // labelCodigoTexto
            // 
            labelCodigoTexto.AutoSize = true;
            labelCodigoTexto.Font = new Font("Segoe UI", 8.5F, FontStyle.Regular);
            labelCodigoTexto.ForeColor = Color.FromArgb(100, 100, 100);
            labelCodigoTexto.Location = new Point(25, 418);
            labelCodigoTexto.Name = "labelCodigoTexto";
            labelCodigoTexto.Size = new Size(137, 20);
            labelCodigoTexto.TabIndex = 4;
            labelCodigoTexto.Text = "Código del Molinete:";
            // 
            // textBoxCodigoQr
            // 
            textBoxCodigoQr.BackColor = Color.FromArgb(245, 245, 245);
            textBoxCodigoQr.Font = new Font("Consolas", 10F, FontStyle.Bold);
            textBoxCodigoQr.Location = new Point(25, 442);
            textBoxCodigoQr.Name = "textBoxCodigoQr";
            textBoxCodigoQr.ReadOnly = true;
            textBoxCodigoQr.Size = new Size(310, 27);
            textBoxCodigoQr.TabIndex = 5;
            // 
            // btnCopiarCodigo
            // 
            btnCopiarCodigo.BackColor = Color.FromArgb(235, 238, 242);
            btnCopiarCodigo.Location = new Point(345, 440);
            btnCopiarCodigo.Name = "btnCopiarCodigo";
            btnCopiarCodigo.Size = new Size(90, 30);
            btnCopiarCodigo.TabIndex = 6;
            btnCopiarCodigo.Text = "Copiar";
            btnCopiarCodigo.UseVisualStyleBackColor = false;
            btnCopiarCodigo.Click += btnCopiarCodigo_Click;
            // 
            // btnCerrar
            // 
            btnCerrar.BackColor = Color.FromArgb(35, 40, 50);
            btnCerrar.FlatStyle = FlatStyle.Flat;
            btnCerrar.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnCerrar.ForeColor = Color.White;
            btnCerrar.Location = new Point(155, 520);
            btnCerrar.Name = "btnCerrar";
            btnCerrar.Size = new Size(150, 38);
            btnCerrar.TabIndex = 7;
            btnCerrar.Text = "Cerrar";
            btnCerrar.UseVisualStyleBackColor = false;
            btnCerrar.Click += btnCerrar_Click;
            // 
            // labelAviso
            // 
            labelAviso.Font = new Font("Segoe UI", 8F);
            labelAviso.ForeColor = Color.FromArgb(110, 110, 110);
            labelAviso.Location = new Point(25, 480);
            labelAviso.Name = "labelAviso";
            labelAviso.Size = new Size(410, 32);
            labelAviso.TabIndex = 8;
            labelAviso.Text = "Código óptico válido para acceso en puerta. Escanear con lector 2D.";
            labelAviso.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // FormTicketQr
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(460, 575);
            Controls.Add(labelAviso);
            Controls.Add(btnCerrar);
            Controls.Add(btnCopiarCodigo);
            Controls.Add(textBoxCodigoQr);
            Controls.Add(labelCodigoTexto);
            Controls.Add(labelSectorUbicacion);
            Controls.Add(labelEvento);
            Controls.Add(pictureBoxQr);
            Controls.Add(panelHeader);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormTicketQr";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Pase de Acceso - Código QR";
            panelHeader.ResumeLayout(false);
            panelHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxQr).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel panelHeader;
        private Label labelHeaderSub;
        private Label labelHeaderTitle;
        private PictureBox pictureBoxQr;
        private Label labelEvento;
        private Label labelSectorUbicacion;
        private Label labelCodigoTexto;
        private TextBox textBoxCodigoQr;
        private Button btnCopiarCodigo;
        private Button btnCerrar;
        private Label labelAviso;
    }
}
