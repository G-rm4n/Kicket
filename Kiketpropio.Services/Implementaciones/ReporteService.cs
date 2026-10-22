using Kiketpropio.Dominio.DTOs;
using Kiketpropio.Dominio.Entidades;
using Kiketpropio.Services.Interfaces;

namespace Kiketpropio.Services.Implementaciones
{
    public class ReporteService:IReporteService
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IEstadioService _estadioService;

        public ReporteService(IUsuarioService usuarioService, IEstadioService estadioService)
        {
            _usuarioService = usuarioService;
            _estadioService = estadioService;
        }

        public IEnumerable<UsuariosPorRol> GetUsuariosPorRol()
        {
            return _usuarioService.GetAll()
                .GroupBy(u => u.Rol)
                .Select(g => new UsuariosPorRol(g.Key, g.Count(), (int)Math.Round(g.Average(u => GetEdad(u)))))
                .OrderByDescending(r => r.Cantidad)
                .ToList();
        }

        public IEnumerable<EstadiosPorCiudad> GetEstadiosPorCiudad()
        {
            return _estadioService.GetAll()
                .GroupBy(e => e.Ciudad)
                .Select(g => new EstadiosPorCiudad(g.Key, g.Count(), string.Join(", ", g.Select(e => e.Nombre).OrderBy(n => n))))
                .OrderByDescending(r => r.Cantidad)
                .ThenBy(r => r.Ciudad)
                .ToList();
        }

        private static int GetEdad(Usuario usuarioInput)
        {
            var edad = DateTime.Today.Year - usuarioInput.FechaNacimiento.Year;
            if (usuarioInput.FechaNacimiento.Date > DateTime.Today.AddYears(-edad)) edad -= 1;

            return edad;
        }
    }
}
