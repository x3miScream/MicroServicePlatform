using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.AsyncDataServices;
using PlatformService.Data;
using PlatformService.DTOs;
using PlatformService.Models;
using PlatformService.SyncDataServices.Http;

namespace PlatformService.Controllers
{
    [Route("api/platformService/[controller]")]
    [ApiController]
    public class PlatformController: ControllerBase
    {
        private readonly IPlatformRepo _repository;
        private readonly IMapper _mapper;
        private readonly ICommandDataClient _commandDataClient;
        private readonly IMessageBusClient _messageBusClient;

        public PlatformController(
            IPlatformRepo repository,
            IMapper mapper,
            ICommandDataClient commandDataClient,
            IMessageBusClient messageBusClient)
        {
            _repository = repository;
            _mapper = mapper;
            _commandDataClient = commandDataClient;
            _messageBusClient = messageBusClient;
        }

        [HttpGet]
        public ActionResult<IEnumerable<List<PlatformReadDTO>>> Get()
        {
            return Ok(
                _mapper.Map<IEnumerable<PlatformReadDTO>>(_repository.GetAllPlatforms())
            );
        }

        [HttpGet("{id}", Name = "GetPlatformById")]
        public ActionResult<PlatformReadDTO> GetPlatformById(int id)
        {
            var platformItem = _repository.GetPlatformById(id);

            if(platformItem != null)
            {
                return Ok(_mapper.Map<PlatformReadDTO>(platformItem));
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<ActionResult<PlatformReadDTO>> CreatePlatform(PlatformCreateDTO platformCreateDto)
        {
            if(platformCreateDto != null)
            {
                Platform platformModel = _mapper.Map<Platform>(platformCreateDto);
                
                _repository.CreatePlatform(platformModel);
                _repository.SaveChanges();

                var platformReadDto = _mapper.Map<PlatformReadDTO>(platformModel);

                // try
                // {
                //     await _commandDataClient.SendPlatformToCommand(platformReadDto);
                //     Console.WriteLine("--> Sending platformReadDTO Sync");
                // }
                // catch(Exception ex)
                // {
                //     Console.WriteLine($"--> Could not post platform to Commands Service: {ex.Message}");
                // }



                try
                {
                    var platformPublisedDTO = _mapper.Map<PlatformPublishedDTO>(platformReadDto);
                    platformPublisedDTO.Event = "Platform_Published";
                    Console.WriteLine("--> Sending platformReadDTO Async");
                    _messageBusClient.PublishNewPlatform(platformPublisedDTO);
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"--> Could not send platformReadDTO to Message Bus: {ex.Message}");
                }



                return CreatedAtRoute(nameof(GetPlatformById), new {id = platformReadDto.Id}, platformReadDto);
            }
            else
            {
                return NotFound();
            }
        }
    }
}