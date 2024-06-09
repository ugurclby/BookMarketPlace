using BookMarketPlace.Core.CustomResponse;
using BookMarketPlace.Services.CatalogApi.Dtos;

namespace BookMarketPlace.Services.CatalogApi.Services
{
    public interface IBookService
    {
        Task<ICustomResponse<List<BookDto>>> GetAllAsync();
        Task<ICustomResponse<BookDto>> GetByIdAsync(string Id);
        Task<ICustomResponse<List<BookDto>>> GetAllByUserAsync(string userId);
        Task<ICustomResponse<BookDto>> CreateAsync(BookCreateDto book);
        Task<ICustomResponse<string>> UpdateAsync(BookUpdateDto book);
        Task<ICustomResponse<string>> DeleteAsync(string id);
    }
}