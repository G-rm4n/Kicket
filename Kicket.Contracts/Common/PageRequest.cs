using System.ComponentModel.DataAnnotations;

namespace Kicket.Contracts.Common
{
    /// <summary>
    /// Parametros de paginado que manda el cliente. Viajan en la query string
    /// (?pagina=2&amp;tamanoPagina=20), no en el cuerpo, porque los endpoints paginados son GET.
    ///
    /// Busqueda y OrdenarPor son los que la capa de datos ya espera como filtro y
    /// ordenamiento en AyncGetPaginated: el cliente manda que quiere, no como se traduce a SQL.
    /// </summary>
    public class PageRequest
    {
        public const int TamanoPorDefecto = 20;

        /// <summary>Tope duro para que un cliente no pida la tabla entera en una sola pagina.</summary>
        public const int TamanoMaximo = 100;

        [Range(1, int.MaxValue, ErrorMessage = "La pagina debe ser 1 o mayor.")]
        public int Pagina { get; set; } = 1;

        [Range(1, TamanoMaximo, ErrorMessage = "El tamano de pagina debe estar entre 1 y 100.")]
        public int TamanoPagina { get; set; } = TamanoPorDefecto;

        /// <summary>Texto libre para filtrar. Que columnas mira lo decide la API.</summary>
        public string? Busqueda { get; set; }

        /// <summary>
        /// Nombre de la propiedad por la que ordenar. Conviene mandar siempre uno:
        /// un Skip/Take sin orden explicito no garantiza el mismo resultado entre paginas.
        /// </summary>
        public string? OrdenarPor { get; set; }

        public bool Descendente { get; set; }

        /// <summary>
        /// Acomoda los valores fuera de rango en vez de fallar. Pagina 0 o negativa
        /// terminaria en un Skip negativo del lado de la base, que revienta la consulta.
        /// </summary>
        public PageRequest Normalizado() => new()
        {
            Pagina = Pagina < 1 ? 1 : Pagina,
            TamanoPagina = TamanoPagina switch
            {
                < 1 => TamanoPorDefecto,
                > TamanoMaximo => TamanoMaximo,
                _ => TamanoPagina
            },
            Busqueda = string.IsNullOrWhiteSpace(Busqueda) ? null : Busqueda.Trim(),
            OrdenarPor = string.IsNullOrWhiteSpace(OrdenarPor) ? null : OrdenarPor.Trim(),
            Descendente = Descendente
        };
    }
}
