using Db.Configs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Db.Repositories.Internal;

public abstract class BaseRepository<T>
{
    protected readonly string CollectionName;
    protected readonly MongoContext context;
    protected readonly ILogger<BaseRepository<T>> logger;

    protected BaseRepository(IConfiguration configuration, ILogger<BaseRepository<T>> logger)
    {
        context = new MongoContext(configuration);
        CollectionName = (typeof(T).GetCustomAttributes(typeof(BsonCollectionAttribute), true).FirstOrDefault() as BsonCollectionAttribute).CollectionName;
        this.logger = logger;
    }

    protected IMongoCollection<T> EntityCollection => context.MongoDatabase.GetCollection<T>(CollectionName);
}