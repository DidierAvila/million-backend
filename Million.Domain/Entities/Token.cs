using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Million.Domain.Entities
{
    public class Token
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = null!;

        [BsonElement("userId")]
        public string UserId { get; set; } = null!;

        [BsonElement("tokenValue")]
        public string? TokenValue { get; set; }

        [BsonElement("createdDate")]
        public DateTime CreatedDate { get; set; }

        [BsonElement("expirationDate")]
        public DateTime ExpirationDate { get; set; }

        [BsonElement("status")]
        public bool Status { get; set; }
    }
}
