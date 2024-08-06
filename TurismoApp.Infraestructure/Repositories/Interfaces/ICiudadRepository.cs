using TurismoApp.Common.DTO.CiudadDtos;

namespace TurismoApp.Infraestructure.Repositories.Interfaces;

public interface ICiudadRepository
{
    Task<IEnumerable<CiudadDto>> GetAllCiudadesAsync();

    Task<CiudadDto> GetCiudadByIdAsync(int id);

    Task AddCiudadAsync(CreateCiudadDto ciudadDto);

    Task UpdateCiudadAsync(int id, UpdateCiudadDto ciudadDto);

    Task DeleteCiudadAsync(int id);

    Task<bool> CodigoExistsAsync(string codigo);

    Task<bool> ExistsCiudadAsync(string descripcion, int departamentoId);

    Task<bool> ExistsCiudadInOtherDepartamentoAsync(string descripcion, int departamentoId);
}