using Abstractions.Enums;
using System.ComponentModel.DataAnnotations;

namespace Abstractions.Models
{
    public class AppMetadata
    {
        [Required]
        public string Name { get; set; }
        
        [Required]
        public AppVersion Version { get; set; }
        
        [Required]
        public AppArch Arch { get; set; }
    }
}
