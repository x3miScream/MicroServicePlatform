using AutoMapper;
using CommandsService.Data;
using CommandsService.DTOs;
using CommandsService.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers
{
    [Route("api/commandsService/[controller]")]
    [ApiController]
    public class PlatformController: ControllerBase
    {
        private readonly IPlatformRepo _repository;
        private readonly ICommandsRepo _commandsRepo;
        private readonly IMapper _mapper;

        public PlatformController(ICommandsRepo commandsRepo, IPlatformRepo repository, IMapper mapper)
        {
            _repository = repository;
            _commandsRepo = commandsRepo;
            _mapper = mapper;
        }

        
        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDTO>> GetPlatforms()
        {
            Console.WriteLine("--> Getting Platforms from CommandsService");

            var platforms = _repository.GetAllPlatforms();

            return Ok(_mapper.Map<IEnumerable<PlatformReadDTO>>(platforms));
        }


        [HttpPost]
        public ActionResult<PlatformReadDTO> CreatePlatform(PlatformReadDTO platformReadDTO)
        {
            Console.WriteLine($"--> Synced from PlatformService {System.Text.Json.JsonSerializer.Serialize(platformReadDTO)}");

            Platform platform = _mapper.Map<Platform>(platformReadDTO);

            _repository.CreatePlatform(platform);
            _repository.SaveChanges();

            return Ok(platformReadDTO);
        }
    }
}