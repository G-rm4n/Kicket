using Kiketpropio.Dominio.Entidades;
using Kiketpropio.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Kiketpropio.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _UsuarioService;

        public UsuarioController(IUsuarioService usuarioService)
        {
            _UsuarioService = usuarioService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_UsuarioService.GetAll());
        }

        [HttpGet("{id}")]
        public IActionResult GetOne(int id)
        {
            var UserFound = _UsuarioService.GetOne(id);
            if (UserFound is null) return NotFound($"Usuario con ID {id} no encontrado");

            return Ok(UserFound);
        }

        [HttpPost]
        public IActionResult CreateUser([FromBody] Usuario userInput)
        {
            if (string.IsNullOrWhiteSpace(userInput.Pass) || string.IsNullOrWhiteSpace(userInput.Email))
                return BadRequest("El email y contraseña son obligatorios");

            var userCreated = _UsuarioService.Create(userInput);
            return CreatedAtAction(nameof(GetOne), new { id = userCreated.IdUsuario }, userCreated);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateUser([FromBody] Usuario userInput,int id)
        {
            if (string.IsNullOrWhiteSpace(userInput.Pass) || string.IsNullOrWhiteSpace(userInput.Email))
                return BadRequest("El email y contraseña son obligatorios");

            var result = _UsuarioService.Update(userInput, id);
            if (result is null) return NotFound($"Usuario con ID {id} no encontrado\"");

            return Ok(userInput);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            var deleted = _UsuarioService.Delete(id);
            if (!deleted) return NotFound($"Usuario con ID {id} no encontrado.");
            return NoContent();
        }
    }
}
