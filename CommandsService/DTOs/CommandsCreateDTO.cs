using System.ComponentModel.DataAnnotations;

namespace CommandsService.DTOs
{
    public class CommandsCreateDTO
    {
        [Required]
        public string HowTo {get;set;} = default!;
        [Required]
        public string CommandLine { get; set; } = default!;
    }
}