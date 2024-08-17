using System.Text;
using System.Text.Json;
using PlatformService.DTOs;

namespace PlatformService.SyncDataServices.Http
{
    public class HttpCommandDataClient : ICommandDataClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public HttpCommandDataClient(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task SendPlatformToCommand(PlatformReadDTO plat)
        {
            var httpContent = new StringContent(
                JsonSerializer.Serialize(plat),
                Encoding.UTF8,
                "application/json"
            );

            string apiAddress = $"{_configuration["CommandsService"]}{_configuration["CommandsService_PostPlatform"]}";
            
            var response = await _httpClient.PostAsync(apiAddress, httpContent);

            if(response.IsSuccessStatusCode)
            {
                Console.WriteLine("--> Successful response from sync post to CommandService");
            }
            else
            {
                Console.WriteLine("--> Unsuccessful response from sync post to CommandService");
            }
        }
    }
}