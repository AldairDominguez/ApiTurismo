using TurismoApp.Application.Interfaces;
using TurismoApp.Common.DTO;
using TurismoApp.Common;
using TurismoApp.Infraestructure.Repositories.Interfaces;
using TurismoApp.Infraestructure.Entities;
using AutoMapper;

namespace TurismoApp.Application.Implementations;

public class CiudadApplication : ICiudadApplication
{
    private readonly ICiudadRepository _ciudadRepository;
    private readonly IMapper _mapper;

    public CiudadApplication(ICiudadRepository ciudadRepository, IMapper mapper)
    {
        _ciudadRepository = ciudadRepository;
        _mapper = mapper;
    }

    public async Task<ResponseDto> AddCiudadAsync(CreateCiudadDto ciudadDto)
    {
        try
        {
            if (await _ciudadRepository.CodigoExistsAsync(ciudadDto.Codigo))
            {
                return ResponseDto.Error("El código ya existe.");
            }

            await _ciudadRepository.AddCiudadAsync(ciudadDto);
            return ResponseDto.Ok("Ciudad creada con éxito");
        }
        catch (Exception ex)
        {
            return ResponseDto.Error(ex.Message);
        }
    }

    public async Task<ResponseDto> DeleteCiudadAsync(int id)
    {
        try
        {
            var ciudad = await _ciudadRepository.GetCiudadByIdAsync(id);
            if (ciudad == null)
            {
                return ResponseDto.Error("Ciudad no encontrada");
            }

            await _ciudadRepository.DeleteCiudadAsync(id);
            return ResponseDto.Ok("Ciudad eliminada con éxito");
        }
        catch (Exception ex)
        {
            return ResponseDto.Error(ex.Message);
        }
    }

    public async Task<IEnumerable<CiudadDto>> GetAllCiudadesAsync()
    {
        return await _ciudadRepository.GetAllCiudadesAsync();
    }

    public async Task<ResponseDto> GetCiudadByIdAsync(int id)
    {
        var ciudad = await _ciudadRepository.GetCiudadByIdAsync(id);
        if (ciudad == null)
        {
            return ResponseDto.Error("Ciudad no encontrada");
        }
        return ResponseDto.Ok(ciudad);
    }

    public async Task<ResponseDto> UpdateCiudadAsync(int id, UpdateCiudadDto ciudadDto)
    {
        try
        {
            var ciudadExistente = await _ciudadRepository.GetCiudadByIdAsync(id);
            if (ciudadExistente == null)
            {
                return ResponseDto.Error("Ciudad no encontrada");
            }

            await _ciudadRepository.UpdateCiudadAsync(id, ciudadDto);
            return ResponseDto.Ok("Ciudad actualizada con éxito");
        }
        catch (Exception ex)
        {
            return ResponseDto.Error(ex.Message);
        }
    }
}