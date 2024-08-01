using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TurismoApp.Application.Interfaces;
using TurismoApp.Common.DTO;

namespace TurismoApp.Api.Controllers
{
    [ApiController]
    [Route("api/Clientes")]
    public class ClientesController : ControllerBase
    {
        private readonly IClienteApplication _clienteApplication;

        public ClientesController(IClienteApplication clienteApplication)
        {
            _clienteApplication = clienteApplication;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClienteDto>>> GetAllClientes()
        {
            var clientes = await _clienteApplication.GetAllClientesAsync();
            return Ok(clientes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ClienteDto>> GetClienteById(int id)
        {
            var result = await _clienteApplication.GetClienteByIdAsync(id);
            if (!result.IsValid)
            {
                return NotFound(result.Message);
            }
            return Ok(result.Data);
        }

        [HttpPost]
        public async Task<ActionResult> AddCliente(CreateClienteDto cliente)
        {
            var result = await _clienteApplication.AddClienteAsync(cliente);
            if (result.IsValid)
            {
                return CreatedAtAction(nameof(GetClienteById), new { id = result.Data }, result.Message);
            }
            return StatusCode(500, result.Message);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCliente(int id, UpdateClienteDto cliente)
        {
            var result = await _clienteApplication.UpdateClienteAsync(id, cliente);
            if (result.IsValid)
            {
                return Ok(result.Message);
            }
            return result.Message == "Cliente no encontrado" ? NotFound(result.Message) : StatusCode(500, result.Message);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCliente(int id)
        {
            var result = await _clienteApplication.DeleteClienteAsync(id);
            if (result.IsValid)
            {
                return Ok(result.Message);
            }
            return result.Message == "Cliente no encontrado" ? NotFound(result.Message) : StatusCode(500, result.Message);
        }
    }
}