namespace Kiketpropio.Escritorio.Servicios
{
    public static class Sesion
    {
        public static string Token { get; set; } = string.Empty;
        public static string Nombre { get; set; } = string.Empty;
        public static string Email { get; set; } = string.Empty;
        public static string Rol { get; set; } = string.Empty;

        public static bool EsAdmin => Rol == "Admin";
        public static bool EsVendedor => Rol == "Vendedor";
        public static bool PuedeEditar => EsAdmin || EsVendedor;
        public static bool PuedeVerReportes => EsAdmin || EsVendedor;

        public static void Limpiar()
        {
            Token = string.Empty;
            Nombre = string.Empty;
            Email = string.Empty;
            Rol = string.Empty;
        }
    }
}
