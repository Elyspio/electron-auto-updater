using AutoUpdater.Db.Configs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace AutoUpdater.Db.Repositories.Internal;

public abstract class BaseRepository<T>
{
	protected readonly string CollectionName;
	protected readonly MongoContext Context;
	protected readonly ILogger<BaseRepository<T>> Logger;

	protected BaseRepository(IConfiguration configuration, ILogger<BaseRepository<T>> logger)
	{
		Context = new(configuration);
		CollectionName = (typeof(T).GetCustomAttributes(typeof(BsonCollectionAttribute), true).FirstOrDefault() as BsonCollectionAttribute)!.CollectionName;
		Logger = logger;
	}

	protected IMongoCollection<T> EntityCollection => Context.MongoDatabase.GetCollection<T>(CollectionName);
}