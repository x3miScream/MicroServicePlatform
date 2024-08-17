using System.Text.Json;
using AutoMapper;
using CommandsService.Data;
using CommandsService.DTOs;
using CommandsService.Models;
using Microsoft.Extensions.DependencyInjection;

namespace CommandsService.EventProcessing
{
    public class EventProcessor : IEventProcessor
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IMapper _mapper;


        public EventProcessor(IServiceScopeFactory scopeFactory, IMapper mapper)
        {
            _scopeFactory = scopeFactory;
            _mapper = mapper;
        }

        public void ProcessEvent(string message)
        {
            var eventType = DetermineEvent(message);

            switch(eventType)
            {
                case EventType.PlatformPublished:
                AddPlatform(message);
                    break;

                default:
                    break;
            }
        }


        private EventType DetermineEvent(string notificationMessage)
        {
            Console.WriteLine("--> Determining Event");

            var eventType = JsonSerializer.Deserialize<GenericEventDTO>(notificationMessage);

            if(eventType != null)
            {
                switch(eventType.Event)
                {
                    case "Platform_Published":
                        Console.WriteLine("--> Platform Pushlished Event Detected");
                        return EventType.PlatformPublished;

                    default:
                        Console.WriteLine("--> Failed to determine the event type");
                        return EventType.Undetermined;
                }
            }
            else
                return EventType.Undetermined;
        }

        private void AddPlatform(string platformPublishedMessage)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<IPlatformRepo>();

                var platformPublishedDto = JsonSerializer.Deserialize<PlatformPublishedDTO>(platformPublishedMessage);

                try
                {
                    var platform = _mapper.Map<Platform>(platformPublishedDto);

                    if(!repo.IsPlatformExistsByExternalId(platform.ExternalId))
                    {
                        repo.CreatePlatform(platform);
                        repo.SaveChanges();
                        
                        Console.WriteLine("--> Platform Added");
                    }
                    else
                    {
                        Console.WriteLine($"--> Platform already exists");
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"--> Could not add platform to DB: {ex.Message}");
                }
            }
        }
    }


    enum EventType
    {
        PlatformPublished,
        Undetermined
    }
}