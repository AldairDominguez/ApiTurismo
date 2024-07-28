using Infraestructure.Context;
using Infraestructure.Entities;
using Infraestructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace Infraestructure.Repositories.Implementations
{
    public class CiudadRepository : ICiudadRepository
    {
        private readonly AppDbContext _context;

        public CiudadRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Ciudad>> GetAllCiudadesAsync()
        {
            return await _context.Ciudades.ToListAsync();
        }

        public async Task<Ciudad> GetCiudadByIdAsync(int id)
        {
            return await _context.Ciudades.FindAsync(id);
        }

        public async Task AddCiudadAsync(Ciudad ciudad)
        {
            _context.Ciudades.Add(ciudad);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCiudadAsync(Ciudad ciudad)
        {
            _context.Entry(ciudad).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCiudadAsync(int id)
        {
            var ciudad = await _context.Ciudades.FindAsync(id);
            if (ciudad != null)
            {
                _context.Ciudades.Remove(ciudad);
                await _context.SaveChangesAsync();
            }
        }
    }
}
