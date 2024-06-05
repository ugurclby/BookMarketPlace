using BookMarketPlace.Core.CustomResponse;
using BookMarketPlace.Services.CatalogApi.Dtos;
using BookMarketPlace.Services.CatalogApi.Models;

namespace BookMarketPlace.Services.CatalogApi.Services
{
    public interface ICategoryService
    {
        Task<Response<List<CategoryDto>>> GetAll();
        Task<Response<CategoryDto>> GetById(string Id);
        Task<Response<CategoryDto>> CreateAsync(Category category);
    }
}
