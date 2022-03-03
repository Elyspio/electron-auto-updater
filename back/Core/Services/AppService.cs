using Abstractions.Enums;
using Abstractions.Interfaces.Repositories;
using Abstractions.Interfaces.Services;
using Abstractions.Models;

namespace Core.Services
{
    public class AppService : IAppService
    {

        private readonly IAppRepository repository;

        public AppService(IAppRepository repository)
        {
            this.repository = repository;
        }

        public async Task Add(App app)
        {
            var metadatas = await GetAllMetadata(app.Metadata.Name);
            if (metadatas.Any(metadata => metadata.Arch == app.Metadata.Arch && metadata.Version.Raw == app.Metadata.Version.Raw))
            {
                await repository.Delete(app.Metadata.Name, app.Metadata.Version, app.Metadata.Arch);
            }
            await repository.Add(app);
        }

        public Task Delete(string name, AppVersion version, AppArch arch)
        {
            return repository.Delete(name, version, arch);
        }

        public Task<List<AppMetadata>> GetAllMetadata(string name)
        {
            return repository.GetAllMetadata(name);
        }

        public Task<byte[]> GetBinary(string name, AppVersion version, AppArch arch)
        {
            return repository.GetBinary(name, version, arch);
        }

        public Task<Dictionary<AppArch, List<AppVersion>>> GetLatestVersion(string name)
        {
            return repository.GetLatestVersion(name);
        }

        public Task<AppVersion> GetLatestVersion(string name, AppArch arch)
        {
            return repository.GetLatestVersion(name, arch);
        }

        public Task<string[]> GetApps()
        {
            return repository.GetApps();
        }
    }
}