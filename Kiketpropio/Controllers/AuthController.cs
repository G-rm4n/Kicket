using Kiketpropio.Dominio.DTOs;
using Kiketpropio.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kiketpropio.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest loginInput)
        {
            if (string.IsNullOrWhiteSpace(loginInput.Email) || string.IsNullOrWhiteSpace(loginInput.Pass))
                return BadRequest("El email y contraseña son obligatorios");

            var result = _authService.Login(loginInput);
            if (result is null) return Unauthorized("Email o contraseña incorrectos");

            return Ok(result);
        }

        [Authorize]
        [HttpGet("perfil")]
        public IActionResult GetPerfil()
        {
            return Ok(new
            {
                Email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value,
                Nombre = User.Identity?.Name,
                Rol = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value
            });
        }
    }
}
