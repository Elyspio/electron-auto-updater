using Abstractions.Enums;
using Abstractions.Interfaces.Repositories;
using Abstractions.Models;
using Db.Entities;
using Db.Repositories.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MongoDB.Driver.GridFS;
using Abstractions.Exceptions;

namespace Db.Repositories;

internal class AppRepository : BaseRepository<AppEntity>, IAppRepository
{
    private readonly GridFSBucket gridFsBucket;
    private new readonly ILogger<AppRepository> logger;

    public AppRepository(IConfiguration configuration, ILogger<AppRepository> logger) : base(configuration, logger)
    {
        gridFsBucket = new GridFSBucket(context.MongoDatabase, new GridFSBucketOptions
        {
            BucketName = CollectionName
        });
        this.logger = logger;
    }



    public async Task Delete(string name, AppVersion version, AppArch arch)
    {

        logger.LogInformation($"Delete - Entering - {name} {arch} {version}");

        var app = await Get(name, version, arch);
        await gridFsBucket.DeleteAsync(app.IdGridFs);
        await EntityCollection.FindOneAndDeleteAsync(f => f.Id == app.Id);
    
        logger.LogInformation($"Delete - Exiting - {name} {arch} {version}");

    }

    public async Task Add(App app)
    {
       logger.LogInformation($"Add - Entering - {app.Metadata.Name} {app.Metadata.Arch} {app.Metadata.Version}");

        var file = new AppEntity
        {
            Metadata = app.Metadata,
        };

        await EntityCollection.InsertOneAsync(file);

        var idGridFs = await gridFsBucket.UploadFromBytesAsync(file.Id.ToString(), app.Binary);

        file.IdGridFs = idGridFs;

        await EntityCollection.ReplaceOneAsync(f => f.Id == file.Id, file);

        logger.LogInformation($"Add - Exiting - {app.Metadata.Name} {app.Metadata.Arch} {app.Metadata.Version}");


    }

    public async Task<List<AppMetadata>> GetAllMetadata(string name)
    {
        logger.LogInformation($"GetAllMetadata - Entering - {name}");

        var metadatas = await EntityCollection
            .AsQueryable()
            .Where(app => app.Metadata.Name == name)
            .Select(app => app.Metadata)
            .ToListAsync();

        logger.LogInformation($"GetAllMetadata - Exiting - {name}");

        return metadatas;
    }


    private Task<AppEntity> Get(string name, AppVersion version, AppArch arch)
    {
        logger.LogDebug($"Get - Entering - {name} {arch} {version}");

        var app = EntityCollection
            .AsQueryable()
            .Where(app => app.Metadata.Name == name && app.Metadata.Version == version && app.Metadata.Arch == arch)
            .FirstOrDefaultAsync();
   
        logger.LogDebug($"Get - Exiting - {name} {arch} {version}");

        return app;
    }

    public async Task<byte[]> GetBinary(string name, AppVersion version, AppArch arch)
    {
        logger.LogInformation($"GetBinary - Entering - {name} {arch} {version}");
      
        var app = await Get(name, version, arch);

        if (app == default) throw new ApplicationNotFoundException(name, version, arch);


        var bytes = await gridFsBucket.DownloadAsBytesByNameAsync(app.Id.ToString());
        
        logger.LogInformation($"GetBinary - Exiting - {name} {arch} {version}");

        return bytes;
    }

    public async Task<Dictionary<AppArch, List<AppVersion>>> GetLatestVersion(string name)
    {

        logger.LogInformation($"GetLatestVersion - Entering - {name}");

        var apps = await EntityCollection
            .AsQueryable()
            .Where(app => app.Metadata.Name == name)
            .ToListAsync();

        var dict = new Dictionary<AppArch, List<AppVersion>>();

        foreach (AppArch arch in Enum.GetValues(typeof(AppArch)))
        {
            dict[arch] = new List<AppVersion>();

            foreach (var app in apps.Where(app => app.Metadata.Arch == arch))
            {
                dict[arch].Add(app.Metadata.Version);
            }
        }

        logger.LogInformation($"GetLatestVersion - Exiting - {name}");

        return dict;
    }

    public async Task<AppVersion> GetLatestVersion(string name, AppArch arch)
    {
        logger.LogInformation($"GetLatestVersion - Entering - {name} {arch}");
        
        var versions = await GetLatestVersion(name);
        var version =  versions[arch].Max();

        logger.LogInformation($"GetLatestVersion - Exiting - {name} {arch}");

        return version; 
    }

    public async Task<string[]> GetApps()
    {
        logger.LogInformation($"GetApps - Entering");

        var apps = (await EntityCollection.AsQueryable().Select(app => app.Metadata.Name).ToListAsync()).ToHashSet();

        logger.LogInformation($"GetApps - Exiting");

        return apps.ToArray();
    }

}