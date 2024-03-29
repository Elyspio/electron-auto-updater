﻿using AutoUpdater.Abstractions.Extensions;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AutoUpdater.Abstractions.Models;

[BsonCollection("Apps")]
public class AppEntity : IComparable<AppEntity>
{
	[BsonId]
	[BsonRepresentation(BsonType.ObjectId)]
	public ObjectId Id { get; set; }

	[BsonRepresentation(BsonType.ObjectId)]
	public ObjectId IdGridFs { get; set; }

	public required AppMetadata Metadata { get; set; }

	public int CompareTo(AppEntity? other)
	{
		return Metadata.Version.CompareTo(other?.Metadata.Version);
	}
}