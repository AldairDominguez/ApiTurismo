using Infraestructure.Context;
using Infraestructure.Entities;
using Infraestructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Repositories.Implementations
{
    public class DepartamentoRepository : IDepartamentoRepository
    {
        private readonly AppDbContext _context;

        public DepartamentoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Departamento>> GetAllDepartamentosAsync()
        {
            return await _context.Departamentos.ToListAsync();
        }

        public async Task<Departamento> GetDepartamentoByIdAsync(int id)
        {
            return await _context.Departamentos.FindAsync(id);
        }

        public async Task AddDepartamentoAsync(Departamento departamento)
        {
            _context.Departamentos.Add(departamento);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateDepartamentoAsync(Departamento departamento)
        {
            _context.Entry(departamento).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteDepartamentoAsync(int id)
        {
            var departamento = await _context.Departamentos.FindAsync(id);
            if (departamento != null)
            {
                _context.Departamentos.Remove(departamento);
                await _context.SaveChangesAsync();
            }
        }
    }
}
