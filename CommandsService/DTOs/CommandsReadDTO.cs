namespace CommandsService.DTOs
{
    public class CommandsReadDTO
    {
        public int Id {get;set;}
        public int PlatformId {get;set;}
        public string HowTo {get;set;} = default!;
        public string CommandLine { get; set; } = default!;
    }
}