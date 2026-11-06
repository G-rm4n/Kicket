using Domain.Entities;
using Kicket.Contracts.Common;
using Kicket.Contracts.Usuarios;

namespace Core.Services
{
    /// <summary>Validaciones compartidas entre servicios. Tiran ArgumentException, que la API responde como 400.</summary>
    internal static class Validaciones
    {
        /// <summary>Normaliza los parametros de un listado y tira ArgumentException si hay alguno invalido.</summary>
        public static void ValidarFiltro(PagedRequest filtro)
        {
            ArgumentNullException.ThrowIfNull(filtro);
            filtro.Normalizar();
            var errores = filtro.Validar();
            if (errores.Count > 0)
                throw new ArgumentException(string.Join(" ", errores));
        }

        /// <summary>
        /// Normaliza y valida los datos personales del usuario (DNI, telefono, fecha de nacimiento).
        /// dniObligatorio = true en las altas; en la edicion se acepta null para no inventar DNI a usuarios viejos.
        /// </summary>
        public static void ValidarDatosPersonales(Usuario usuario, bool dniObligatorio)
        {
            usuario.Dni = Dni.Normalizar(usuario.Dni);
            if (usuario.Dni is null)
            {
                if (dniObligatorio)
                    throw new ArgumentException("El DNI es obligatorio.");
            }
            else if (!Dni.EsValido(usuario.Dni))
            {
                throw new ArgumentException(Dni.MensajeInvalido);
            }

            usuario.Telefono = string.IsNullOrWhiteSpace(usuario.Telefono) ? null : usuario.Telefono.Trim();
            if (usuario.Telefono is { Length: > 30 })
                throw new ArgumentException("El telefono no puede superar los 30 caracteres.");

            if (usuario.FechaNacimiento.HasValue)
            {
                var fecha = usuario.FechaNacimiento.Value.Date;
                if (fecha > DateTime.Today)
                    throw new ArgumentException("La fecha de nacimiento no puede ser futura.");
                if (fecha.Year < 1900)
                    throw new ArgumentException("La fecha de nacimiento no es valida.");
                usuario.FechaNacimiento = fecha;
            }
        }

        /// <summary>Devuelve el texto recortado o null si quedo vacio.</summary>
        public static string? TextoOpcional(string? texto) =>
            string.IsNullOrWhiteSpace(texto) ? null : texto.Trim();
    }
}
