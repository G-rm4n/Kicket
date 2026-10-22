using Kiketpropio.Dominio.Entidades;
using Kiketpropio.Web.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kiketpropio.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsuariosController : Controller
    {
        private const string Recurso = "Usuario";

        private readonly ApiClient _apiClient;

        public UsuariosController(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<IActionResult> Index()
        {
            var usuarios = await _apiClient.GetAll<Usuario>(Recurso);
            return View(usuarios);
        }

        public async Task<IActionResult> Detalle(int id)
        {
            var usuario = await _apiClient.GetOne<Usuario>(Recurso, id);
            if (usuario is null) return NotFound();

            return View(usuario);
        }

        [HttpGet]
        public IActionResult Crear()
        {
            return View(new Usuario());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(Usuario usuarioInput)
        {
            if (string.IsNullOrWhiteSpace(usuarioInput.Email) || string.IsNullOrWhiteSpace(usuarioInput.Pass))
            {
                ViewBag.Error = "El email y contraseña son obligatorios";
                return View(usuarioInput);
            }

            var resultado = await _apiClient.Create(Recurso, usuarioInput);
            if (!resultado)
            {
                ViewBag.Error = "No se pudo registrar el usuario";
                return View(usuarioInput);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            var usuario = await _apiClient.GetOne<Usuario>(Recurso, id);
            if (usuario is null) return NotFound();

            return View(usuario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(Usuario usuarioInput)
        {
            if (string.IsNullOrWhiteSpace(usuarioInput.Email) || string.IsNullOrWhiteSpace(usuarioInput.Pass))
            {
                ViewBag.Error = "El email y contraseña son obligatorios";
                return View(usuarioInput);
            }

            var resultado = await _apiClient.Update(Recurso, usuarioInput.IdUsuario, usuarioInput);
            if (!resultado)
            {
                ViewBag.Error = "No se pudo actualizar el usuario";
                return View(usuarioInput);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Eliminar(int id)
        {
            var usuario = await _apiClient.GetOne<Usuario>(Recurso, id);
            if (usuario is null) return NotFound();

            return View(usuario);
        }

        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarConfirmado(int id)
        {
            await _apiClient.Delete(Recurso, id);
            return RedirectToAction(nameof(Index));
        }
    }
}
