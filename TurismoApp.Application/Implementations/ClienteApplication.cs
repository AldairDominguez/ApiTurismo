using AutoMapper;
using TurismoApp.Application.Interfaces;
using TurismoApp.Common;
using TurismoApp.Common.DTO;
using TurismoApp.Infraestructure.Entities;
using TurismoApp.Infraestructure.Repositories.Interfaces;

namespace TurismoApp.Application.Implementations;

public class ClienteApplication : IClienteApplication
{
    private readonly IClienteRepository _clienteRepository;
    private readonly IMapper _mapper;

    public ClienteApplication(IClienteRepository clienteRepository, IMapper mapper)
    {
        _clienteRepository = clienteRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ClienteDto>> GetAllClientesAsync()
    {
        return await _clienteRepository.GetAllClientesAsync();
    }

    public async Task<ResponseDto> GetClienteByIdAsync(int id)
    {
        var cliente = await _clienteRepository.GetClienteByIdAsync(id);
        if (cliente == null)
        {
            return ResponseDto.Error("Cliente no encontrado.");
        }
        return ResponseDto.Ok(cliente);
    }

    public async Task<ResponseDto> AddClienteAsync(CreateClienteDto clienteDto)
    {
        if (await _clienteRepository.DniExistsAsync(clienteDto.Dni))
        {
            return ResponseDto.Error("El DNI ya existe.");
        }

        if (await _clienteRepository.CorreoExistsAsync(clienteDto.Correo))
        {
            return ResponseDto.Error("El correo ya existe.");
        }

        var cliente = _mapper.Map<Cliente>(clienteDto);
        await _clienteRepository.AddClienteAsync(clienteDto);
        var clienteResultDto = _mapper.Map<ClienteDto>(cliente);

        return ResponseDto.Ok(clienteResultDto, "Cliente creado con éxito.");
    }

    public async Task<ResponseDto> UpdateClienteAsync(int id, UpdateClienteDto clienteDto)
    {
        try
        {
            var clienteExistente = await _clienteRepository.GetClienteByIdAsync(id);
            if (clienteExistente == null)
            {
                return ResponseDto.Error("Cliente no encontrado.");
            }

            if (await _clienteRepository.DniExistsAsync(clienteDto.Dni, id))
            {
                return ResponseDto.Error("El DNI ya existe.");
            }

            if (await _clienteRepository.CorreoExistsAsync(clienteDto.Correo, id))
            {
                return ResponseDto.Error("El correo ya existe.");
            }


            await _clienteRepository.UpdateClienteAsync(id, clienteDto);
            return ResponseDto.Ok(message: "Cliente actualizado con éxito.");
        }
        catch (Exception ex)
        {
            return ResponseDto.Error(ex.Message);
        }
    }

    public async Task<ResponseDto> DeleteClienteAsync(int id)
    {
        try
        {
            var cliente = await _clienteRepository.GetClienteByIdAsync(id);
            if (cliente == null)
            {
                return ResponseDto.Error("Cliente no encontrado.");
            }

            await _clienteRepository.DeleteClienteAsync(id);
            return ResponseDto.Ok(message: "Cliente eliminado con éxito.");
        }
        catch (Exception ex)
        {
            return ResponseDto.Error(ex.Message);
        }
    }

    public async Task<ResponseDto> VerifyEmailAsync(int clientId)
    {
        var cliente = await _clienteRepository.GetClienteByIdAsync(clientId);
        if (cliente == null)
        {
            return ResponseDto.Error("Cliente no encontrado.");
        }

        cliente.IsVerified = true;
        var updateClienteDto = _mapper.Map<UpdateClienteDto>(cliente);
        await _clienteRepository.UpdateClienteAsync(clientId, updateClienteDto);

        return ResponseDto.Ok(null, "Correo verificado con éxito.");
    }

    public async Task<ResponseDto> SaveVerificationTokenAsync(int clientId, string token)
    {
        var cliente = await _clienteRepository.GetClienteByIdAsync(clientId);
        if (cliente == null)
        {
            return ResponseDto.Error("Cliente no encontrado.");
        }

        cliente.VerificationToken = token;

        var updateClienteDto = _mapper.Map<UpdateClienteDto>(cliente);
        await _clienteRepository.UpdateClienteAsync(clientId, updateClienteDto);
        return ResponseDto.Ok();
    }

    public async Task<bool> VerifyTokenAsync(int clientId, string token)
    {
        var cliente = await _clienteRepository.GetClienteByIdAsync(clientId);
        return cliente != null && cliente.VerificationToken == token;
    }
}