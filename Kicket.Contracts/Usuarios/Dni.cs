namespace Kicket.Contracts.Usuarios
{
    /// <summary>
    /// Regla unica del DNI argentino, compartida por la API y los clientes:
    /// se aceptan 7 u 8 digitos, con o sin puntos ("30.123.456" o "30123456"),
    /// y se guarda normalizado como texto de solo digitos (asi se conservan ceros a la izquierda).
    /// </summary>
    public static class Dni
    {
        // Patron para [RegularExpression]: 7 u 8 digitos, puntos de miles opcionales.
        public const string Patron = @"^\s*\d{1,2}\.?\d{3}\.?\d{3}\s*$";
        public const string MensajeInvalido = "El DNI debe tener 7 u 8 digitos (con o sin puntos).";
        // Largo maximo guardado en la base (solo digitos).
        public const int LargoMaximo = 10;

        /// <summary>Saca puntos, espacios y guiones. Devuelve null si no queda nada.</summary>
        public static string? Normalizar(string? dni)
        {
            if (string.IsNullOrWhiteSpace(dni)) return null;
            var limpio = new string(dni.Where(c => c != '.' && c != ' ' && c != '-').ToArray());
            return limpio.Length == 0 ? null : limpio;
        }

        /// <summary>true si, ya normalizado, son 7 u 8 digitos.</summary>
        public static bool EsValido(string? dni)
        {
            var limpio = Normalizar(dni);
            return limpio is not null && limpio.Length is 7 or 8 && limpio.All(char.IsDigit);
        }
    }
}
