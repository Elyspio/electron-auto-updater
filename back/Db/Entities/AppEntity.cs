using Abstractions.Models;
using Db.Configs;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Db.Entities;

[BsonCollection("Apps")]
public class AppEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId Id { get; set; }
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId IdGridFs { get; set; }
    public AppMetadata Metadata { get; set; }
}