using AutoMapper;
using BookMarketPlace.Services.CatalogApi.Dtos;
using BookMarketPlace.Services.CatalogApi.Models;

namespace BookMarketPlace.Services.CatalogApi.Mapping
{
    public class CatalogApiMapping:Profile
    {
        public CatalogApiMapping()
        {
            CreateMap<Book, BookDto>().ReverseMap();
            CreateMap<Book, BookCreateDto>().ReverseMap();
            CreateMap<Book, BookUpdateDto>().ReverseMap();

            CreateMap<Author, AuthorDto>().ReverseMap();
            CreateMap<Author, AuthorCreateDto>().ReverseMap();
            CreateMap<Author, AuthorUpdateDto>().ReverseMap();

            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<Category, CategoryCreateDto>().ReverseMap(); 
        }
    }
}
