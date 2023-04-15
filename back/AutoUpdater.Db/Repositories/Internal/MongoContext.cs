using AutoUpdater.Db.Configs;
using AutoUpdater.Core.Utils;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace AutoUpdater.Db.Repositories.Internal;

public class MongoContext
{
    public MongoContext(IConfiguration configuration)
    {
        var cs = configuration["Database"];

        var url = new MongoUrl(cs);
        var client = new MongoClient(url);
        Console.WriteLine($"Connecting to Database '{url.DatabaseName}' on {url.Url} as {url.Username}");
        MongoDatabase = client.GetDatabase(url.DatabaseName);
    }

    /// <summary>
    ///     Récupération de la IMongoDatabase
    /// </summary>
    /// <returns></returns>
    public IMongoDatabase MongoDatabase { get; }
}