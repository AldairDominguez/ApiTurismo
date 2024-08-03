using TurismoApp.Infraestructure.Entities;

namespace TurismoApp.Infraestructure.Repositories.Interfaces;

public interface IRecorridoRepository
{
    Task<IEnumerable<Recorrido>> GetAllAsync();
    Task<Recorrido> GetByIdAsync(int id);
    Task AddAsync(Recorrido recorrido);
    Task UpdateAsync(Recorrido recorrido);
    Task DeleteAsync(int id);
}