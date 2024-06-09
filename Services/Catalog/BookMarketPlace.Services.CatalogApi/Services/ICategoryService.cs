using BookMarketPlace.Core.CustomResponse;
using BookMarketPlace.Services.CatalogApi.Dtos;
using BookMarketPlace.Services.CatalogApi.Models;

namespace BookMarketPlace.Services.CatalogApi.Services
{
    public interface ICategoryService
    {
        Task<ICustomResponse<List<CategoryDto>>> GetAllAsync();
        Task<ICustomResponse<CategoryDto>> GetByIdAsync(string Id);
        Task<ICustomResponse<CategoryDto>> CreateAsync(CategoryCreateDto category);
        Task<ICustomResponse<string>> UpdateAsync(CategoryDto category);
        Task<ICustomResponse<string>> DeleteAsync(string id);
    }
}
