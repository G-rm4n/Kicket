using System;
using QRCoder;

namespace Kicket.Blazor.Core.Services
{
    public static class QrCodeService
    {
        public static string GenerarQrSvgDataUri(string payload)
        {
            if (string.IsNullOrWhiteSpace(payload))
            {
                return string.Empty;
            }

            using var qrGenerator = new QRCodeGenerator();
            using var qrCodeData = qrGenerator.CreateQrCode(payload, QRCodeGenerator.ECCLevel.Q);
            using var svgCode = new SvgQRCode(qrCodeData);
            string svg = svgCode.GetGraphic(8);
            return $"data:image/svg+xml;utf8,{Uri.EscapeDataString(svg)}";
        }
    }
}
