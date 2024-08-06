using AutoMapper;
using TurismoApp.Common.DTO;
using TurismoApp.Common.DTO.CiudadDtos;
using TurismoApp.Common.DTO.ClientesDtos;
using TurismoApp.Common.DTO.DepartamentoDtos;
using TurismoApp.Common.DTO.RecorridoDtos;
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

        CreateMap<Ciudad, CiudadDto>()
            .ForMember(dest => dest.DepartamentoDescripcion, opt => opt.MapFrom(src => src.Departamento.Descripcion));

        CreateMap<Cliente, ClienteDto>();
        CreateMap<ClienteDto, Cliente>();
        CreateMap<CreateClienteDto, Cliente>();
        CreateMap<UpdateClienteDto, Cliente>();
        CreateMap<Cliente, ClienteResponseDto>();
        CreateMap<Cliente, VerifyClienteDto>();
        CreateMap<ClienteDto, VerifyClienteDto>();
        CreateMap<ClienteDto, UpdateClienteDto>();
        CreateMap<ClienteDto, ClienteResponseDto>();
        CreateMap<VerifyClienteDto, Cliente>();
        CreateMap<ClienteResponseDto, ClienteDto>();
        CreateMap<ClienteResponseDto, VerifyClienteDto>();

        CreateMap<Recorrido, RecorridoDto>()
            .ForMember(dest => dest.CiudadOrigen, opt => opt.MapFrom(src => src.CiudadOrigen.Descripcion))
            .ForMember(dest => dest.IdCiudadOrigen, opt => opt.MapFrom(src => src.CiudadOrigenId)) 
            .ForMember(dest => dest.CiudadDestino, opt => opt.MapFrom(src => src.CiudadDestino.Descripcion))
            .ForMember(dest => dest.IdCiudadDestino, opt => opt.MapFrom(src => src.CiudadDestinoId)) 
            .ForMember(dest => dest.Pasajeros, opt => opt.MapFrom(src => src.ClienteRecorridos.Select(cr => cr.Cliente)));

        CreateMap<CreateRecorridoDto, Recorrido>()
            .ForMember(dest => dest.ClienteRecorridos, opt => opt.Ignore());

        CreateMap<UpdateRecorridoDto, Recorrido>()
            .ForMember(dest => dest.ClienteRecorridos, opt => opt.Ignore());

        CreateMap<RecorridoDto, UpdateRecorridoDto>();
        CreateMap<Cliente, PasajeroResponseDto>();
    }
}