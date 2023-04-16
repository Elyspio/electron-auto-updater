using AutoUpdater.Abstractions.Models;

namespace AutoUpdater.Abstractions.Transports;

public class App
{
	public required byte[] Binary { get; set; }

	public required AppMetadata Metadata { get; set; }
}