using Kiketpropio.Dominio.Entidades;
using Kiketpropio.Web.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kiketpropio.Web.Controllers
{
    [Authorize]
    public class EstadiosController : Controller
    {
        private const string Recurso = "Estadio";

        private readonly ApiClient _apiClient;

        public EstadiosController(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<IActionResult> Index()
        {
            var estadios = await _apiClient.GetAll<Estadio>(Recurso);
            return View(estadios);
        }

        public async Task<IActionResult> Detalle(int id)
        {
            var estadio = await _apiClient.GetOne<Estadio>(Recurso, id);
            if (estadio is null) return NotFound();

            return View(estadio);
        }

        [Authorize(Roles = "Admin,Vendedor")]
        [HttpGet]
        public IActionResult Crear()
        {
            return View(new Estadio());
        }

        [Authorize(Roles = "Admin,Vendedor")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(Estadio estadioInput)
        {
            if (string.IsNullOrWhiteSpace(estadioInput.Nombre) || string.IsNullOrWhiteSpace(estadioInput.Ciudad))
            {
                ViewBag.Error = "El nombre y la ciudad son obligatorios";
                return View(estadioInput);
            }

            var resultado = await _apiClient.Create(Recurso, estadioInput);
            if (!resultado)
            {
                ViewBag.Error = "No se pudo registrar el estadio";
                return View(estadioInput);
            }

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin,Vendedor")]
        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            var estadio = await _apiClient.GetOne<Estadio>(Recurso, id);
            if (estadio is null) return NotFound();

            return View(estadio);
        }

        [Authorize(Roles = "Admin,Vendedor")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(Estadio estadioInput)
        {
            if (string.IsNullOrWhiteSpace(estadioInput.Nombre) || string.IsNullOrWhiteSpace(estadioInput.Ciudad))
            {
                ViewBag.Error = "El nombre y la ciudad son obligatorios";
                return View(estadioInput);
            }

            var resultado = await _apiClient.Update(Recurso, estadioInput.IdEstadio, estadioInput);
            if (!resultado)
            {
                ViewBag.Error = "No se pudo actualizar el estadio";
                return View(estadioInput);
            }

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Eliminar(int id)
        {
            var estadio = await _apiClient.GetOne<Estadio>(Recurso, id);
            if (estadio is null) return NotFound();

            return View(estadio);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarConfirmado(int id)
        {
            await _apiClient.Delete(Recurso, id);
            return RedirectToAction(nameof(Index));
        }
    }
}
