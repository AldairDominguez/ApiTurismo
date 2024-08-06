using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TurismoApp.Common.DTO;
using TurismoApp.Common.DTO.ClientesDtos;
using TurismoApp.Infraestructure.Context;
using TurismoApp.Infraestructure.Entities;
using TurismoApp.Infraestructure.Repositories.Interfaces;

namespace TurismoApp.Infraestructure.Repositories.Implementations;

public class ClienteRepository : IClienteRepository
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public ClienteRepository(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ClienteResponseDto>> GetAllClientesAsync()
    {
        var clientes = await _context.Clientes.Where(c => !c.Eliminado).ToListAsync();
        return _mapper.Map<IEnumerable<ClienteResponseDto>>(clientes);
    }

    public async Task<ClienteDto> GetClienteByIdAsync(int id)
    {
        var cliente = await _context.Clientes.Where(c => c.Id == id && !c.Eliminado).FirstOrDefaultAsync();
        return _mapper.Map<ClienteDto>(cliente);
    }
    public async Task<ClienteDto> GetClienteVerificationByIdAsync(int id)
    {
        var cliente = await _context.Clientes.Where(c => c.Id == id && !c.Eliminado).FirstOrDefaultAsync();
        return _mapper.Map<ClienteDto>(cliente);
    }

    public async Task AddClienteAsync(CreateClienteDto clienteDto)
    {
        var cliente = _mapper.Map<Cliente>(clienteDto);
        cliente.VerificacionToken = Guid.NewGuid().ToString();
        _context.Clientes.Add(cliente);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateClienteAsync(int id, UpdateClienteDto clienteDto)
    {
        var clienteExistente = await _context.Clientes.FindAsync(id);
        if (clienteExistente != null)
        {
            _mapper.Map(clienteDto, clienteExistente);
            _context.Entry(clienteExistente).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteClienteAsync(int id)
    {
        var cliente = await _context.Clientes.FindAsync(id);
        if (cliente != null)
        {
            cliente.Eliminado = true;
            _context.Clientes.Update(cliente);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> DniExistsAsync(string dni, int? excludedClientId = null)
    {
        var query = _context.Clientes.Where(c => !c.Eliminado);

        if (excludedClientId.HasValue)
        {
            query = query.Where(c => c.Dni == dni && c.Id != excludedClientId.Value);
        }
        else
        {
            query = query.Where(c => c.Dni == dni);
        }
        return await query.AnyAsync();
    }

    public async Task<bool> CorreoExistsAsync(string correo, int? excludedClientId = null)
    {
        var query = _context.Clientes.Where(c => !c.Eliminado);
        if (excludedClientId.HasValue)
        {
            query = query.Where(c => c.Correo == correo && c.Id != excludedClientId.Value);
        }
        else
        {
            query = query.Where(c => c.Correo == correo);
        }
        return await query.AnyAsync();
    }

    public async Task UpdateClienteVerificationAsync(int id, VerifyClienteDto clienteDto)
    {
        var cliente = await _context.Clientes.FindAsync(id);
        if (cliente != null)
        {
            cliente.Verificado = clienteDto.Verificado;
            cliente.VerificacionToken = clienteDto.VerificacionToken;
            cliente.FechaVerificacion = clienteDto.FechaVerificacion;
            await _context.SaveChangesAsync();
        }
    }
}