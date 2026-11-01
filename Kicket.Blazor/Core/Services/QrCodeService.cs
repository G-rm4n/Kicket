using System;
using QRCoder;

namespace Kicket.Blazor.Core.Services
{
    // Servicio estático que convierte un texto en una imagen QR para mostrar en el navegador.
    public static class QrCodeService
    {
        // Genera un QR en SVG a partir del texto y lo devuelve como data URI (para usar en <img src>).
        public static string GenerarQrSvgDataUri(string payload)
        {
            // Sin texto no hay QR.
            if (string.IsNullOrWhiteSpace(payload))
            {
                return string.Empty;
            }

            // Crea el generador, codifica el texto con corrección de errores nivel Q y lo dibuja en SVG.
            using var qrGenerator = new QRCodeGenerator();
            using var qrCodeData = qrGenerator.CreateQrCode(payload, QRCodeGenerator.ECCLevel.Q);
            using var svgCode = new SvgQRCode(qrCodeData);
            string svg = svgCode.GetGraphic(8);
            // Lo codifica para que el navegador lo pueda usar directamente como imagen.
            return $"data:image/svg+xml;utf8,{Uri.EscapeDataString(svg)}";
        }
    }
}
