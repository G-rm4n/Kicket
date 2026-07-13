using Microsoft.AspNetCore.Mvc;
using Kiketpropio.Services.Interfaces;
using Kiketpropio.Dominio.Entidades;

namespace Kiketpropio.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClubController : ControllerBase
    {
        private readonly IClubService _clubService;

        public ClubController(IClubService clubService)
        {
            _clubService = clubService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_clubService.GetAll()); 
        }

        [HttpGet("{id}")]
        public IActionResult GetOne(int id)
        {
            var ClubFounded = _clubService.GetOne(id);
            if (ClubFounded is null) return NotFound($"Club con ID {id} encontrado");

            return Ok(ClubFounded);
        }

        [HttpPut("{id}")]
        public IActionResult Update([FromBody] Club clubInput, int id)
        {
            if (string.IsNullOrWhiteSpace(clubInput.Nombre) || string.IsNullOrWhiteSpace(clubInput.Logo_Url))
                return BadRequest("El nombre o URL no pueden estar vacios");

            var result = _clubService.Update(clubInput, id);

            if (result is null) return NotFound($"Club con ID {id} encontrado");
            return Ok(result);
        }

        [HttpPost]
        public IActionResult CreateClub([FromBody] Club clubInput)
        {
            var createdClub = _clubService.Create(clubInput);

            return CreatedAtAction(nameof(GetOne), new { id = createdClub.IdClub }, createdClub);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteClub(int id)
        {
            var result = _clubService.Delete(id);
            if (!result) return NotFound($"Club con ID {id} encontrado");

            return NoContent();
        }
    }
}
