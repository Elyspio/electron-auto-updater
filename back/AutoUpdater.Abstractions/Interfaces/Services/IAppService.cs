using AutoUpdater.Abstractions.Enums;
using AutoUpdater.Abstractions.Models;

namespace AutoUpdater.Abstractions.Interfaces.Services;

public interface IAppService
{
    Task Add(App app);
    Task Delete(string name, AppVersion version, AppArch arch);
    Task<byte[]> GetBinary(string name, AppVersion version, AppArch arch);
    Task<List<AppMetadata>> GetAllMetadata(string name);
    Task<Dictionary<AppArch, AppVersion>> GetLatestArchSpecificVersion(string name);
    Task<AppVersion> GetLatestArchSpecificVersion(string name, AppArch arch);
    Task<string[]> GetApps();
    Task<AppVersion> GetLatestVersion(string name);
}