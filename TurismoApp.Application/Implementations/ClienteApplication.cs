using AutoMapper;
using TurismoApp.Application.Interfaces;
using TurismoApp.Common;
using TurismoApp.Common.DTO;
using TurismoApp.Common.DTO.ClientesDtos;
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

    public async Task<IEnumerable<ClienteResponseDto>> GetAllClientesAsync()
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

        var clienteResponse = _mapper.Map<ClienteResponseDto>(cliente);
        return ResponseDto.Ok(clienteResponse);
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
        cliente.VerificacionToken = Guid.NewGuid().ToString();  

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

        var verifyClienteDto = _mapper.Map<VerifyClienteDto>(cliente);
        verifyClienteDto.Verificado = true;

        await _clienteRepository.UpdateClienteVerificationAsync(clientId, verifyClienteDto);

        return ResponseDto.Ok(null, "Correo verificado con éxito.");
    }

    public async Task<ResponseDto> SaveVerificationTokenAsync(int clientId, string token)
    {
        var cliente = await _clienteRepository.GetClienteByIdAsync(clientId);
        if (cliente == null)
        {
            return ResponseDto.Error("Cliente no encontrado.");
        }

        var verifyClienteDto = _mapper.Map<VerifyClienteDto>(cliente);
        verifyClienteDto.VerificacionToken = token;

        await _clienteRepository.UpdateClienteVerificationAsync(clientId, verifyClienteDto);
        return ResponseDto.Ok();
    }

    public async Task<bool> VerifyTokenAsync(int clientId, string token)
    {
        var cliente = await _clienteRepository.GetClienteByIdAsync(clientId);
        return cliente != null && cliente.VerificacionToken == token && !cliente.Eliminado;
    }

    public async Task<ResponseDto> UpdateClienteVerificationAsync(int clientId, VerifyClienteDto verifyClienteDto)
    {
        var clienteExistente = await _clienteRepository.GetClienteByIdAsync(clientId);
        if (clienteExistente == null)
        {
            return ResponseDto.Error("Cliente no encontrado.");
        }

        clienteExistente.Verificado = verifyClienteDto.Verificado;
        clienteExistente.VerificacionToken = verifyClienteDto.VerificacionToken;
        clienteExistente.FechaVerificacion = verifyClienteDto.FechaVerificacion;

        await _clienteRepository.UpdateClienteVerificationAsync(clienteExistente.Id, verifyClienteDto);
        return ResponseDto.Ok(null, "Verificación actualizada con éxito.");
    }

    public async Task<ResponseDto> GetClienteVerificationByIdAsync(int id)
    {
        var cliente = await _clienteRepository.GetClienteVerificationByIdAsync(id);
        if (cliente == null)
        {
            return ResponseDto.Error("Cliente no encontrado.");
        }

        return ResponseDto.Ok(cliente);
    }
}