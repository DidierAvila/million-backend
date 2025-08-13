using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Million.Infrastructure.Migrations
{
    public class Migration
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        
        [BsonElement("name")]
        public string Name { get; set; }
        
        [BsonElement("version")]
        public int Version { get; set; }
        
        [BsonElement("description")]
        public string Description { get; set; }
        
        [BsonElement("applied")]
        public DateTime Applied { get; set; }
        
        [BsonElement("success")]
        public bool Success { get; set; }
    }
}
