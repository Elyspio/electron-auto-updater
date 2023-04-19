using CommandLine;
using Electron.Updater.Release.App;
using Electron.Updater.Release.App.Config;
using Electron.Updater.Release.App.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SwaggerMerge.ReleaseApp.UpdaterClient;

using var host = Host.CreateDefaultBuilder(args)
	.ConfigureServices(services =>
	{
		var arguments = Parser.Default.ParseArguments<AppArguments>(args).Value;

		services.AddSingleton<Settings>(_ => Settings.GetSettings(arguments.ConfigPath));

		services.AddSingleton<UpdaterService>();
		services.AddSingleton<ReleaseService>();
		services.AddSingleton<DockerService>();
		services.AddSingleton<Paths>();
		
		services.AddHttpClient<ElectronAppsClient>((provider, client) =>
		{
			var settings = provider.GetRequiredService<Settings>();
			client.BaseAddress = new(settings.Server);
		});
		services.AddHttpClient<AppsClient>((provider, client) =>
		{
			var settings = provider.GetRequiredService<Settings>();
			client.BaseAddress = new(settings.Server);
		});
	})
	.Build();

var releaseService = host.Services.GetRequiredService<ReleaseService>();

await releaseService.Run();

namespace Electron.Updater.Release.App
{
	internal class AppArguments
	{
		[Option("minor", Required = false, HelpText = "Change minor version instead of revision number", Default = false)]
		public bool Minor { get; set; }

		[Option('c', "config", Required = false, HelpText = "Path to config", Default = "./settings.json")]
		public required string ConfigPath { get; set; }
	}
}