using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using QRCoder;

namespace Kicket.WinForms.Forms
{
    public partial class FormTicketQr : Form
    {
        public FormTicketQr()
        {
            InitializeComponent();
        }

        public void CargarDatos(int entradaId, string eventoNombre, string sectorNombre, string ubicacion, string codigoQr)
        {
            labelHeaderTitle.Text = $"Entrada Digital #{entradaId}";
            labelEvento.Text = eventoNombre;
            labelSectorUbicacion.Text = $"{sectorNombre} | {ubicacion}";
            textBoxCodigoQr.Text = codigoQr;

            GenerarYMostrarQr(codigoQr);
        }

        private void GenerarYMostrarQr(string payload)
        {
            if (string.IsNullOrWhiteSpace(payload))
            {
                pictureBoxQr.Image = null;
                return;
            }

            try
            {
                using var qrGenerator = new QRCodeGenerator();
                using var qrCodeData = qrGenerator.CreateQrCode(payload, QRCodeGenerator.ECCLevel.Q);
                using var pngByteCode = new PngByteQRCode(qrCodeData);
                byte[] qrBytes = pngByteCode.GetGraphic(10);

                using var ms = new MemoryStream(qrBytes);
                pictureBoxQr.Image = Image.FromStream(ms);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"No se pudo generar el código QR: {ex.Message}", "Error al generar QR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCopiarCodigo_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(textBoxCodigoQr.Text))
            {
                Clipboard.SetText(textBoxCodigoQr.Text);
                MessageBox.Show("Código alfanumérico copiado al portapapeles.", "Copiado", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
