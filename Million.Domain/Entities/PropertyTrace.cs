using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Million.Domain.Entities
{
    public class PropertyTrace
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string IdPropertyTrace { get; set; } = null!;

        [BsonElement("idProperty")]
        public string IdProperty { get; set; } = null!;

        [BsonElement("saleDate")]
        public DateTime SaleDate { get; set; }

        [BsonElement("value")]
        public decimal Value { get; set; }
    }
}
