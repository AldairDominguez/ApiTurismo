using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TurismoApp.Application.Interfaces;
using TurismoApp.Common.DTO;
using TurismoApp.Common.Enums;

namespace TurismoApp.Api.Controllers
{
    [ApiController]
    [Route("api/Recorridos")]
    public class RecorridoController : ControllerBase
    {
        private readonly IRecorridoApplication _recorridoApplication;
        private readonly IMapper _mapper;


        public RecorridoController(IRecorridoApplication recorridoApplication, IMapper mapper)
        {
            _recorridoApplication = recorridoApplication;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetRecorridos()
        {
            var recorridos = await _recorridoApplication.GetAllAsync();
            var recorridoDtos = _mapper.Map<IEnumerable<RecorridoDto>>(recorridos);
            return Ok(recorridoDtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRecorridoById(int id)
        {
            var recorrido = await _recorridoApplication.GetByIdAsync(id);
            if (recorrido == null)
            {
                return NotFound();
            }
            var recorridoDto = _mapper.Map<RecorridoDto>(recorrido);
            return Ok(recorridoDto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRecorrido(CreateRecorridoDto createRecorridoDto)
        {
            try
            {
                var result = await _recorridoApplication.AddAsync(createRecorridoDto);
                if (!result.IsValid)
                {
                    return BadRequest(result.Message);
                }

                var createdRecorrido = result.Data as RecorridoDto;
                if (createdRecorrido == null)
                {
                    return StatusCode(500, "Ocurrió un error al crear el recorrido.");
                }

                return CreatedAtAction(nameof(GetRecorridoById), new { id = createdRecorrido.Id }, result.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ocurrió un error inesperado.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRecorrido(int id, UpdateRecorridoDto updateRecorridoDto)
        {

            var result = await _recorridoApplication.UpdateAsync(id, updateRecorridoDto);
            if (!result.IsValid)
            {
                return BadRequest(result.Message);
            }

            return Ok(result.Message);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRecorrido(int id)
        {
            var result = await _recorridoApplication.DeleteAsync(id);
            if (!result.IsValid)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.Message);
        }

        [HttpPut("{id}/estado")]
        public async Task<IActionResult> UpdateRecorridoEstado(int id, [FromBody] EstadoRecorrido nuevoEstado)
        {
            if (nuevoEstado != EstadoRecorrido.EnProgreso && nuevoEstado != EstadoRecorrido.Finalizado)
            {
                return BadRequest("El estado solo puede ser actualizado a 'En progreso' o 'Finalizado'.");
            }

            var result = await _recorridoApplication.UpdateEstadoAsync(id, nuevoEstado);
            if (!result.IsValid)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.Message);
        }

        [HttpGet("FiltroRecorridos")]
        public async Task<IActionResult> GetRecorridos([FromQuery] EstadoRecorrido? estado)
        {
            var result = await _recorridoApplication.GetRecorridosByEstadoAsync(estado);
            return Ok(result);
        }

        [HttpGet("FiltroFechas")]
        public async Task<IActionResult> GetRecorridosByFechaAsync([FromQuery] DateTime fechaInicio, [FromQuery] DateTime fechaFin)
        {
            var result = await _recorridoApplication.GetRecorridosByFechaAsync(fechaInicio, fechaFin);
            return Ok(result);
        }

        [HttpGet("FiltroCodigo")]
        public async Task<IActionResult> GetRecorridoByCodigoAsync([FromQuery] string codigo)
        {
            var result = await _recorridoApplication.GetRecorridoByCodigoAsync(codigo);
            if (result == null)
            {
                return NotFound($"No se encontró un recorrido con el código: {codigo}");
            }
            return Ok(result);
        }
    }
}