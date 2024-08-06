using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using TurismoApp.Application.Interfaces;
using TurismoApp.Common.DTO;
using TurismoApp.Common.DTO.DepartamentoDtos;

namespace ApiTurismo.Controllers
{
    [ApiController]
    [Route("api/Departamentos")]
    public class DepartamentosController : ControllerBase
    {
        private readonly IDepartamentoApplication _departamentoApplication;

        public DepartamentosController(IDepartamentoApplication departamentoApplication)
        {
            _departamentoApplication = departamentoApplication;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DepartamentoDto>>> GetAllDepartamentos()
        {
            var departamentos = await _departamentoApplication.GetAllDepartamentosAsync();

            return Ok(departamentos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DepartamentoDto>> GetDepartamentoById(int id)
        {
            var departamento = await _departamentoApplication.GetDepartamentoByIdAsync(id);
            if (departamento == null)
            {
                return NotFound("Departamento no encontrado");
            }
            return Ok(departamento);
        }

        [HttpPost]
        public async Task<ActionResult> AddDepartamento(UpdateDepartamentoDto departamento)
        {
            var result = await _departamentoApplication.AddDepartamentoAsync(departamento);
            if (result.IsValid)
            {
                return CreatedAtAction(nameof(GetDepartamentoById), new { id = result.Data }, result.Message);
            }
            else
            {
                return StatusCode(500, result.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDepartamento(int id, UpdateDepartamentoDto departamento)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _departamentoApplication.UpdateDepartamentoAsync(id, departamento);
            if (result.IsValid)
            {
                return Ok(result.Message);
            }
            else if (result.Message == "Departamento no encontrado")
            {
                return NotFound(result.Message);
            }
            else
            {
                return StatusCode(500, result.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartamento(int id)
        {
            var result = await _departamentoApplication.DeleteDepartamentoAsync(id);
            if (result.IsValid)
            {
                return Ok(result.Message);
            }
            else if (result.Message == "Departamento no encontrado")
            {
                return NotFound(result.Message);
            }
            else
            {
                return StatusCode(500, result.Message);
            }
        }
    }
}