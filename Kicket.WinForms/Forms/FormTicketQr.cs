using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using QRCoder;

namespace Kicket.WinForms.Forms
{
    // Formulario que muestra una entrada como ticket con su código QR.
    public partial class FormTicketQr : Form
    {
        // Constructor: crea los controles del diseñador.
        public FormTicketQr()
        {
            InitializeComponent();
        }

        // Carga los datos de la entrada en las etiquetas y genera el QR.
        public void CargarDatos(int entradaId, string eventoNombre, string sectorNombre, string ubicacion, string codigoQr)
        {
            labelHeaderTitle.Text = $"Entrada Digital #{entradaId}";
            labelEvento.Text = eventoNombre;
            labelSectorUbicacion.Text = $"{sectorNombre} | {ubicacion}";
            textBoxCodigoQr.Text = codigoQr;

            GenerarYMostrarQr(codigoQr);
        }

        // Genera la imagen QR (PNG) a partir del código y la muestra en el PictureBox.
        private void GenerarYMostrarQr(string payload)
        {
            // Sin código no hay imagen.
            if (string.IsNullOrWhiteSpace(payload))
            {
                pictureBoxQr.Image = null;
                return;
            }

            try
            {
                // Codifica el texto con corrección de errores nivel Q y lo convierte a PNG.
                using var qrGenerator = new QRCodeGenerator();
                using var qrCodeData = qrGenerator.CreateQrCode(payload, QRCodeGenerator.ECCLevel.Q);
                using var pngByteCode = new PngByteQRCode(qrCodeData);
                byte[] qrBytes = pngByteCode.GetGraphic(10);

                // Convierte los bytes en una imagen para el PictureBox.
                using var ms = new MemoryStream(qrBytes);
                pictureBoxQr.Image = Image.FromStream(ms);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"No se pudo generar el código QR: {ex.Message}", "Error al generar QR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Copia el código alfanumérico al portapapeles.
        private void btnCopiarCodigo_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(textBoxCodigoQr.Text))
            {
                Clipboard.SetText(textBoxCodigoQr.Text);
                MessageBox.Show("Código alfanumérico copiado al portapapeles.", "Copiado", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // Cierra el formulario.
        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
