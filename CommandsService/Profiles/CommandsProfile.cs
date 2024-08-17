using AutoMapper;
using CommandsService.DTOs;
using CommandsService.Models;

namespace CommandsService.Profiles
{
    public class CommandsProfile: Profile
    {
        public CommandsProfile()
        {
            // Source -> Target

            CreateMap<Platform, PlatformReadDTO>();
            CreateMap<CommandsCreateDTO, Commands>();
            CreateMap<Commands, CommandsReadDTO>();
        }
    }
}