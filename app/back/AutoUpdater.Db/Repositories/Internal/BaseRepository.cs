using AutoUpdater.Abstractions.Extensions;
using AutoUpdater.Db.Configs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace AutoUpdater.Db.Repositories.Internal;

public abstract class BaseRepository<T>
{
	protected readonly string CollectionName;
	protected readonly MongoContext Context;
	protected readonly ILogger<BaseRepository<T>> BaseLogger;

	protected BaseRepository(IConfiguration configuration, ILogger<BaseRepository<T>> baseLogger)
	{
		Context = new(configuration);
		CollectionName = GetAttributeCollectionName() ?? GetDefaultCollectionName();
		BaseLogger = baseLogger;
		CollectionName = typeof(T).Name[..^"Entity".Length];
		var pack = new ConventionPack
		{
			new EnumRepresentationConvention(BsonType.String)
		};

		ConventionRegistry.Register("EnumStringConvention", pack, t => true);
		BsonSerializer.RegisterSerializationProvider(new EnumAsStringSerializationProvider());
	}

	private string? GetAttributeCollectionName()
	{
		return (typeof(T).GetCustomAttributes(typeof(BsonCollectionAttribute), true).FirstOrDefault() as BsonCollectionAttribute)?.CollectionName;
	}


	private string GetDefaultCollectionName()
	{
		return typeof(T).Name[..^"Entity".Length];
	}


	protected IMongoCollection<T> EntityCollection => Context.MongoDatabase.GetCollection<T>(CollectionName);
}

public class EnumAsStringSerializationProvider : BsonSerializationProviderBase
{
	public override IBsonSerializer GetSerializer(Type type, IBsonSerializerRegistry serializerRegistry)
	{
		if (!type.IsEnum) return null;

		var enumSerializerType = typeof(EnumSerializer<>).MakeGenericType(type);
		var enumSerializerConstructor = enumSerializerType.GetConstructor(new[]
		{
			typeof(BsonType)
		});
		var enumSerializer = (IBsonSerializer) enumSerializerConstructor.Invoke(new object[]
		{
			BsonType.String
		});

		return enumSerializer;
	}
}