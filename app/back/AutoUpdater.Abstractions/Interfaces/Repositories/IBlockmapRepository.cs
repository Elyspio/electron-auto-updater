using AutoUpdater.Abstractions.Enums;
using AutoUpdater.Abstractions.Models;
using MongoDB.Bson;

namespace AutoUpdater.Abstractions.Interfaces.Repositories;

public interface IBlockmapRepository
{
	Task<BlockmapEntity> GetBlockmap(string app, AppArch arch, string version);
	Task AddBlockmap(string app, AppArch arch, AppVersion version, byte[] content);
}