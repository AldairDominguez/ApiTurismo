using Microsoft.EntityFrameworkCore;
using TurismoApp.Application.Interfaces;
using TurismoApp.Common;
using TurismoApp.Common.DTO;
using TurismoApp.Common.DTO.DepartamentoDtos;
using TurismoApp.Infraestructure.Repositories.Interfaces;

namespace TurismoApp.Application.Implementations;

public class DepartamentoApplication : IDepartamentoApplication
{
    private readonly IDepartamentoRepository _departamentoRepository;

    public DepartamentoApplication(IDepartamentoRepository departamentoRepository)
    {
        _departamentoRepository = departamentoRepository;
    }

    public async Task<ResponseDto> AddDepartamentoAsync(UpdateDepartamentoDto departamento)
    {
        try
        {
            if (await _departamentoRepository.ExistsDepartamentoAsync(departamento.Descripcion))
            {
                return ResponseDto.Error("El departamento ya existe.");
            }
            var addedDepartamento = await _departamentoRepository.AddDepartamentoAsync(departamento);
            return ResponseDto.Ok(addedDepartamento);
        }
        catch (Exception ex)
        {
            return ResponseDto.Error(ex.Message);
        }
    }

    public async Task<ResponseDto> DeleteDepartamentoAsync(int id)
    {
        try
        {
            var departamento = await _departamentoRepository.GetDepartamentoByIdAsync(id);
            if (departamento == null)
            {
                return ResponseDto.Error("Departamento no encontrado");
            }

            await _departamentoRepository.DeleteDepartamentoAsync(id);
            return ResponseDto.Ok(null, "Se elimino con éxito");
        }
        catch (Exception ex)
        {
            return ResponseDto.Error(ex.Message);
        }
    }

    public async Task<IEnumerable<DepartamentoDto>> GetAllDepartamentosAsync()
    {
        return await _departamentoRepository.GetAllDepartamentosAsync();
    }

    public async Task<DepartamentoDto> GetDepartamentoByIdAsync(int id)
    {
        return await _departamentoRepository.GetDepartamentoByIdAsync(id);
    }

    public async Task<ResponseDto> UpdateDepartamentoAsync(int id, UpdateDepartamentoDto departamento)
    {
        try
        {
            var existingDepartamento = await _departamentoRepository.GetDepartamentoByIdAsync(id);
            if (existingDepartamento == null)
            {
                return ResponseDto.Error("Departamento no encontrado");
            }

            if (await _departamentoRepository.ExistsDepartamentoAsync(departamento.Descripcion, id))
            {
                return ResponseDto.Error("El departamento ya existe.");
            }

            await _departamentoRepository.UpdateDepartamentoAsync(id, departamento);
            return ResponseDto.Ok(null, "Departamento actualizado con éxito");
        }
        catch (Exception ex)
        {
            return ResponseDto.Error(ex.Message);
        }
    }
   
}
