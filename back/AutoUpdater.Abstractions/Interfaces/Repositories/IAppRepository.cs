using AutoUpdater.Abstractions.Enums;
using AutoUpdater.Abstractions.Models;

namespace AutoUpdater.Abstractions.Interfaces.Repositories
{
    public interface IAppRepository
    {
        Task Add(App app);
        Task Delete(string name, AppVersion version, AppArch arch);
        Task<List<AppMetadata>> GetAllMetadata(string name);
        Task<byte[]> GetBinary(string name, AppVersion version, AppArch arch);
        Task<Dictionary<AppArch, AppVersion>> GetLatestVersions(string name);
        Task<AppVersion> GetLatestVersions(string name, AppArch arch);
        Task<string[]> GetApps();
        Task<AppVersion> GetLatestVersion(string name);
    }
}