using Microsoft.EntityFrameworkCore;
using TurismoApp.Infraestructure.Context;
using TurismoApp.Infraestructure.Entities;
using TurismoApp.Infraestructure.Repositories.Interfaces;

namespace TurismoApp.Infraestructure.Repositories.Implementations
{
    public class RecorridoRepository : IRecorridoRepository
    {
        private readonly AppDbContext _context;

        public RecorridoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Recorrido>> GetAllAsync()
        {
            return await _context.Recorridos
                .Include(r => r.ClienteRecorridos)
                    .ThenInclude(cr => cr.Cliente)
                .ToListAsync();
        }

        public async Task<Recorrido> GetByIdAsync(int id)
        {
            return await _context.Recorridos
                 .Include(r => r.CiudadOrigen)
                 .Include(r => r.CiudadDestino)
                 .Include(r => r.ClienteRecorridos)
                     .ThenInclude(cr => cr.Cliente)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task AddAsync(Recorrido recorrido)
        {
            await _context.Recorridos.AddAsync(recorrido);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Recorrido recorrido)
        {
            _context.Recorridos.Update(recorrido);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var recorrido = await _context.Recorridos.FindAsync(id);
            if (recorrido != null)
            {
                _context.Recorridos.Remove(recorrido);
                await _context.SaveChangesAsync();
            }
        }
    }
}