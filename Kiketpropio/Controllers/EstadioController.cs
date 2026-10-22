using Kiketpropio.Dominio.Entidades;
using Kiketpropio.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kiketpropio.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class EstadioController : ControllerBase
    {
        private readonly IEstadioService _estadioService;

        public EstadioController(IEstadioService estadioService)
        {
            _estadioService = estadioService;
        }

        [HttpGet]
        public IActionResult getAll()
        {
            return Ok(_estadioService.GetAll());
        }

        [HttpGet("{id}")]
        public IActionResult GetOne(int id)
        {
            var estadioFounded = _estadioService.GetOne(id);

            if (estadioFounded is null) return NotFound($"No se ha encontrado un estadio con ID {id}");

            return Ok(estadioFounded);

        }

        [Authorize(Roles = "Admin,Vendedor")]
        [HttpPost]
        public IActionResult CreateEstadio([FromBody] Estadio estadioInput)
        {
            var estadioCreado = _estadioService.Create(estadioInput);

            return CreatedAtAction(nameof(GetOne), new { id = estadioCreado.IdEstadio }, estadioCreado);
        }

        [Authorize(Roles = "Admin,Vendedor")]
        [HttpPut("{id}")]
        public IActionResult UpdateEstadio([FromBody] Estadio estadioInput, int id)
        {
            if (string.IsNullOrWhiteSpace(estadioInput.Nombre)) return BadRequest("El nombre no pued estar vacio");

            var resul = _estadioService.Update(estadioInput, id);

            if (resul is null) return NotFound($"No se ha encontrado un estadio con ID {id}");

            return Ok(resul);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public IActionResult DeleteEstadio(int id)
        {
            var result = _estadioService.Delete(id);

            if(!result)return NotFound($"No se ha encontrado un estadio con ID {id}");

            return NoContent();
        }
    }
}
