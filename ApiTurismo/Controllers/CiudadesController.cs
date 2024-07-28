using Microsoft.AspNetCore.Mvc;
using Infraestructure.Entities;
using Infraestructure.Repositories.Interfaces;

namespace ApiTurismo.Controllers
{
    [ApiController]
    [Route("api/Ciudades")]
   
    public class CiudadesController : ControllerBase
    {
        private readonly ICiudadRepository _ciudadRepository;

        public CiudadesController(ICiudadRepository ciudadRepository)
        {
            _ciudadRepository = ciudadRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ciudad>>> GetCiudades()
        {
            var ciudades = await _ciudadRepository.GetAllCiudadesAsync();
            return Ok(ciudades);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Ciudad>> GetCiudad(int id)
        {
            var ciudad = await _ciudadRepository.GetCiudadByIdAsync(id);
            if (ciudad == null)
            {
                return NotFound();
            }
            return Ok(ciudad);
        }

        [HttpPost]
        public async Task<ActionResult<Ciudad>> PostCiudad(Ciudad ciudad)
        {
            await _ciudadRepository.AddCiudadAsync(ciudad);
            return CreatedAtAction(nameof(GetCiudad), new { id = ciudad.Id }, ciudad);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCiudad(int id, Ciudad ciudad)
        {
            if (id != ciudad.Id)
            {
                return BadRequest();
            }
            await _ciudadRepository.UpdateCiudadAsync(ciudad);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCiudad(int id)
        {
            await _ciudadRepository.DeleteCiudadAsync(id);
            return NoContent();
        }
    }
}
