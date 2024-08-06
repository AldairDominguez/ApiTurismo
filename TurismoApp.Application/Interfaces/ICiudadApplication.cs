using TurismoApp.Common;
using TurismoApp.Common.DTO.CiudadDtos;

namespace TurismoApp.Application.Interfaces;

public interface ICiudadApplication
{
    Task<IEnumerable<CiudadDto>> GetAllCiudadesAsync();
    Task<ResponseDto> GetCiudadByIdAsync(int id);
    Task<ResponseDto> AddCiudadAsync(CreateCiudadDto ciudadDto);
    Task<ResponseDto> UpdateCiudadAsync(int id, UpdateCiudadDto ciudad);
    Task<ResponseDto> DeleteCiudadAsync(int id);
}