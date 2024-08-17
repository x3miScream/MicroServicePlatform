using System.ComponentModel.DataAnnotations;

namespace PlatformService.Models
{
    public class Platform
    {
        public Platform()
        {
            if(Name == null) Name = string.Empty;
            if(Publisher == null) Publisher = string.Empty;
            if(Cost == null) Cost = string.Empty;
        }

        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Publisher { get; set; }
        [Required]
        public string Cost { get; set; }
    }
}