using AutoMapper;
using Externo.Data.Dtos;
using Externo.Models;

namespace Externo.Profiles
{
    public class EmailProfile : Profile
    {
        public EmailProfile()
        {
            CreateMap<EmailMensagem, ReadEmailDto>();
        }
    }
}
