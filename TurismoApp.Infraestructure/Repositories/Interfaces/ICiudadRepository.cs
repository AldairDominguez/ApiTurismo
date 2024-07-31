using TurismoApp.Common.DTO;

namespace TurismoApp.Infraestructure.Repositories.Interfaces;

public interface ICiudadRepository
{
    Task<IEnumerable<CiudadDto>> GetAllCiudadesAsync();

    Task<CiudadDto> GetCiudadByIdAsync(int id);

    Task AddCiudadAsync(CreateCiudadDto ciudadDto);

    Task UpdateCiudadAsync(int id, UpdateCiudadDto ciudadDto);

    Task DeleteCiudadAsync(int id);

    Task<bool> CodigoExistsAsync(string codigo);
}