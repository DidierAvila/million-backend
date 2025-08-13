using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Million.Domain.Entities
{
    public class Owner
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = null!;

        [BsonElement("name")]
        public string Name { get; set; } = null!;

        [BsonElement("address")]
        public string Address { get; set; } = null!;

        [BsonElement("photo")]
        public string? Photo { get; set; }

        [BsonElement("birthDate")]
        public DateTime BirthDate { get; set; }
    }
}
