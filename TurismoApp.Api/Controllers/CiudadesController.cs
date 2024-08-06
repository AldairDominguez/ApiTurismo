using Microsoft.AspNetCore.Mvc;
using TurismoApp.Application.Interfaces;
using TurismoApp.Common.DTO.CiudadDtos;

namespace ApiTurismo.Controllers
{
    [ApiController]
    [Route("api/Ciudades")]
    public class CiudadesController : ControllerBase
    {
        private readonly ICiudadApplication _ciudadApplication;

        public CiudadesController(ICiudadApplication ciudadApplication)
        {
            _ciudadApplication = ciudadApplication;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CiudadDto>>> GetAllCiudades()
        {
            var ciudades = await _ciudadApplication.GetAllCiudadesAsync();
            return Ok(ciudades);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CiudadDto>> GetCiudadById(int id)
        {
            var result = await _ciudadApplication.GetCiudadByIdAsync(id);
            if (!result.IsValid)
            {
                return NotFound(result.Message);
            }
            return Ok(result.Data);
        }

        [HttpPost]
        public async Task<ActionResult> AddCiudad(CreateCiudadDto ciudad)
        {
            var result = await _ciudadApplication.AddCiudadAsync(ciudad);
            if (result.IsValid)
            {
                return CreatedAtAction(nameof(GetCiudadById), new { id = result.Data }, result.Message);
            }
            else
            {
                return StatusCode(500, result.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCiudad(int id, UpdateCiudadDto ciudad)
        {
            var result = await _ciudadApplication.UpdateCiudadAsync(id, ciudad);
            if (result.IsValid)
            {
                return Ok(result.Message); 
            }
            else
            {
                if (result.Message == "Ciudad no encontrada")
                {
                    return NotFound(result.Message);
                }
                return StatusCode(500, result.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCiudad(int id)
        {
            var result = await _ciudadApplication.DeleteCiudadAsync(id);
            if (result.IsValid)
            {
                return Ok(result.Message);
            }
            else
            {
                if (result.Message == "Ciudad no encontrada")
                {
                    return NotFound(result.Message);
                }
                return StatusCode(500, result.Message);
            }
        }
    }
}
