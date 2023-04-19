using Electron.Updater.Release.App.Config;
using SwaggerMerge.ReleaseApp.UpdaterClient;

namespace Electron.Updater.Release.App.Services;

public class UpdaterService
{
	private readonly AppsClient _appsApi;
	private readonly ElectronAppsClient _electronApi;
	private readonly Settings _settings;

	public UpdaterService(AppsClient appsApi, Settings settings, ElectronAppsClient electronApi)
	{
		_appsApi = appsApi;
		_settings = settings;
		_electronApi = electronApi;
	}

	public async Task Upload(string path, AppVersion version, AppArch arch)
	{
		try
		{
			var startTime = DateTime.Now;
			var bytes = await File.ReadAllBytesAsync(path);
			Console.WriteLine("Adding " + path);
			await _appsApi.AddFromBytesAsync(_settings.AppName, version.ToString(), arch, bytes);
			Console.WriteLine($"Added {path} in {(DateTime.Now - startTime).Seconds}s");
		}

		catch (Exception e)
		{
			await Console.Error.WriteLineAsync($"Error in upload of {path}, retry in 1s");
			await Console.Error.WriteLineAsync(e.ToString());
			await Task.Delay(1000);
			await Upload(path, version, arch);
		}
	}


	public async Task UploadBlockmap(string path, AppVersion version)
	{
		try
		{
			var startTime = DateTime.Now;
			var bytes = await File.ReadAllBytesAsync(path);
			Console.WriteLine("Adding " + path);
			await _electronApi.AddBlockmapAsync(_settings.AppName, AppArch.Win64, version.ToString(), bytes);
			Console.WriteLine($"Added {path} in {(DateTime.Now - startTime).Seconds}s");
		}

		catch (Exception e)
		{
			await Console.Error.WriteLineAsync($"Error in upload of {path}, retry in 1s");
			await Console.Error.WriteLineAsync(e.ToString());
			await Task.Delay(1000);
			await UploadBlockmap(path, version);
		}
	}


	public async Task<AppVersion> GetLatestVersion()
	{
		return await _appsApi.GetLatestVersionAsync(_settings.AppName);
	}
}