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

        CreateMap<Cliente, ClienteDto>();
        CreateMap<ClienteDto, Cliente>();
        CreateMap<CreateClienteDto, Cliente>();
        CreateMap<UpdateClienteDto, Cliente>();

        CreateMap<ClienteDto, UpdateClienteDto>();

        CreateMap<Recorrido, RecorridoDto>()
            .ForMember(dest => dest.CiudadOrigen, opt => opt.MapFrom(src => src.CiudadOrigen.Descripcion))
            .ForMember(dest => dest.CiudadDestino, opt => opt.MapFrom(src => src.CiudadDestino.Descripcion))
            .ForMember(dest => dest.Pasajeros, opt => opt.MapFrom(src => src.ClienteRecorridos.Select(cr => cr.Cliente)));

        CreateMap<CreateRecorridoDto, Recorrido>()
            .ForMember(dest => dest.ClienteRecorridos, opt => opt.Ignore());

        CreateMap<UpdateRecorridoDto, Recorrido>()
            .ForMember(dest => dest.ClienteRecorridos, opt => opt.Ignore());

        CreateMap<RecorridoDto, UpdateRecorridoDto>();
        CreateMap<Cliente, PasajeroResponseDto>();
    }
}