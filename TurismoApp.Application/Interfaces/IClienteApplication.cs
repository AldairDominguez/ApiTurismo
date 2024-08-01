using TurismoApp.Common.DTO;
using TurismoApp.Common;

namespace TurismoApp.Application.Interfaces;

public interface IClienteApplication
{
    Task<IEnumerable<ClienteDto>> GetAllClientesAsync();
    Task<ResponseDto> GetClienteByIdAsync(int id);
    Task<ResponseDto> AddClienteAsync(CreateClienteDto clienteDto);
    Task<ResponseDto> UpdateClienteAsync(int id, UpdateClienteDto clienteDto);
    Task<ResponseDto> DeleteClienteAsync(int id);
}