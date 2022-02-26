using System.ComponentModel.DataAnnotations;

namespace Abstractions.Models
{
    public class App
    {
        [Required]
        public byte[] Binary { get; set; }

        [Required]
        public AppMetadata Metadata { get; set; }

    }
}
