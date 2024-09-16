using AutoMapper;
using EventHandler.Dto;
using EventHandler.Models.Entities;

namespace RestApiwithDB.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<RegisterDto, AppUser>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email));
        }
    }
}