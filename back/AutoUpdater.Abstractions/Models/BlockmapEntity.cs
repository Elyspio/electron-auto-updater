using AutoUpdater.Abstractions.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AutoUpdater.Abstractions.Models;

public class BlockmapEntity
{
	[BsonId]
	public ObjectId Id;

	public required string App { get; set; }
	public required AppArch Arch { get; set; }
	public required AppVersion Version { get; set; }
	public required byte[] Content { get; set; }
}