using Domain.Entities;
using Kicket.Contracts.Clubes;
using Kicket.Contracts.Common;
using Kicket.Contracts.Estadios;
using Kicket.Contracts.Eventos;
using Kicket.Contracts.Sectores;
using Kicket.Contracts.Usuarios;

namespace WebApi.EndPoints
{
    /// <summary>
    /// Conversiones entidad → DTO en un solo lugar (antes cada endpoint repetía el mismo bloque).
    /// UsuarioDto nunca lleva la contraseña.
    /// </summary>
    public static class Mapeos
    {
        public static ClubDto ADto(this Club c) => new()
        {
            ClubId = c.ClubId,
            Nombre = c.Nombre,
            Descripcion = c.Descripcion,
            Abreviatura = c.Abreviatura,
            Ciudad = c.Ciudad,
            AnioFundacion = c.AnioFundacion
        };

        public static EstadioDto ADto(this Estadio e) => new()
        {
            IdEstadio = e.EstadioId,
            Nombre = e.Nombre,
            Direccion = e.Direccion,
            Ciudad = e.Ciudad,
            Provincia = e.Provincia
        };

        public static EventoDto ADto(this Evento e) => new()
        {
            IdEvento = e.IdEvento,
            Nombre = e.Nombre,
            Fecha = e.Fecha,
            IdEstadio = e.EstadioId,
            IdEquipoLocal = e.ClubLocalId,
            IdEquipoVisitante = e.ClubVisitanteId,
            EstaCancelado = e.EstaCancelado,
            Descripcion = e.Descripcion
        };

        public static SectorDto ADto(this Sector s) => new()
        {
            SectorId = s.SectorId,
            EstadioId = s.EstadioId,
            Nombre = s.Nombre,
            CapacidadMaxima = s.CapacidadMaxima,
            PrecioBase = s.PrecioBase
        };

        public static UsuarioDto ADto(this Usuario u) => new()
        {
            IdUsuario = u.IdUsuario,
            Nombre = u.Nombre,
            Apellido = u.Apellido,
            Email = u.Email,
            Rol = u.Rol,
            Dni = u.Dni,
            Telefono = u.Telefono,
            FechaNacimiento = u.FechaNacimiento,
            FechaRegistro = u.FechaRegistro
        };
    }
}
