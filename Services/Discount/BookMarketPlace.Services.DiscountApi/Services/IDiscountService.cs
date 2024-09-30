using BookMarketPlace.Core.CustomResponse;
using BookMarketPlace.Services.DiscountApi.Models;

namespace BookMarketPlace.Services.DiscountApi.Services
{
    public interface IDiscountService
    {
        Task<ICustomResponse<List<Discount>>> GetAll();
        Task<ICustomResponse<Discount>> GetById(int id); 
        Task<ICustomResponse<Discount>> Save(Discount discount);
        Task<ICustomResponse<bool>> Update(Discount discount);
        Task<ICustomResponse<bool>> DeleteById(int id); 
        Task<ICustomResponse<Discount>> GetByCodeAndUserId(string code, string userId);
    }
}
