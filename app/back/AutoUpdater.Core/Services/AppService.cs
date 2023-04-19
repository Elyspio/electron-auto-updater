using AutoUpdater.Abstractions.Enums;
using AutoUpdater.Abstractions.Helpers;
using AutoUpdater.Abstractions.Interfaces.Repositories;
using AutoUpdater.Abstractions.Interfaces.Services;
using AutoUpdater.Abstractions.Models;
using AutoUpdater.Abstractions.Transports;
using Microsoft.Extensions.Logging;

namespace AutoUpdater.Core.Services;

public class AppService : IAppService
{
	private readonly ILogger<AppService> _logger;
	private readonly IAppRepository _repository;

	public AppService(IAppRepository repository, ILogger<AppService> logger)
	{
		_repository = repository;
		_logger = logger;
	}

	public async Task Add(App app)
	{
		var logger = _logger.Enter($"{Log.F(app.Metadata)}");

		var metadatas = await _repository.GetAllMetadata(app.Metadata.Name);

		if (metadatas.Any(metadata => metadata.Arch == app.Metadata.Arch && metadata.Version.Raw == app.Metadata.Version.Raw))
			await _repository.Delete(app.Metadata.Name, app.Metadata.Version, app.Metadata.Arch);

		await _repository.Add(app);

		logger.Exit();
	}

	public async Task Delete(string name, AppVersion version, AppArch arch)
	{
		var logger = _logger.Enter($"{Log.F(name)} {Log.F(version)} {Log.F(arch)}");

		await _repository.Delete(name, version, arch);

		logger.Exit();
	}

	public async Task<byte[]> GetBinary(string name, AppVersion version, AppArch arch)
	{
		var logger = _logger.Enter($"{Log.F(name)} {Log.F(version)} {Log.F(arch)}");

		var data = await _repository.GetBinary(name, version, arch);

		logger.Exit();

		return data;
	}

	public Task<Dictionary<AppArch, AppVersion>> GetLatestArchSpecificVersion(string name)
	{
		var logger = _logger.Enter($"{Log.F(name)}");

		var version = _repository.GetLatestVersions(name);

		logger.Exit();

		return version;
	}

	public async Task<List<AppMetadata>> GetAllMetadata(string name)
	{
		var logger = _logger.Enter($"{Log.F(name)}");

		var metadatas = await _repository.GetAllMetadata(name);

		logger.Exit();

		return metadatas;
	}

	public async Task<AppVersion> GetLatestArchSpecificVersion(string name, AppArch arch)
	{
		var logger = _logger.Enter($"{Log.F(name)} {Log.F(arch)}");

		var versions = await _repository.GetLatestVersions(name, arch);

		logger.Exit();

		return versions;
	}

	public async Task<string[]> GetApps()
	{
		var logger = _logger.Enter();

		var apps = await _repository.GetApps();

		logger.Exit();

		return apps;
	}

	public async Task<AppVersion> GetLatestVersion(string name)
	{
		var logger = _logger.Enter($"{Log.F(name)}");

		var version = await _repository.GetLatestVersion(name);

		logger.Exit();

		return version;
	}
}