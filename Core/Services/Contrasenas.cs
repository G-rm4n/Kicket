namespace Core.Services
{
    /// <summary>
    /// Unico lugar donde se toca BCrypt: hashear al guardar, verificar al loguear.
    /// </summary>
    internal static class Contrasenas
    {
        public static string Hashear(string plana) => BCrypt.Net.BCrypt.HashPassword(plana);

        // shortcut: se detecta "ya es hash" por formato ($2x$ + 60 chars); sirve para no re-hashear
        // el hash que devuelve el PUT /usuarios cuando no se cambia la clave. Migrar a un campo propio si crece.
        public static bool EsHash(string valor) => valor.Length == 60 && valor.StartsWith("$2");

        // Una clave guardada en texto plano (base vieja) nunca verifica: hay que recrear la base.
        public static bool Verificar(string plana, string guardada) =>
            EsHash(guardada) && BCrypt.Net.BCrypt.Verify(plana, guardada);
    }
}
