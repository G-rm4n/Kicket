using Kiketpropio.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kiketpropio.API.Controllers
{
    [ApiController]
    [Authorize(Roles = "Admin,Vendedor")]
    [Route("api/[controller]")]
    public class ReporteController : ControllerBase
    {
        private readonly IReporteService _reporteService;

        public ReporteController(IReporteService reporteService)
        {
            _reporteService = reporteService;
        }

        [HttpGet("usuarios-por-rol")]
        public IActionResult GetUsuariosPorRol()
        {
            return Ok(_reporteService.GetUsuariosPorRol());
        }

        [HttpGet("estadios-por-ciudad")]
        public IActionResult GetEstadiosPorCiudad()
        {
            return Ok(_reporteService.GetEstadiosPorCiudad());
        }
    }
}
