using AutoUpdater.Abstractions.Enums;
using AutoUpdater.Abstractions.Transports;

namespace AutoUpdater.Abstractions.Interfaces.Services;

public interface IElectronService
{
	Task<ElectronBuilderInfo> GetLatestYml(string app, AppArch arch);
	Task<byte[]> GetBlockmap(string app, AppArch arch, string version);
	Task AddBlockmap(string app, AppArch arch, string version, byte[] content);
}