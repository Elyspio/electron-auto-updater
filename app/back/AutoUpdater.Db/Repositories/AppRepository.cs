using AutoUpdater.Abstractions.Enums;
using AutoUpdater.Abstractions.Exceptions;
using AutoUpdater.Abstractions.Helpers;
using AutoUpdater.Abstractions.Interfaces.Repositories;
using AutoUpdater.Abstractions.Models;
using AutoUpdater.Abstractions.Transports;
using AutoUpdater.Db.Repositories.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using MongoDB.Driver.Linq;
using System.Net;

namespace AutoUpdater.Db.Repositories;

internal class AppRepository : BaseRepository<AppEntity>, IAppRepository
{
	private readonly GridFSBucket _gridFsBucket;
	private readonly ILogger<AppRepository> _baseLogger;

	public AppRepository(IConfiguration configuration, ILogger<AppRepository> baseLogger) : base(configuration, baseLogger)
	{
		_gridFsBucket = new(Context.MongoDatabase, new()
		{
			BucketName = CollectionName
		});
		_baseLogger = baseLogger;
	}


	public async Task Delete(string name, AppVersion version, AppArch arch)
	{
		var logger = _baseLogger.Enter($"{Log.F(name)} {Log.F(arch)} {Log.F(version)}");

		var app = await Get(name, version, arch);
		await _gridFsBucket.DeleteAsync(app.IdGridFs);
		await EntityCollection.FindOneAndDeleteAsync(f => f.Id == app.Id);

		logger.Exit();
	}

	public async Task Add(App app)
	{
		var logger = _baseLogger.Enter(Log.F(app.Metadata));

		var file = new AppEntity
		{
			Metadata = app.Metadata
		};

		await EntityCollection.InsertOneAsync(file);

		var idGridFs = await _gridFsBucket.UploadFromBytesAsync(file.Id.ToString(), app.Binary);

		file.IdGridFs = idGridFs;

		await EntityCollection.ReplaceOneAsync(f => f.Id == file.Id, file);

		logger.Exit();
	}

	public async Task<List<AppMetadata>> GetAllMetadata(string name)
	{
		var logger = _baseLogger.Enter(Log.F(name));

		var metadatas = await EntityCollection
			.AsQueryable()
			.Where(app => app.Metadata.Name == name)
			.Select(app => app.Metadata)
			.ToListAsync();

		logger.Exit();

		return metadatas;
	}

	public async Task<byte[]> GetBinary(string name, AppVersion version, AppArch arch)
	{
		var logger = _baseLogger.Enter($"{Log.F(name)} {Log.F(arch)} {Log.F(version)}");

		var app = await Get(name, version, arch);

		if (app == default) throw new HttpException(HttpStatusCode.NotFound, $"Could not find app: {Log.F(name)} {Log.F(arch)}  {Log.F(version)}");

		var bytes = await _gridFsBucket.DownloadAsBytesByNameAsync(app.Id.ToString());

		logger.Exit();

		return bytes;
	}

	public async Task<Dictionary<AppArch, AppVersion>> GetLatestVersions(string name)
	{
		var logger = _baseLogger.Enter(Log.F(name));

		var apps = await EntityCollection
			.AsQueryable()
			.Where(app => app.Metadata.Name == name)
			.ToListAsync();

		var dict = new Dictionary<AppArch, AppVersion>();

		foreach (AppArch arch in Enum.GetValues(typeof(AppArch)))
		{
			var archApps = apps.Where(app => app.Metadata.Arch == arch).ToList();
			if (archApps.Any())
				dict[arch] = archApps.Max()!.Metadata.Version;
		}

		logger.Exit();

		return dict;
	}

	public async Task<AppVersion> GetLatestVersions(string name, AppArch arch)
	{
		var logger = _baseLogger.Enter($"{Log.F(name)} {Log.F(arch)}");

		var versions = await GetLatestVersions(name);
		var version = versions[arch];

		logger.Exit();

		return version;
	}

	public async Task<string[]> GetApps()
	{
		var logger = _baseLogger.Enter();

		var apps = (await EntityCollection.AsQueryable().Select(app => app.Metadata.Name).ToListAsync()).ToHashSet();

		logger.Exit();

		return apps.ToArray();
	}

	public async Task<AppVersion> GetLatestVersion(string name)
	{
		var logger = _baseLogger.Enter(Log.F(name));

		var versions = await GetLatestVersions(name);

		var latest = versions.Values.Max() ?? throw new HttpException(HttpStatusCode.NotFound, $"Could not find app {Log.F(name)}");

		logger.Exit();

		return latest;
	}

	public async Task<DateTime> GetReleaseDate(string name, AppVersion version, AppArch arch)
	{
		var logger = _baseLogger.Enter($"{Log.F(name)}");

		var entity = await EntityCollection.AsQueryable().SingleOrDefaultAsync(app => app.Metadata.Name == name && app.Metadata.Version == version && app.Metadata.Arch == arch);

		logger.Exit();

		return entity.Id.CreationTime;
	}


	private Task<AppEntity> Get(string name, AppVersion version, AppArch arch)
	{
		var logger = _baseLogger.Enter($"{Log.F(name)} {Log.F(arch)} {Log.F(version)}");

		var app = EntityCollection
			.AsQueryable()
			.Where(app => app.Metadata.Name == name && app.Metadata.Version == version && app.Metadata.Arch == arch)
			.FirstOrDefaultAsync();

		logger.Exit();

		return app;
	}
}