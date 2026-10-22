using Kiketpropio.Dominio.Entidades;
using Kiketpropio.Web.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kiketpropio.Web.Controllers
{
    [Authorize]
    public class ClubesController : Controller
    {
        private const string Recurso = "Club";

        private readonly ApiClient _apiClient;

        public ClubesController(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<IActionResult> Index()
        {
            var clubes = await _apiClient.GetAll<Club>(Recurso);
            return View(clubes);
        }

        public async Task<IActionResult> Detalle(int id)
        {
            var club = await _apiClient.GetOne<Club>(Recurso, id);
            if (club is null) return NotFound();

            return View(club);
        }

        [Authorize(Roles = "Admin,Vendedor")]
        [HttpGet]
        public IActionResult Crear()
        {
            return View(new Club());
        }

        [Authorize(Roles = "Admin,Vendedor")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(Club clubInput)
        {
            if (string.IsNullOrWhiteSpace(clubInput.Nombre) || string.IsNullOrWhiteSpace(clubInput.Logo_Url))
            {
                ViewBag.Error = "El nombre o URL no pueden estar vacios";
                return View(clubInput);
            }

            var resultado = await _apiClient.Create(Recurso, clubInput);
            if (!resultado)
            {
                ViewBag.Error = "No se pudo registrar el club";
                return View(clubInput);
            }

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin,Vendedor")]
        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            var club = await _apiClient.GetOne<Club>(Recurso, id);
            if (club is null) return NotFound();

            return View(club);
        }

        [Authorize(Roles = "Admin,Vendedor")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(Club clubInput)
        {
            if (string.IsNullOrWhiteSpace(clubInput.Nombre) || string.IsNullOrWhiteSpace(clubInput.Logo_Url))
            {
                ViewBag.Error = "El nombre o URL no pueden estar vacios";
                return View(clubInput);
            }

            var resultado = await _apiClient.Update(Recurso, clubInput.IdClub, clubInput);
            if (!resultado)
            {
                ViewBag.Error = "No se pudo actualizar el club";
                return View(clubInput);
            }

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Eliminar(int id)
        {
            var club = await _apiClient.GetOne<Club>(Recurso, id);
            if (club is null) return NotFound();

            return View(club);
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
