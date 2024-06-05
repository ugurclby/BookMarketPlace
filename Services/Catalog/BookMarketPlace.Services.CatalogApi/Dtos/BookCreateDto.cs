﻿namespace BookMarketPlace.Services.CatalogApi.Dtos
{
    public class BookCreateDto
    { 
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string BookCoverPicture { get; set; }
        public string CategoryId { get; set; }
        public List<string> AuthorsIds { get; set; }
        public string CreatedUser { get; set; } 
        
    }
}