using ClosedXML.Excel;
using Kicket.Contracts.Reportes;

namespace Core.Reportes
{
    /// <summary>
    /// Genera el reporte de ventas en .xlsx con ClosedXML (licencia MIT).
    /// Una hoja de resumen y una hoja por tabla, con formato de fecha y moneda, totales y autofiltro.
    /// ClosedXML no sabe crear graficos de Excel: los graficos quedan en la web y en el PDF.
    /// </summary>
    public static class ExportadorExcel
    {
        private const string FormatoMoneda = "\"$\" #,##0.00";
        private const string FormatoEntero = "#,##0";

        public static byte[] Generar(ReporteVentasDto r)
        {
            using var libro = new XLWorkbook();

            // --- Hoja Resumen
            var hoja = libro.Worksheets.Add("Resumen");
            hoja.Cell(1, 1).Value = "Kicket – Reporte de ventas";
            hoja.Cell(1, 1).Style.Font.SetBold().Font.SetFontSize(16);
            hoja.Cell(2, 1).Value = "Generado";
            hoja.Cell(2, 2).Value = r.GeneradoEn;
            hoja.Cell(2, 2).Style.DateFormat.Format = "dd/mm/yyyy hh:mm";
            hoja.Cell(3, 1).Value = "Desde";
            hoja.Cell(3, 2).Value = r.Desde.HasValue ? r.Desde.Value.ToString("dd/MM/yyyy") : "Sin límite";
            hoja.Cell(4, 1).Value = "Hasta";
            hoja.Cell(4, 2).Value = r.Hasta.HasValue ? r.Hasta.Value.ToString("dd/MM/yyyy") : "Sin límite";
            hoja.Cell(5, 1).Value = "Partido";
            hoja.Cell(5, 2).Value = r.EventoNombre ?? "Todos";

            var indicadores = new (string Nombre, object Valor, string Formato)[]
            {
                ("Total recaudado", r.Resumen.TotalRecaudado, FormatoMoneda),
                ("Entradas vendidas", r.Resumen.EntradasVendidas, FormatoEntero),
                ("Compras confirmadas", r.Resumen.ComprasConfirmadas, FormatoEntero),
                ("Ticket promedio", r.Resumen.TicketPromedio, FormatoMoneda),
                ("Partidos con ventas", r.Resumen.EventosConVentas, FormatoEntero),
                ("Partidos cargados", r.Resumen.EventosTotales, FormatoEntero),
                ("Compradores distintos", r.Resumen.CompradoresDistintos, FormatoEntero),
                ("Compras no confirmadas (no suman)", r.Resumen.ComprasNoConfirmadas, FormatoEntero)
            };
            hoja.Cell(7, 1).Value = "Indicador";
            hoja.Cell(7, 2).Value = "Valor";
            EstiloCabecera(hoja.Range(7, 1, 7, 2));
            for (int i = 0; i < indicadores.Length; i++)
            {
                hoja.Cell(8 + i, 1).Value = indicadores[i].Nombre;
                hoja.Cell(8 + i, 2).Value = XLCellValue.FromObject(indicadores[i].Valor);
                hoja.Cell(8 + i, 2).Style.NumberFormat.Format = indicadores[i].Formato;
            }
            hoja.Cell(9 + indicadores.Length, 1).Value = ReporteVentasDto.CriterioVentas;
            hoja.Cell(9 + indicadores.Length, 1).Style.Font.SetItalic();
            hoja.Column(1).Width = 38;
            hoja.Column(2).Width = 22;

            // --- Hojas de detalle
            AgregarTabla(libro, "Por partido",
                new[] { "Partido", "Fecha", "Estadio", "Compras", "Entradas", "Recaudado" },
                r.PorEvento.Select(e => new object[] { e.Evento, e.Fecha, e.Estadio, e.Compras, e.Entradas, e.Recaudado }),
                new[] { "", "dd/mm/yyyy hh:mm", "", FormatoEntero, FormatoEntero, FormatoMoneda },
                columnasConTotal: new[] { 4, 5, 6 });

            AgregarTabla(libro, "Por club",
                new[] { "Club", "Partidos como local", "Partidos como visitante", "Entradas (local)", "Recaudado (local)" },
                r.PorClub.Select(c => new object[] { c.Club, c.EventosComoLocal, c.EventosComoVisitante, c.Entradas, c.Recaudado }),
                new[] { "", FormatoEntero, FormatoEntero, FormatoEntero, FormatoMoneda },
                columnasConTotal: new[] { 4, 5 });

            AgregarTabla(libro, "Por sector",
                new[] { "Estadio", "Sector", "Capacidad por partido", "Entradas", "Recaudado" },
                r.PorSector.Select(s => new object[] { s.Estadio, s.Sector, s.CapacidadPorEvento, s.Entradas, s.Recaudado }),
                new[] { "", "", FormatoEntero, FormatoEntero, FormatoMoneda },
                columnasConTotal: new[] { 4, 5 });

            AgregarTabla(libro, "Por día",
                new[] { "Día", "Compras", "Entradas", "Recaudado" },
                r.PorPeriodo.Select(p => new object[] { p.Dia, p.Compras, p.Entradas, p.Recaudado }),
                new[] { "dd/mm/yyyy", FormatoEntero, FormatoEntero, FormatoMoneda },
                columnasConTotal: new[] { 2, 3, 4 });

            using var memoria = new MemoryStream();
            libro.SaveAs(memoria);
            return memoria.ToArray();
        }

