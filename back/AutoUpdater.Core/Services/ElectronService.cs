using AutoUpdater.Abstractions.Enums;
using AutoUpdater.Abstractions.Helpers;
using AutoUpdater.Abstractions.Interfaces.Repositories;
using AutoUpdater.Abstractions.Interfaces.Services;
using AutoUpdater.Abstractions.Models;
using AutoUpdater.Abstractions.Transports;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;

namespace AutoUpdater.Core.Services;

public class ElectronService : IElectronService
{
	private readonly IAppRepository _appRepository;
	private readonly IBlockmapRepository _blockmapRepository;
	private readonly IAppService _appService;
	private readonly ILogger<ElectronService> _logger;
	private readonly IMemoryCache _memoryCache;


	public ElectronService(IAppService appService, ILogger<ElectronService> logger, IAppRepository appRepository, IMemoryCache memoryCache, IBlockmapRepository blockmapRepository)
	{
		_appService = appService;
		_logger = logger;
		_appRepository = appRepository;
		_memoryCache = memoryCache;
		_blockmapRepository = blockmapRepository;
	}

	public async Task<ElectronBuilderInfo> GetLatestYml(string app, AppArch arch)
	{
		var logger = _logger.Enter($"{Log.F(app)} {Log.F(arch)}");

		var latest = await _appService.GetLatestArchSpecificVersion(app, arch);

		if (_memoryCache.TryGetValue(GetCacheKey(app, arch, latest), out ElectronBuilderInfo? info)) return info!;

		var bytes = await _appService.GetBinary(app, latest, arch);

		var sha = ComputeSha512(bytes);

		var path = $"{latest.Raw}";
		info = new()
		{
			Path = path,
			Sha512 = sha,
			Version = latest.Raw,
			Files = new()
			{
				new()
				{
					Sha512 = sha,
					Size = bytes.Length,
					Url = path
				}
			},
			ReleaseDate = await _appRepository.GetReleaseDate(app, latest, arch)
		};

		_memoryCache.Set(GetCacheKey(app, arch, latest), info, new MemoryCacheEntryOptions
		{
			SlidingExpiration = TimeSpan.FromHours(1),
			Priority = CacheItemPriority.Low
		});


		logger.Exit();

		return info;
	}

	public async Task<byte[]> GetBlockmap(string app, AppArch arch, string version)
	{
		var logger = _logger.Enter($"{Log.F(app)} {Log.F(arch)} {Log.F(version)}");

		var entity = await _blockmapRepository.GetBlockmap(app, arch, version);

		logger.Exit();

		return entity.Content;
	}

	public async Task AddBlockmap(string app, AppArch arch, string version, byte[] content)
	{
		var logger = _logger.Enter($"{Log.F(app)} {Log.F(arch)} {Log.F(version)} {Log.F(content.Length)}");

		await _blockmapRepository.AddBlockmap(app, arch, version, content);

		logger.Exit();
	}

	private static string GetCacheKey(string app, AppArch arch, AppVersion version)
	{
		return $"{app}.{arch}.${version.Raw}";
	}

	private string ComputeSha512(byte[] content)
	{
		using var sha512 = SHA512.Create();
		var hashBytes = sha512.ComputeHash(content);
		return Convert.ToBase64String(hashBytes);
	}
}