using MongoDB.Bson.Serialization.Attributes;

namespace BookMarketPlace.Services.CatalogApi.Models
{
    public class Author
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; } 
        public string Name { get; set; } 
        public string Surname { get; set; }  
    }
}
