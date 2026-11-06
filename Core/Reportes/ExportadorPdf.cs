using System.Globalization;
using Kicket.Contracts.Reportes;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Core.Reportes
{
    /// <summary>
    /// Genera el reporte de ventas en PDF con QuestPDF (licencia Community: gratuita para uso academico
    /// y para organizaciones con ingresos menores a 1 millon de USD al año).
    /// Los graficos son los mismos SVG que muestra la web, dibujados por QuestPDF como vectores.
    /// </summary>
    public static class ExportadorPdf
    {
        private static readonly CultureInfo Ar = CultureInfo.GetCultureInfo("es-AR");
        private const string ColorTitulo = "#1F3864";
        private const string ColorCabecera = "#1F6FB2";

        static ExportadorPdf()
        {
            QuestPDF.Settings.License = LicenseType.Community;
        }

        public static byte[] Generar(ReporteVentasDto r)
        {
            var documento = Document.Create(contenedor =>
            {
                contenedor.Page(pagina =>
                {
                    pagina.Size(PageSizes.A4);
                    pagina.Margin(32);
                    pagina.DefaultTextStyle(t => t.FontSize(9.5f).FontColor("#222222"));

                    pagina.Header().Column(col =>
                    {
                        col.Item().Row(fila =>
                        {
                            fila.RelativeItem().Text("Kicket – Reporte de ventas").FontSize(18).Bold().FontColor(ColorTitulo);
                            fila.ConstantItem(170).AlignRight().Text($"Generado: {r.GeneradoEn:dd/MM/yyyy HH:mm}").FontSize(9).FontColor("#666666");
                        });
                        col.Item().Text(DescribirFiltro(r)).FontSize(9).FontColor("#555555");
                        col.Item().PaddingTop(4).LineHorizontal(1).LineColor(ColorTitulo);
                    });

                    pagina.Content().PaddingTop(10).Column(col =>
                    {
                        col.Spacing(12);
                        col.Item().Element(c => Indicadores(c, r.Resumen));
                        col.Item().Text(ReporteVentasDto.CriterioVentas).FontSize(8).Italic().FontColor("#666666");

                        foreach (var grafico in r.Graficos)
                        {
                            col.Item().ShowEntire().Column(g =>
                            {
                                g.Item().Text(grafico.Titulo).FontSize(12).Bold().FontColor(ColorTitulo);
                                g.Item().PaddingTop(4).AlignCenter().Width(470).Svg(grafico.Svg);
                            });
                        }

                        col.Item().PageBreak();
                        col.Item().Element(c => Seccion(c, "Ventas por partido", 3,
                            new[] { "Partido", "Fecha", "Estadio", "Compras", "Entradas", "Recaudado" },
                            new float[] { 3, 1.4f, 2, 1, 1, 1.5f },
                            r.PorEvento.Select(e => new[] { e.Evento, e.Fecha.ToString("dd/MM/yyyy HH:mm"), e.Estadio, N(e.Compras), N(e.Entradas), M(e.Recaudado) }),
                            new[] { "Total", "", "", N(r.PorEvento.Sum(e => e.Compras)), N(r.PorEvento.Sum(e => e.Entradas)), M(r.PorEvento.Sum(e => e.Recaudado)) }));

                        col.Item().Element(c => Seccion(c, "Ventas por club (atribuidas al club local)", 1,
                            new[] { "Club", "Partidos local", "Partidos visitante", "Entradas", "Recaudado" },
                            new float[] { 3, 1.2f, 1.4f, 1, 1.5f },
                            r.PorClub.Select(x => new[] { x.Club, N(x.EventosComoLocal), N(x.EventosComoVisitante), N(x.Entradas), M(x.Recaudado) }),
                            new[] { "Total", "", "", N(r.PorClub.Sum(x => x.Entradas)), M(r.PorClub.Sum(x => x.Recaudado)) }));

                        col.Item().Element(c => Seccion(c, "Ventas por sector", 2,
                            new[] { "Estadio", "Sector", "Capacidad por partido", "Entradas", "Recaudado" },
                            new float[] { 2, 2, 1.5f, 1, 1.5f },
                            r.PorSector.Select(x => new[] { x.Estadio, x.Sector, N(x.CapacidadPorEvento), N(x.Entradas), M(x.Recaudado) }),
                            new[] { "Total", "", "", N(r.PorSector.Sum(x => x.Entradas)), M(r.PorSector.Sum(x => x.Recaudado)) }));

                        col.Item().Element(c => Seccion(c, "Ventas por día", 1,
                            new[] { "Día", "Compras", "Entradas", "Recaudado" },
                            new float[] { 2, 1, 1, 1.5f },
                            r.PorPeriodo.Select(x => new[] { x.Dia.ToString("dd/MM/yyyy"), N(x.Compras), N(x.Entradas), M(x.Recaudado) }),
                            new[] { "Total", N(r.PorPeriodo.Sum(x => x.Compras)), N(r.PorPeriodo.Sum(x => x.Entradas)), M(r.PorPeriodo.Sum(x => x.Recaudado)) }));
                    });

                    pagina.Footer().AlignCenter().Text(t =>
                    {
                        t.DefaultTextStyle(s => s.FontSize(8).FontColor("#777777"));
                        t.Span("Kicket – Página ");
                        t.CurrentPageNumber();
                        t.Span(" de ");
                        t.TotalPages();
                    });
                });
            });

            return documento.GeneratePdf();
        }

        // Fila de tarjetas con los indicadores principales.
        private static void Indicadores(IContainer contenedor, ResumenVentasDto resumen)
        {
            var tarjetas = new (string Titulo, string Valor)[]
            {
                ("Total recaudado", M(resumen.TotalRecaudado)),
                ("Entradas vendidas", N(resumen.EntradasVendidas)),
                ("Compras confirmadas", N(resumen.ComprasConfirmadas)),
                ("Ticket promedio", M(resumen.TicketPromedio)),
                ("Partidos con ventas", $"{resumen.EventosConVentas} de {resumen.EventosTotales}"),
                ("Compradores", N(resumen.CompradoresDistintos))
            };
            contenedor.Table(tabla =>
            {
                tabla.ColumnsDefinition(c => { for (int i = 0; i < 3; i++) c.RelativeColumn(); });
                foreach (var (titulo, valor) in tarjetas)
                {
                    tabla.Cell().Padding(3).Border(0.5f).BorderColor("#D0D7E2").Background("#F4F7FB").Padding(8).Column(c =>
                    {
                        c.Item().Text(titulo).FontSize(8).FontColor("#555555");
                        c.Item().Text(valor).FontSize(14).Bold().FontColor(ColorTitulo);
                    });
                }
            });
        }

        // Tabla con cabecera de color, filas alternadas y fila de totales.
        // Las primeras "columnasTexto" columnas se alinean a la izquierda; las numericas, a la derecha.
        private static void Seccion(IContainer contenedor, string titulo, int columnasTexto, string[] cabeceras, float[] anchos,
            IEnumerable<string[]> filas, string[] totales)
        {
            contenedor.Column(col =>
            {
                col.Item().PaddingBottom(4).Text(titulo).FontSize(12).Bold().FontColor(ColorTitulo);
                col.Item().Table(tabla =>
                {
                    tabla.ColumnsDefinition(c => { foreach (var a in anchos) c.RelativeColumn(a); });
                    tabla.Header(h =>
                    {
                        for (int i = 0; i < cabeceras.Length; i++)
                        {
                            var celda = h.Cell().Background(ColorCabecera).Padding(4);
                            (i < columnasTexto ? celda : celda.AlignRight()).Text(cabeceras[i]).FontColor(Colors.White).Bold();
                        }
                    });

                    var lista = filas.ToList();
                    if (lista.Count == 0)
                    {
                        tabla.Cell().ColumnSpan((uint)cabeceras.Length).Padding(4).Text("Sin datos para los filtros elegidos.").Italic();
                    }
                    for (int f = 0; f < lista.Count; f++)
                    {
                        string fondo = f % 2 == 0 ? "#FFFFFF" : "#F5F5F5";
                        for (int i = 0; i < lista[f].Length; i++)
                        {
                            var celda = tabla.Cell().Background(fondo).BorderBottom(0.5f).BorderColor("#E0E0E0").Padding(4);
                            (i < columnasTexto ? celda : celda.AlignRight()).Text(lista[f][i]);
                        }
                    }
                    for (int i = 0; i < totales.Length; i++)
                    {
                        var celda = tabla.Cell().Background("#E8EEF6").Padding(4);
                        (i < columnasTexto ? celda : celda.AlignRight()).Text(totales[i]).Bold();
                    }
                });
            });
        }

        private static string DescribirFiltro(ReporteVentasDto r)
        {
            string desde = r.Desde?.ToString("dd/MM/yyyy") ?? "el inicio";
            string hasta = r.Hasta?.ToString("dd/MM/yyyy") ?? "hoy";
            string texto = $"Período de compras: desde {desde} hasta {hasta}";
            return r.EventoNombre is null ? texto + " · Todos los partidos" : texto + $" · Partido: {r.EventoNombre}";
        }

        private static string N(int valor) => valor.ToString("N0", Ar);
        private static string M(decimal valor) => "$ " + valor.ToString("N2", Ar);
    }
}
