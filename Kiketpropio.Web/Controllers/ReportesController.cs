using Kiketpropio.Dominio.DTOs;
using Kiketpropio.Web.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kiketpropio.Web.Controllers
{
    [Authorize(Roles = "Admin,Vendedor")]
    public class ReportesController : Controller
    {
        private readonly ApiClient _apiClient;

        public ReportesController(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<IActionResult> UsuariosPorRol()
        {
            var reporte = await _apiClient.GetAll<UsuariosPorRol>("Reporte/usuarios-por-rol");
            return View(reporte);
        }

        public async Task<IActionResult> EstadiosPorCiudad()
        {
            var reporte = await _apiClient.GetAll<EstadiosPorCiudad>("Reporte/estadios-por-ciudad");
            return View(reporte);
        }
    }
}
