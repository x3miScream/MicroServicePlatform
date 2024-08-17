namespace CommandsService.DTOs
{
    public class PlatformCreateDTO
    {
        public PlatformCreateDTO()
        {
            if(Name == null)
                Name = string.Empty;
        }

        public string Name{get;set;}
    }
}