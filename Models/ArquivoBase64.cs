using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MinimalApi.Models
{
    public class ArquivoBase64
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("nome")]
        public string Nome { get; set; } = string.Empty;

        [BsonElement("contentType")]
        public string ContentType { get; set; } = "application/octet-stream";

        [BsonElement("base64")]
        public string Base64 { get; set; } = string.Empty;

        [BsonElement("tamanhoBytes")]
        public long TamanhoBytes { get; set; }

        [BsonElement("criadoEm")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime CriadoEm { get; set; } = DateTime.Now;
    }
}
