using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TurismoApp.Common.DTO;
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

    public async Task<IEnumerable<ClienteDto>> GetAllClientesAsync()
    {
        var clientes = await _context.Clientes.ToListAsync();
        return _mapper.Map<IEnumerable<ClienteDto>>(clientes);
    }

    public async Task<ClienteDto> GetClienteByIdAsync(int id)
    {
        var cliente = await _context.Clientes.FindAsync(id);
        return _mapper.Map<ClienteDto>(cliente);
    }

    public async Task AddClienteAsync(CreateClienteDto clienteDto)
    {
        var cliente = _mapper.Map<Cliente>(clienteDto);
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
            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> DniExistsAsync(string dni, int? excludedClientId = null)
    {
        if (excludedClientId.HasValue)
        {
            return await _context.Clientes.AnyAsync(c => c.Dni == dni && c.Id != excludedClientId.Value);
        }
        return await _context.Clientes.AnyAsync(c => c.Dni == dni);
    }

    public async Task<bool> CorreoExistsAsync(string correo, int? excludedClientId = null)
    {
        if (excludedClientId.HasValue)
        {
            return await _context.Clientes.AnyAsync(c => c.Correo == correo && c.Id != excludedClientId.Value);
        }
        return await _context.Clientes.AnyAsync(c => c.Correo == correo);
    }


}