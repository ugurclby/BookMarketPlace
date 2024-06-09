using BookMarketPlace.Core.CustomResponse;
using BookMarketPlace.Services.CatalogApi.Dtos; 

namespace BookMarketPlace.Services.CatalogApi.Services
{
    public interface IAuthorService
    {
        Task<ICustomResponse<List<AuthorDto>>> GetAllAsync();
        Task<ICustomResponse<AuthorDto>> GetByIdAsync(string Id);
        Task<ICustomResponse<AuthorDto>> CreateAsync(AuthorCreateDto author);
        Task<ICustomResponse<string>> UpdateAsync(AuthorUpdateDto author);
        Task<ICustomResponse<string>> DeleteAsync(string id);
    }
}
