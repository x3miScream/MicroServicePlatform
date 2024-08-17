using AutoMapper;
using CommandsService.Data;
using Microsoft.AspNetCore.Mvc;
using CommandsService.DTOs;
using CommandsService.Models;

namespace CommandsService.Controllers
{
    [Route("api/commandsService/platforms/{platformsId}/[controller]")]
    [ApiController]
    public class CommandsController : ControllerBase
    {
        private readonly IPlatformRepo _platformRepo;
        private readonly ICommandsRepo _repository;
        private readonly IMapper _mapper;
        public CommandsController(IPlatformRepo platformRepo, ICommandsRepo repository, IMapper mapper)
        {
            _repository = repository;
            _platformRepo = platformRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CommandsReadDTO>> GetCommandsForPlatform(int platformsId)
        {
            Console.WriteLine($"--> Getting Commands for platform {platformsId}");

            if(!_platformRepo.IsPlatformExists(platformsId))
            {
                return NotFound();
            }

            var commands = _repository.GetAllCommandsByPlatformId(platformsId);
            return Ok(_mapper.Map<IEnumerable<CommandsReadDTO>>(commands));
        }

        [HttpGet("{commandsId}", Name = "GetCommandsForPlatform")]
        public ActionResult<CommandsReadDTO> GetCommandsForPlatform(int platformsId, int  commandsId)
        {
            Console.WriteLine($"--> Getting Commands for platform {platformsId} specific command {commandsId}");

            if(!_platformRepo.IsPlatformExists(platformsId))
            {
                return NotFound();
            }

            var commands = _repository.GetCommandById(platformsId, commandsId);
            return Ok(_mapper.Map<CommandsReadDTO>(commands));
        }


        [HttpPost]
        public ActionResult<CommandsReadDTO> CreateCommandForPlatform(int platformsId, CommandsCreateDTO commandReadDTO)
        {
            Console.WriteLine($"--> Creating a command for platform{platformsId}");

            if(!_platformRepo.IsPlatformExists(platformsId))
            {
                return NotFound();
            }

            var command = _mapper.Map<Commands>(commandReadDTO);

            _repository.CreateCommand(platformsId, command);
            _repository.SaveChanges();

            var commandsReadDTO = _mapper.Map<CommandsReadDTO>(command);

            return CreatedAtAction(nameof(GetCommandsForPlatform), new {platformsId = commandsReadDTO.PlatformId, commandsId = commandsReadDTO.Id}, commandsReadDTO);
        }
    }
}