        // Hoja con cabecera, datos tipados (fechas y numeros reales, no texto), autofiltro y fila de totales con SUM.
        private static void AgregarTabla(XLWorkbook libro, string nombre, string[] cabeceras,
            IEnumerable<object[]> filas, string[] formatos, int[] columnasConTotal)
        {
            var hoja = libro.Worksheets.Add(nombre);
            for (int c = 0; c < cabeceras.Length; c++)
                hoja.Cell(1, c + 1).Value = cabeceras[c];
            EstiloCabecera(hoja.Range(1, 1, 1, cabeceras.Length));

            int fila = 2;
            foreach (var datos in filas)
            {
                for (int c = 0; c < datos.Length; c++)
                {
                    var celda = hoja.Cell(fila, c + 1);
                    celda.Value = XLCellValue.FromObject(datos[c]);
                    if (!string.IsNullOrEmpty(formatos[c]))
                    {
                        if (datos[c] is DateTime) celda.Style.DateFormat.Format = formatos[c];
                        else celda.Style.NumberFormat.Format = formatos[c];
                    }
                }
                fila++;
            }
            int ultimaFilaDatos = fila - 1;

            if (ultimaFilaDatos >= 2)
                hoja.Range(1, 1, ultimaFilaDatos, cabeceras.Length).SetAutoFilter();

            // Totales con formula, asi el usuario puede filtrar y el archivo sigue siendo coherente.
            hoja.Cell(fila, 1).Value = "Total";
            foreach (int c in columnasConTotal)
            {
                var celda = hoja.Cell(fila, c);
                celda.FormulaA1 = ultimaFilaDatos >= 2
                    ? $"SUM({hoja.Cell(2, c).Address}:{hoja.Cell(ultimaFilaDatos, c).Address})"
                    : "0";
                celda.Style.NumberFormat.Format = formatos[c - 1];
            }
            var filaTotal = hoja.Range(fila, 1, fila, cabeceras.Length);
            filaTotal.Style.Font.SetBold();
            filaTotal.Style.Fill.SetBackgroundColor(XLColor.FromHtml("#E8EEF6"));

            hoja.SheetView.FreezeRows(1);
            hoja.Columns().AdjustToContents();
        }

        private static void EstiloCabecera(IXLRange rango)
        {
            rango.Style.Font.SetBold().Font.SetFontColor(XLColor.White);
            rango.Style.Fill.SetBackgroundColor(XLColor.FromHtml("#1F6FB2"));
        }
    }
}
