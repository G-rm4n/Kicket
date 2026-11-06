using Kicket.Contracts.Common;

namespace Kicket.Contracts.Usuarios
{
    /// <summary>Filtros de GET /usuarios/paginado (solo Admin). Busqueda: nombre, apellido, email o DNI.</summary>
    public class UsuarioFiltro : PagedRequest
    {
        // Rol exacto (Admin, Usuario o Cliente).
        public string? Rol { get; set; }
        // DNI exacto o parcial (se ignoran puntos y espacios).
        public string? Dni { get; set; }

        public override IReadOnlyCollection<string> OrdenesPermitidos => new[] { "id", "nombre", "apellido", "email", "dni", "rol", "registro" };

        public override List<string> Validar()
        {
            var errores = base.Validar();
            if (!string.IsNullOrWhiteSpace(Rol) && !Roles.EsValido(Rol))
                errores.Add($"Rol invalido. Valores validos: {string.Join(", ", Roles.Todos)}.");
            return errores;
        }
    }
}
