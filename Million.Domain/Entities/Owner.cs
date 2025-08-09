using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Million.Domain.Entities
{
    public class Owner
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string IdOwner { get; set; } = null!;

        [BsonElement("name")]
        public string Name { get; set; } = null!;

        [BsonElement("birthDate")]
        public DateTime BirthDate { get; set; }
    }
}
