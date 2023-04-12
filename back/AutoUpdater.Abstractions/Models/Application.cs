using System.ComponentModel.DataAnnotations;

namespace AutoUpdater.Abstractions.Models
{
    public class App
    {
        public required byte[] Binary { get; set; }

        public required AppMetadata Metadata { get; set; }

    }
}
