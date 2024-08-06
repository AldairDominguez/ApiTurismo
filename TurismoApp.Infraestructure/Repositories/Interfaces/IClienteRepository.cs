using TurismoApp.Common.DTO;
using TurismoApp.Common.DTO.ClientesDtos;
using TurismoApp.Infraestructure.Entities;

namespace TurismoApp.Infraestructure.Repositories.Interfaces;

public interface IClienteRepository
{
    Task<IEnumerable<ClienteResponseDto>> GetAllClientesAsync();

    Task<ClienteDto> GetClienteByIdAsync(int id);

    Task<ClienteDto> GetClienteVerificationByIdAsync(int id);

    Task AddClienteAsync(CreateClienteDto clienteDto);

    Task UpdateClienteAsync(int id, UpdateClienteDto clienteDto);

    Task UpdateClienteVerificationAsync(int id, VerifyClienteDto clienteDto);


    Task DeleteClienteAsync(int id);

    Task<bool> DniExistsAsync(string dni, int? excludedClientId = null);
    Task<bool> CorreoExistsAsync(string correo, int? excludedClientId = null);
}