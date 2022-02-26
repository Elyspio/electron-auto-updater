using Abstractions.Models;
using System.ComponentModel.DataAnnotations;

namespace Web.Models
{
    public class AddApp
    {
        [Required]
        public int[] Binary { get; set; }

        [Required]
        public AppMetadata Metadata { get; set; }
    }
}
