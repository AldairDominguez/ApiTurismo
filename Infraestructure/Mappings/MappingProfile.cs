using AutoMapper;
using Common.DTO;
using Infraestructure.Entities;

namespace Infraestructure.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //CreateMap<Ciudad, CiudadDto>();
            //CreateMap<CiudadDto, Ciudad>();

            CreateMap<Departamento, DepartamentoDto>();
            CreateMap<DepartamentoDto, Departamento>();
        }
    }
}
