using BookMarketPlace.Core.CustomResponse;
using BookMarketPlace.Services.CatalogApi.Dtos;

namespace BookMarketPlace.Services.CatalogApi.Services
{
    public interface IBookService
    {
        Task<Response<List<BookDto>>> GetAllAsync();
        Task<Response<BookDto>> GetByIdAsync(string Id);
        Task<Response<List<BookDto>>> GetAllByUserAsync(string userId);
        Task<Response<BookDto>> CreateAsync(BookCreateDto book);
        Task<ResponseNoContent> UpdateAsync(BookUpdateDto book);
        Task<ResponseNoContent> DeleteAsync(string id);
    }
}