namespace CommandsService.DTOs
{
    public class PlatformReadDTO
    {
        public PlatformReadDTO()
        {
            if(Name == null)
                Name = string.Empty;
        }

        public int Id {get;set;}
        public string Name{get;set;}
    }
}