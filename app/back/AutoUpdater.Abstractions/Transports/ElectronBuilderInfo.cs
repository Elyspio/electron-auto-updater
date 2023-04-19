using Newtonsoft.Json;

namespace AutoUpdater.Abstractions.Transports;

public class ElectronBuilderInfo
{
	[JsonProperty("version")]
	public required string Version { get; set; }

	[JsonProperty("files")]
	public required List<File> Files { get; set; }

	[JsonProperty("path")]
	public required string Path { get; set; }

	[JsonProperty("sha512")]
	public required string Sha512 { get; set; }

	[JsonProperty("releaseDate")]
	public required DateTime ReleaseDate { get; set; }
}

public class File
{
	[JsonProperty("url")]
	public required string Url { get; set; }

	[JsonProperty("sha512")]
	public required string Sha512 { get; set; }

	[JsonProperty("size")]
	public required long Size { get; set; }
}