using Abstractions.Enums;
using Abstractions.Models;

namespace Abstractions.Interfaces.Services;

public interface IAppService
{
    Task Add(App app);
    Task Delete(string name, AppVersion version, AppArch arch);
    Task<List<AppMetadata>> GetAllMetadata(string name);
    Task<byte[]> GetBinary(string name, AppVersion version, AppArch arch);
    Task<Dictionary<AppArch, List<AppVersion>>> GetLatestVersion(string name);
    Task<AppVersion> GetLatestVersion(string name, AppArch arch);
    Task<string[]> GetApps();
}