using TurismoApp.Infraestructure.Context;
using TurismoApp.Infraestructure.Entities;
using TurismoApp.Infraestructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using TurismoApp.Common.DTO;
using AutoMapper;
using TurismoApp.Common.DTO.DepartamentoDtos;


namespace TurismoApp.Infraestructure.Repositories.Implementations;

public class DepartamentoRepository : IDepartamentoRepository
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public DepartamentoRepository(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<DepartamentoDto>> GetAllDepartamentosAsync()
    {
        var departamentos = await _context.Departamentos
            .Where(d => !d.Eliminado)
            .ToListAsync();

        return _mapper.Map<IEnumerable<DepartamentoDto>>(departamentos);
    }

    public async Task<DepartamentoDto> GetDepartamentoByIdAsync(int id)
    {
        var departamento = await _context.Departamentos
                                     .Where(d => d.Id == id && !d.Eliminado)
                                     .FirstOrDefaultAsync();

        return _mapper.Map<DepartamentoDto>(departamento);
    }

    public async Task<DepartamentoDto> AddDepartamentoAsync(UpdateDepartamentoDto departamentoDto)
    {
        var departamento = _mapper.Map<Departamento>(departamentoDto);
        _context.Departamentos.Add(departamento);
        await _context.SaveChangesAsync();
        return _mapper.Map<DepartamentoDto>(departamento);
    }

    public async Task UpdateDepartamentoAsync(int id, UpdateDepartamentoDto departamentoDto)
    {
        var departamento = await _context.Departamentos.FindAsync(id);
        if (departamento != null)
        {
            _mapper.Map(departamentoDto, departamento);
            _context.Entry(departamento).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteDepartamentoAsync(int id)
    {
        var departamento = await _context.Departamentos.FindAsync(id);
        if (departamento != null)
        {
            departamento.Eliminado = true;
            _context.Departamentos.Update(departamento);
            await _context.SaveChangesAsync();
        }
    }
    public async Task<bool> ExistsDepartamentoAsync(string descripcion, int? excludeId = null)
    {
        var query = _context.Departamentos.AsQueryable();

        query = query.Where(d => !d.Eliminado);

        if (excludeId.HasValue)
        {
            query = query.Where(d => d.Id != excludeId.Value);
        }
        return await query.AnyAsync(d => d.Descripcion.ToLower() == descripcion.ToLower());
    }
}

