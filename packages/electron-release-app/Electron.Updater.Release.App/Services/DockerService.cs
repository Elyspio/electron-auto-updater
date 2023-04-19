using System.Diagnostics;
using Electron.Updater.Release.App.Config;
using SwaggerMerge.ReleaseApp.UpdaterClient;

namespace Electron.Updater.Release.App.Services;

public class DockerService
{
	private readonly Settings _settings;
	private readonly  Paths _paths;

	public DockerService(Settings settings, Paths paths)
	{
		_settings = settings;
		_paths = paths;
	}

	private void SetupProcess(Process process)
	{
		process.StartInfo.RedirectStandardOutput = true;
		process.StartInfo.RedirectStandardError = true;
		process.StartInfo.RedirectStandardInput = true;
		process.StartInfo.UseShellExecute = false;
		process.StartInfo.CreateNoWindow = true;
		process.StartInfo.Environment["APP"] = _settings.AppName.Replace(".", "-"); 
		process.ErrorDataReceived += (_, a) => Console.WriteLine(a.Data);
		process.OutputDataReceived += (_, a) => Console.WriteLine(a.Data);
		process.EnableRaisingEvents = true;
		process.Start();
		process.BeginOutputReadLine();
		process.BeginErrorReadLine();
	}

	public void Run(string path)
	{
		var process = new Process();
		process.StartInfo.WorkingDirectory = path;
		process.StartInfo.FileName = "docker-compose";
		process.StartInfo.Arguments = "up";
		process.StartInfo.Environment["RELEASE_PATH"] = Path.GetRelativePath(path, _paths.ReleasesDir); 

		SetupProcess(process);

		process.StandardInput.WriteLine("exit");

		process.WaitForExit();
	}

	public void Build(string path, AppVersion appVersion)
	{
		var process = new Process();
		process.StartInfo.WorkingDirectory = path;
		process.StartInfo.FileName = "docker-compose";
		process.StartInfo.Arguments = $"build --build-arg APP_VERSION={appVersion.Raw}";

		SetupProcess(process);

		process.StandardInput.WriteLine("exit");

		process.WaitForExit();
	}

	public void Stop(string path)
	{
		var process = new Process();
		process.StartInfo.WorkingDirectory = path;
		process.StartInfo.FileName = "docker-compose";
		process.StartInfo.Arguments = "down";

		SetupProcess(process);

		process.StandardInput.WriteLine("exit");

		process.WaitForExit();
	}
}