using Kiketpropio.Dominio.DTOs;

namespace Kiketpropio.Services.Interfaces
{
    public interface IReporteService
    {
        IEnumerable<UsuariosPorRol> GetUsuariosPorRol();
        IEnumerable<EstadiosPorCiudad> GetEstadiosPorCiudad();
    }
}
