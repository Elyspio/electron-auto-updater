using CommandLine;
using Electron.Updater.Release.App.Config;
using SwaggerMerge.ReleaseApp.UpdaterClient;

namespace Electron.Updater.Release.App.Services;

public class ReleaseService
{
    private readonly DockerService _dockerService;
    private readonly Settings _settings;
    private readonly UpdaterService _updaterService;
    private readonly Paths _paths;

    public ReleaseService(DockerService dockerService, Settings settings, UpdaterService updaterService, Paths paths)
    {
        _dockerService = dockerService;
        _settings = settings;
        _updaterService = updaterService;
        _paths = paths;
    }

    public async Task Run()
    {
        var arguments = Parser.Default.ParseArguments<AppArguments>(Environment.GetCommandLineArgs()).Value;

        Console.WriteLine($"Releasing {_settings.AppName}");

        Console.WriteLine($"dockerDir {_paths.DockerDir}");

        if (Directory.Exists(_paths.ReleasesDir)) Directory.Delete(_paths.ReleasesDir, true);

        var version = await _updaterService.GetLatestVersion();

        Console.WriteLine($"server version {version}");

        if (arguments.Minor) version.Minor += 1;
        else version.Revision += 1;

        Console.WriteLine($"next version {version}");

        _dockerService.Stop(_paths.DockerDir);
        _dockerService.Build(_paths.DockerDir, version);
        _dockerService.Run(_paths.DockerDir);

        await UploadInstallers(version);
    }

    private readonly Dictionary<AppArch, string> _extensionsByArch = new()
    {
        { AppArch.AppImage, "AppImage" },
        { AppArch.Flatpak, "flatpak" },
        { AppArch.LinuxDeb, "deb" },
        { AppArch.LinuxRpm, "rpm" },
        { AppArch.LinuxSnap, "snap" },
        { AppArch.Win64, "exe" }
    };

    private async Task UploadInstallers(AppVersion newVersion)
    {
        var tasks = new List<Task>();

        var files = Directory.GetFiles(_paths.ReleasesDir).Select(f => Path.Combine(_paths.ReleasesDir, f)).ToList();

        var winBlockmap = files.Find(f => f.EndsWith(".blockmap") && f.Contains(newVersion.Raw));
        if (winBlockmap != null) tasks.Add(_updaterService.UploadBlockmap(winBlockmap, newVersion));

        foreach (var (arch, ext) in _extensionsByArch)
        {
            var installerPath = files.FirstOrDefault(f => f.EndsWith(ext) && f.Contains(newVersion.Raw));
            if (installerPath != default)
            {
                tasks.Add(_updaterService.Upload(installerPath, newVersion, arch));
            }
        }

        await Task.WhenAll(tasks.ToArray());
    }
}