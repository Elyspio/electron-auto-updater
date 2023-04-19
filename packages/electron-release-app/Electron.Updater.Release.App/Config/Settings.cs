using Newtonsoft.Json;

namespace Electron.Updater.Release.App.Config;

public class Settings
{
	public required string AppName { get; set; }
	public required string Server { get; set; }
	public required SettingsPaths Paths { get; set; }


	public static Settings GetSettings(string path)
	{
		path = Path.GetFullPath(path, Directory.GetCurrentDirectory());

		var json = File.ReadAllText(path);
		var settings = JsonConvert.DeserializeObject<Settings>(json)!;

		settings.Paths.Releases = Path.GetFullPath(settings.Paths.Releases, path);
		settings.Paths.Root = Path.GetFullPath(settings.Paths.Root, path);

		return settings;
	}

	public class SettingsPaths
	{
		public required string Root { get; set; }
		public required string Releases { get; set; }
	}
}