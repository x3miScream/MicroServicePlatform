using AutoMapper;
using CommandsService.DTOs;
using CommandsService.Models;

namespace CommandsService.Profiles
{
    public class PlatformProfile: Profile
    {
        public PlatformProfile()
        {
            CreateMap<Platform, PlatformReadDTO>();
            CreateMap<Platform, PlatformCreateDTO>();
            CreateMap<PlatformCreateDTO, Platform>();
            CreateMap<PlatformPublishedDTO, Platform>()
                .ForMember(dest =>  dest.ExternalId, opt => opt.MapFrom(src => src.Id));
        }
    }
}