using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MinimalApi.Models
{
    public class StringItem
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("valor")]
        public string Valor { get; set; } = string.Empty;
    }
}
