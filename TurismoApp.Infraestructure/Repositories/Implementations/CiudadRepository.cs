using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TurismoApp.Common.DTO.CiudadDtos;
using TurismoApp.Infraestructure.Context;
using TurismoApp.Infraestructure.Entities;
using TurismoApp.Infraestructure.Repositories.Interfaces;

namespace TurismoApp.Infraestructure.Repositories.Implementations;

public class CiudadRepository : ICiudadRepository
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public CiudadRepository(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CiudadDto>> GetAllCiudadesAsync()
    {
        var ciudades = await _context.Ciudades
                                 .Include(c => c.Departamento)
                                 .Where(c => !c.Eliminado)
                                 .ToListAsync();
        return _mapper.Map<IEnumerable<CiudadDto>>(ciudades);
    }

    public async Task<CiudadDto> GetCiudadByIdAsync(int id)
    {
        var ciudad = await _context.Ciudades
                              .Include(c => c.Departamento)
                              .Where(c => !c.Eliminado && c.Id == id)
                              .FirstOrDefaultAsync();
        return _mapper.Map<CiudadDto>(ciudad);
    }

    public async Task AddCiudadAsync(CreateCiudadDto ciudadDto)
    {
        var ciudad = _mapper.Map<Ciudad>(ciudadDto);
        _context.Ciudades.Add(ciudad);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateCiudadAsync(int id, UpdateCiudadDto ciudadDto)
    {
        var ciudad = await _context.Ciudades.FindAsync(id);
        if (ciudad != null)
        {
            _mapper.Map(ciudadDto, ciudad);
            _context.Entry(ciudad).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteCiudadAsync(int id)
    {
        var ciudad = await _context.Ciudades.FindAsync(id);
        if (ciudad != null)
        {
            ciudad.Eliminado = true;
            _context.Ciudades.Update(ciudad);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> CodigoExistsAsync(string codigo)
    {
        return await _context.Ciudades
        .AnyAsync(c => c.Codigo == codigo && !c.Eliminado);
    }

    public async Task<bool> ExistsCiudadAsync(string descripcion, int departamentoId)
    {
        return await _context.Ciudades
         .AnyAsync(c => c.Descripcion.ToLower() == descripcion.ToLower() && c.DepartamentoId == departamentoId && !c.Eliminado);
    }

    public async Task<bool> ExistsCiudadInOtherDepartamentoAsync(string descripcion, int departamentoId)
    {
        return await _context.Ciudades
         .AnyAsync(c => c.Descripcion.ToLower() == descripcion.ToLower() && c.DepartamentoId != departamentoId && !c.Eliminado);
    }

}