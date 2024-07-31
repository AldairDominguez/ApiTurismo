using AutoMapper;
using TurismoApp.Common.DTO;
using TurismoApp.Infraestructure.Entities;

namespace TurismoApp.Infraestructure.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Departamento, DepartamentoDto>();
        CreateMap<DepartamentoDto, Departamento>();
        CreateMap<UpdateDepartamentoDto, Departamento>();

        // Configuración de mapeo para Ciudad
        CreateMap<Ciudad, CiudadDto>();
        CreateMap<CiudadDto, Ciudad>();
        CreateMap<CreateCiudadDto, Ciudad>();
        CreateMap<UpdateCiudadDto, Ciudad>();
    }
}