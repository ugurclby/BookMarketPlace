using BookMarketPlace.Core.CustomResponse;
using BookMarketPlace.Services.BasketApi.Dtos;

namespace BookMarketPlace.Services.BasketApi.Services
{
    public interface IBasketService
    {
        Task<Response<BasketDto>> GetBasket(string userId);
        Task<Response<bool>> SaveOrUpdate(BasketDto basketDto); 
        Task<Response<bool>> DeleteBasket(string userId);
        Task<Response<List<BasketDto>>> GetAllBasket();
    }
}
