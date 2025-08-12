using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations.Schema;

namespace Million.Domain.Entities
{
    public class Property
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = null!;

        [BsonElement("name")]
        public string Name { get; set; } = null!;

        [BsonElement("address")]
        public string Address { get; set; } = null!;

        [BsonElement("price")]
        public decimal Price { get; set; }

        [BsonElement("year")]
        public int Year { get; set; }

        [BsonElement("internalCode")]
        public string InternalCode { get; set; } = null!;

        [BsonElement("idOwner")]
        public string IdOwner { get; set; } = null!;

        [NotMapped]
        public string? OwnerName { get; set; }
    }
}
