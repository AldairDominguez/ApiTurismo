using AutoMapper;
using Infraestructure.Entities;
using Infraestructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Common.DTO;

namespace ApiTurismo.Controllers
{
    [ApiController]
    [Route("api/Departamentos")]

    public class DepartamentosController : ControllerBase
    {
        private readonly IDepartamentoRepository _departamentoRepository;
        private readonly IMapper _mapper;

        public DepartamentosController(IDepartamentoRepository departamentoRepository, IMapper mapper)
        {
            _departamentoRepository = departamentoRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DepartamentoDto>>> GetAllDepartamentos()
        {
            var departamentos = await _departamentoRepository.GetAllDepartamentosAsync();
            var departamentosDto = _mapper.Map<IEnumerable<DepartamentoDto>>(departamentos);
            return Ok(departamentosDto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DepartamentoDto>> GetDepartamentoById(int id)
        {
            var departamento = await _departamentoRepository.GetDepartamentoByIdAsync(id);
            if (departamento == null)
            {
                return NotFound();
            }
            var departamentoDto = _mapper.Map<DepartamentoDto>(departamento);
            return Ok(departamentoDto);
        }

        [HttpPost]
        public async Task<ActionResult<DepartamentoDto>> AddDepartamento(DepartamentoDto departamentoDto)
        {
            var departamento = _mapper.Map<Departamento>(departamentoDto);
            await _departamentoRepository.AddDepartamentoAsync(departamento);
            return CreatedAtAction(nameof(GetDepartamentoById), new { id = departamento.Id }, departamentoDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDepartamento(int id, DepartamentoDto departamentoDto)
        {
            if (id != departamentoDto.Id)
            {
                return BadRequest();
            }

            var departamento = _mapper.Map<Departamento>(departamentoDto);
            await _departamentoRepository.UpdateDepartamentoAsync(departamento);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartamento(int id)
        {
            await _departamentoRepository.DeleteDepartamentoAsync(id);
            return NoContent();
        }
    }
}