using System.ComponentModel.DataAnnotations;

namespace CommandsService.Models
{
    public class Commands
    {
        public Commands()
        {

        }

        [Key]
        [Required]
        public int Id {get;set;}

        [Required]
        public int PlatformId {get;set;}
        public string HowTo {get;set;} = default!;
        public string CommandLine { get; set; } = default!;

        public Platform Plaatform {get;set;}
    }
}