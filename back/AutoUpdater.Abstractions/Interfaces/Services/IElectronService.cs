using AutoUpdater.Abstractions.Enums;
using AutoUpdater.Abstractions.Transports;

namespace AutoUpdater.Abstractions.Interfaces.Services;

public interface IElectronService
{
	Task<ElectronBuilderInfo> GetLatestYml(string app, AppArch arch);
}