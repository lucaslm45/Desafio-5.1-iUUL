using AutoMapper;
using Externo.Data.Dtos;
using Externo.Models;

namespace Externo.Profiles
{
    public class CobrancaProfile : Profile
    {
        public CobrancaProfile()
        {
            CreateMap<CreateCobrancaDto, Cobranca>();
            CreateMap<CreateCobrancaDto, ReadCobrancaDto>();
            CreateMap<Cobranca, ReadCobrancaDto>();
        }
    }
}
