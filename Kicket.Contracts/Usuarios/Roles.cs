namespace Kicket.Contracts.Usuarios
{
    /// <summary>
    /// Roles validos del sistema. Estan aca y no como string suelto para que la API,
    /// la capa cliente y los formularios de escritorio usen todos el mismo valor.
    /// </summary>
    public static class Roles
    {
        // Rol de administrador: puede gestionar clubes, estadios, eventos, sectores y usuarios.
        public const string Admin = "Admin";
        // Rol por defecto de un usuario común.
        public const string Usuario = "Usuario";
        // Rol que asigna el registro de AuthService.
        public const string Cliente = "Cliente";

        // Lista de todos los roles válidos.
        public static readonly string[] Todos = { Admin, Usuario, Cliente };

        // Devuelve true si el texto recibido es uno de los roles válidos.
        public static bool EsValido(string? rol) =>
            rol is not null && Todos.Contains(rol);
    }
}
