using MongoDB.Bson.Serialization.Attributes;

namespace BookMarketPlace.Services.CatalogApi.Models
{
    public class Book
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        [BsonRepresentation(MongoDB.Bson.BsonType.Decimal128)]
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string BookCoverPicture { get; set; }
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string CategoryId { get; set; }
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public List<string> AuthorsIds { get; set; }
        public DateTime CreatedTime { get; set; }
        public string CreatedUser { get; set; } 
        [BsonIgnore]
        public Category Category { get; set; } 
        [BsonIgnore]
        public List<Author> Authors { get; set; } 
    }
}
