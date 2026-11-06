using System.Globalization;
using System.Security;
using System.Text;

namespace Core.Reportes
{
    /// <summary>
    /// Genera graficos como texto SVG a partir de datos reales. Se hace en el servidor para que el
    /// grafico que se ve en la web sea EXACTAMENTE el mismo que se incrusta en el PDF (QuestPDF dibuja SVG),
    /// sin depender de capturas de pantalla ni de librerias de graficos en el navegador.
    /// </summary>
    public static class GraficosSvg
    {
        private const int Ancho = 720;
        private static readonly CultureInfo Inv = CultureInfo.InvariantCulture;
        private static readonly CultureInfo Ar = CultureInfo.GetCultureInfo("es-AR");
        // Paleta: un color principal y una serie para categorias.
        private const string ColorPrincipal = "#1F6FB2";
        private static readonly string[] Paleta = { "#1F6FB2", "#2E9E6B", "#E0A23A", "#C8553D", "#7A5BB5", "#3AA6C4", "#8C8C8C", "#B5487A" };
        private const string ColorTexto = "#333333";
        private const string ColorGrilla = "#E3E3E3";

        /// <summary>Barras horizontales (sirve para nombres largos, como partidos o clubes).</summary>
        public static string BarrasHorizontales(IReadOnlyList<(string Etiqueta, decimal Valor)> datos, bool esMoneda)
        {
            if (datos.Count == 0) return SinDatos();

            const int altoBarra = 26, separacion = 10, margenIzq = 230, margenDer = 110, margenSup = 10;
            int alto = margenSup * 2 + datos.Count * (altoBarra + separacion);
            decimal maximo = Math.Max(datos.Max(d => d.Valor), 1);
            int anchoUtil = Ancho - margenIzq - margenDer;

            var svg = Inicio(alto);
            for (int i = 0; i < datos.Count; i++)
            {
                var (etiqueta, valor) = datos[i];
                int y = margenSup + i * (altoBarra + separacion);
                double largo = (double)(valor / maximo) * anchoUtil;
                svg.Append(Texto(margenIzq - 8, y + altoBarra / 2 + 5, Recortar(etiqueta, 32), "end", 13));
                svg.Append($"<rect x=\"{margenIzq}\" y=\"{y}\" width=\"{N(Math.Max(largo, 1))}\" height=\"{altoBarra}\" rx=\"3\" fill=\"{ColorPrincipal}\"/>");
                svg.Append(Texto(margenIzq + largo + 6, y + altoBarra / 2 + 5, Formatear(valor, esMoneda), "start", 12, negrita: true));
            }
            return Fin(svg);
        }

        /// <summary>Linea temporal (evolucion de ventas por dia).</summary>
        public static string Linea(IReadOnlyList<(DateTime Fecha, decimal Valor)> datos, bool esMoneda)
        {
            if (datos.Count == 0) return SinDatos();

            const int alto = 300, margenIzq = 90, margenDer = 20, margenSup = 20, margenInf = 50;
            int anchoUtil = Ancho - margenIzq - margenDer, altoUtil = alto - margenSup - margenInf;
            decimal maximo = Math.Max(datos.Max(d => d.Valor), 1);

            var svg = Inicio(alto);
            // Grilla horizontal con 4 divisiones y sus valores.
            for (int i = 0; i <= 4; i++)
            {
                double y = margenSup + altoUtil - altoUtil * i / 4.0;
                svg.Append($"<line x1=\"{margenIzq}\" y1=\"{N(y)}\" x2=\"{Ancho - margenDer}\" y2=\"{N(y)}\" stroke=\"{ColorGrilla}\"/>");
                svg.Append(Texto(margenIzq - 6, y + 4, Formatear(maximo * i / 4, esMoneda), "end", 11));
            }

            double paso = datos.Count == 1 ? 0 : anchoUtil / (double)(datos.Count - 1);
            var puntos = new List<(double X, double Y)>();
            for (int i = 0; i < datos.Count; i++)
            {
                double x = datos.Count == 1 ? margenIzq + anchoUtil / 2.0 : margenIzq + paso * i;
                double y = margenSup + altoUtil - (double)(datos[i].Valor / maximo) * altoUtil;
                puntos.Add((x, y));
            }
            if (puntos.Count > 1)
            {
                svg.Append($"<polyline fill=\"none\" stroke=\"{ColorPrincipal}\" stroke-width=\"2.5\" points=\"{string.Join(" ", puntos.Select(p => $"{N(p.X)},{N(p.Y)}"))}\"/>");
            }
            // Puntos y fechas (se muestran como maximo ~10 etiquetas para que no se pisen).
            int cadaCuantas = Math.Max(1, (int)Math.Ceiling(datos.Count / 10.0));
            for (int i = 0; i < puntos.Count; i++)
            {
                svg.Append($"<circle cx=\"{N(puntos[i].X)}\" cy=\"{N(puntos[i].Y)}\" r=\"4\" fill=\"{ColorPrincipal}\"/>");
                if (i % cadaCuantas == 0 || i == puntos.Count - 1)
                    svg.Append(Texto(puntos[i].X, alto - margenInf + 20, datos[i].Fecha.ToString("dd/MM/yy", Ar), "middle", 11));
            }
            return Fin(svg);
        }

