using TurismoApp.Common.DTO;
using TurismoApp.Common.DTO.DepartamentoDtos;
using TurismoApp.Infraestructure.Entities;

namespace TurismoApp.Infraestructure.Repositories.Interfaces;

public interface IDepartamentoRepository
{
    Task<IEnumerable<DepartamentoDto>> GetAllDepartamentosAsync();

    Task<DepartamentoDto> GetDepartamentoByIdAsync(int id);

    Task<DepartamentoDto> AddDepartamentoAsync(UpdateDepartamentoDto departamentoDto);

    Task UpdateDepartamentoAsync(int id, UpdateDepartamentoDto departamentoDto);

    Task DeleteDepartamentoAsync(int id);
    Task<bool> ExistsDepartamentoAsync(string descripcion, int? excludeId = null);
}