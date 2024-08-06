using TurismoApp.Common;
using TurismoApp.Common.DTO;
using TurismoApp.Common.DTO.DepartamentoDtos;

namespace TurismoApp.Application.Interfaces;

public interface IDepartamentoApplication
{
    Task<IEnumerable<DepartamentoDto>> GetAllDepartamentosAsync();

    Task<DepartamentoDto> GetDepartamentoByIdAsync(int id);

    Task<ResponseDto> AddDepartamentoAsync(UpdateDepartamentoDto departamento);

    Task<ResponseDto> UpdateDepartamentoAsync(int id, UpdateDepartamentoDto departamento);

    Task<ResponseDto> DeleteDepartamentoAsync(int id);

}