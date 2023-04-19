using AutoUpdater.Abstractions.Enums;
using AutoUpdater.Abstractions.Helpers;
using AutoUpdater.Abstractions.Interfaces.Repositories;
using AutoUpdater.Abstractions.Models;
using AutoUpdater.Db.Repositories.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace AutoUpdater.Db.Repositories;

class BlockmapRepository : BaseRepository<BlockmapEntity>, IBlockmapRepository
{
	private readonly ILogger<BaseRepository<BlockmapEntity>> _logger;

	public BlockmapRepository(IConfiguration configuration, ILogger<BaseRepository<BlockmapEntity>> baseLogger, ILogger<BaseRepository<BlockmapEntity>> logger) : base(configuration, baseLogger)
	{
		_logger = logger;
	}


	public async Task<BlockmapEntity> GetBlockmap(string app, AppArch arch, string version)
	{
		var logger = _logger.Enter($"{Log.F(app)} {Log.F(arch)} {Log.F(version)}");

		var entity = await EntityCollection.AsQueryable().FirstOrDefaultAsync(blockmap => blockmap.App == app && blockmap.Version.Raw == version && blockmap.Arch == arch);

		logger.Exit();

		return entity;
	}

	public async Task AddBlockmap(string app, AppArch arch, AppVersion version, byte[] content)
	{
		var logger = _logger.Enter($"{Log.F(app)} {Log.F(arch)} {Log.F(version)} {Log.F(content.Length)}");

		await EntityCollection.InsertOneAsync(new()
		{
			App = app,
			Arch = arch,
			Content = content,
			Version = version,
		});

		logger.Exit();
	}
}