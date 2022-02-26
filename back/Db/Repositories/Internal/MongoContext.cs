using Core.Utils;
using Db.Configs;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Db.Repositories.Internal;

public class MongoContext
{
    public MongoContext(IConfiguration configuration)
    {
        var conf = new DbConfig();
        configuration.GetSection(DbConfig.Section).Bind(conf);

        var host = Env.Get("DB_HOST", conf.Host);
        var username = Env.Get("DB_USERNAME", conf.Username);
        var password = Env.Get("DB_PASSWORD", conf.Password);
        var database = Env.Get("DB_DATABASE", conf.Database);
        var port = Env.Get("DB_PORT", conf.Port);
        var client = new MongoClient($"mongodb://{username}:{password}@{host}:{port}");
        Console.WriteLine($"Connecting to Database '{database}' on {host}:{port} as {username}");
        MongoDatabase = client.GetDatabase(database);
    }

    /// <summary>
    ///     Récupération de la IMongoDatabase
    /// </summary>
    /// <returns></returns>
    public IMongoDatabase MongoDatabase { get; }
}