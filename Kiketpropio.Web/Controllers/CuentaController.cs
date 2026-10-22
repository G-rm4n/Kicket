using System.Security.Claims;
using Kiketpropio.Dominio.DTOs;
using Kiketpropio.Web.Servicios;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace Kiketpropio.Web.Controllers
{
    public class CuentaController : Controller
    {
        private readonly ApiClient _apiClient;

        public CuentaController(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginRequest());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginRequest loginInput)
        {
            if (string.IsNullOrWhiteSpace(loginInput.Email) || string.IsNullOrWhiteSpace(loginInput.Pass))
            {
                ViewBag.Error = "El email y contraseña son obligatorios";
                return View(loginInput);
            }

            var resultado = await _apiClient.Login(loginInput);
            if (resultado is null)
            {
                ViewBag.Error = "Email o contraseña incorrectos";
                return View(loginInput);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, $"{resultado.Nombre} {resultado.Apellido}"),
                new Claim(ClaimTypes.Email, resultado.Email),
                new Claim(ClaimTypes.Role, resultado.Rol),
                new Claim("Token", resultado.Token)
            };

            var identidad = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identidad));

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(Login));
        }

        [HttpGet]
        public IActionResult AccesoDenegado()
        {
            return View();
        }
    }
}
