using BookMarketPlace.Core.CustomResponse;
using BookMarketPlace.Services.BasketApi.Dtos;
using System.Text.Json;

namespace BookMarketPlace.Services.BasketApi.Services
{
    public class BasketService : IBasketService
    {
        private readonly RedisService _redisService; 
        public BasketService(RedisService redisService)
        {
            _redisService=redisService;
        }
        public async Task<Response<bool>> DeleteBasket(string userId)
        {
           var status= await _redisService.GetDatabase().KeyDeleteAsync(userId);
            return status ? Response<bool>.Success(true, 204) :
                Response<bool>.Error(new List<string> { "Sepet bulunamadı" }, 204);
        }

        public async Task<Response<BasketDto>> GetBasket(string userId)
        {
            var existsBasket = await _redisService.GetDatabase().StringGetAsync(userId);
            if (String.IsNullOrEmpty(existsBasket))
            {
                return Response<BasketDto>.Error(new List<string> { "Sepet Bulunamadı" }, 404);
            }
            return Response<BasketDto>.Success(JsonSerializer.Deserialize<BasketDto>(existsBasket), 200);
        }

        public async Task<Response<bool>> SaveOrUpdate(BasketDto basketDto)
        {
            var status = await _redisService.GetDatabase().StringSetAsync(basketDto.UserId,JsonSerializer.Serialize(basketDto));

            return status ? Response<bool>.Success(true, 204) :
                Response<bool>.Error(new List<string> { "Sepet güncellenemedi" }, 204);

        }
    }
}
