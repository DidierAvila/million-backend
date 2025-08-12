using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Million.Domain.Entities
{
    public class PropertyImage
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string IdPropertyImage { get; set; } = null!;

        [BsonElement("idProperty")]
        public string IdProperty { get; set; } = null!;

        [BsonElement("file")]
        public string File { get; set; } = null!;

        [BsonElement("enabled")]
        public bool Enabled { get; set; }
    }
}