        /// <summary>Grafico de dona con leyenda (distribucion porcentual).</summary>
        public static string Dona(IReadOnlyList<(string Etiqueta, decimal Valor)> datos)
        {
            var conValor = datos.Where(d => d.Valor > 0).ToList();
            if (conValor.Count == 0) return SinDatos();

            const int alto = 280, cx = 150, cy = 140, radio = 115, grosor = 48;
            decimal total = conValor.Sum(d => d.Valor);
            var svg = Inicio(alto);

            double angulo = -Math.PI / 2;
            for (int i = 0; i < conValor.Count; i++)
            {
                string color = Paleta[i % Paleta.Length];
                double porcion = (double)(conValor[i].Valor / total);
                if (porcion >= 0.9999)
                {
                    // Un solo segmento: el arco SVG no puede dibujar 360 grados, se usa un circulo.
                    svg.Append($"<circle cx=\"{cx}\" cy=\"{cy}\" r=\"{radio - grosor / 2}\" fill=\"none\" stroke=\"{color}\" stroke-width=\"{grosor}\"/>");
                }
                else
                {
                    double fin = angulo + porcion * 2 * Math.PI;
                    double r = radio - grosor / 2.0;
                    double x1 = cx + r * Math.Cos(angulo), y1 = cy + r * Math.Sin(angulo);
                    double x2 = cx + r * Math.Cos(fin), y2 = cy + r * Math.Sin(fin);
                    int arcoGrande = porcion > 0.5 ? 1 : 0;
                    svg.Append($"<path d=\"M {N(x1)} {N(y1)} A {N(r)} {N(r)} 0 {arcoGrande} 1 {N(x2)} {N(y2)}\" fill=\"none\" stroke=\"{color}\" stroke-width=\"{grosor}\"/>");
                    angulo = fin;
                }

                // Leyenda a la derecha.
                int ly = 40 + i * 28;
                svg.Append($"<rect x=\"320\" y=\"{ly - 12}\" width=\"16\" height=\"16\" rx=\"3\" fill=\"{color}\"/>");
                svg.Append(Texto(344, ly + 1, $"{Recortar(conValor[i].Etiqueta, 34)} — {N0(conValor[i].Valor)} ({porcion:P0})", "start", 13));
            }
            svg.Append(Texto(cx, cy + 6, N0(total), "middle", 18, negrita: true));
            return Fin(svg);
        }

        // ---------- ayudas ----------

        private static StringBuilder Inicio(int alto) => new StringBuilder()
            .Append($"<svg xmlns=\"http://www.w3.org/2000/svg\" viewBox=\"0 0 {Ancho} {alto}\" width=\"{Ancho}\" height=\"{alto}\" font-family=\"Arial, Helvetica, sans-serif\">")
            .Append($"<rect x=\"0\" y=\"0\" width=\"{Ancho}\" height=\"{alto}\" fill=\"#FFFFFF\"/>");

        private static string Fin(StringBuilder svg) => svg.Append("</svg>").ToString();

        private static string SinDatos()
        {
            var svg = Inicio(80);
            svg.Append(Texto(Ancho / 2.0, 45, "Sin ventas confirmadas para los filtros elegidos", "middle", 14));
            return Fin(svg);
        }

        private static string Texto(double x, double y, string texto, string ancla, int tamano, bool negrita = false) =>
            $"<text x=\"{N(x)}\" y=\"{N(y)}\" text-anchor=\"{ancla}\" font-size=\"{tamano}\" fill=\"{ColorTexto}\"{(negrita ? " font-weight=\"bold\"" : "")}>{SecurityElement.Escape(texto)}</text>";

        private static string Recortar(string texto, int maximo) =>
            texto.Length <= maximo ? texto : texto[..(maximo - 1)] + "…";

        private static string Formatear(decimal valor, bool esMoneda) =>
            esMoneda ? "$ " + valor.ToString("N0", Ar) : N0(valor);

        private static string N0(decimal valor) => valor.ToString("N0", Ar);

        // Numeros del SVG siempre con punto decimal (si no, "12,5" rompe el dibujo).
        private static string N(double valor) => valor.ToString("0.##", Inv);
    }
}
