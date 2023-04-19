using AutoUpdater.Abstractions.Enums;

namespace AutoUpdater.Abstractions.Models;

public class AppMetadata
{
	public required string Name { get; set; }
	public required AppVersion Version { get; set; }
	public required AppArch Arch { get; set; }
}