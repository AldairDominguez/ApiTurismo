using TurismoApp.Common.DTO;
using TurismoApp.Common;
using TurismoApp.Common.Enums;
using TurismoApp.Infraestructure.Entities;

namespace TurismoApp.Application.Interfaces;

public interface IRecorridoApplication
{
    Task<IEnumerable<RecorridoDto>> GetAllAsync();
    Task<RecorridoDto> GetByIdAsync(int id);
    Task<ResponseDto> AddAsync(CreateRecorridoDto createRecorridoDto);
    Task<ResponseDto> UpdateAsync(int id, UpdateRecorridoDto updateRecorridoDto); 
    Task<ResponseDto> DeleteAsync(int id);
    Task<string> GenerateCodigo(int ciudadOrigenId, int ciudadDestinoId, DateTime fechaViaje);
    Task<double> CalculateDistance(int ciudadOrigenId, int ciudadDestinoId);
    double CalculatePrice(double distancia, DateTime fechaViaje);
    Task<bool> ExisteRecorridoConMismasCiudadesYFecha(int ciudadOrigenId, int ciudadDestinoId, DateTime fechaViaje, int? excludeId = null);
    Task<ResponseDto> UpdateEstadoAsync(int id, EstadoRecorrido nuevoEstado);
    Task<IEnumerable<RecorridoDto>> GetRecorridosByEstadoAsync(EstadoRecorrido? estado);
    Task<IEnumerable<RecorridoDto>> GetRecorridosByFechaAsync(DateTime fechaInicio, DateTime fechaFin);

    Task<RecorridoDto> GetRecorridoByCodigoAsync(string codigo);
}