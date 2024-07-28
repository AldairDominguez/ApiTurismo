using Infraestructure.Entities;


namespace Infraestructure.Repositories.Interfaces
{
    public interface ICiudadRepository
    {
        Task<IEnumerable<Ciudad>> GetAllCiudadesAsync();
        Task<Ciudad> GetCiudadByIdAsync(int id);
        Task AddCiudadAsync(Ciudad ciudad);
        Task UpdateCiudadAsync(Ciudad ciudad);
        Task DeleteCiudadAsync(int id);
    }
}
