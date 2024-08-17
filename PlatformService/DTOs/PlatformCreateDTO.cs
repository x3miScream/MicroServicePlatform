namespace PlatformService.DTOs
{
    public class PlatformCreateDTO
    {
        public string Name{get;set;} = default!;
        public string Publisher {get;set;} = default!;
        public string Cost {get;set;} = default!;
    }
}