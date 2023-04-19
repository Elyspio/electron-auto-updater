using System.Runtime.CompilerServices;

namespace Electron.Updater.Release.App.Config;

public class Paths
{
	public required string RootDir { get; set; }
	public required string DockerDir { get; set; }
	public required string ReleasesDir { get; set; }
	public required string ScriptDir { get; set; }


	public Paths(Settings settings)
	{
		ScriptDir = Path.GetDirectoryName(GetCurrentFileName())!;
		DockerDir = Path.GetFullPath(Path.Combine(ScriptDir, "..", "docker"));
		RootDir = settings.Paths.Root;
		ReleasesDir = settings.Paths.Releases;
	}


	private static string GetCurrentFileName([CallerFilePath] string fileName = null)
	{
		return fileName;
	}
}