namespace Kicket.Contracts.Usuarios
{
    /// <summary>
    /// Roles validos del sistema. Estan aca y no como string suelto para que la API,
    /// la capa cliente y los formularios de escritorio usen todos el mismo valor.
    /// </summary>
    public static class Roles
    {
        public const string Admin = "Admin";
        public const string Usuario = "Usuario";

        public static readonly string[] Todos = { Admin, Usuario };

        public static bool EsValido(string? rol) =>
            rol is not null && Todos.Contains(rol);
    }
}
