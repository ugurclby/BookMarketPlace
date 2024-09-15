using BookMarketPlace.Core.CustomController;
using BookMarketPlace.Core.CustomResponse;
using BookMarketPlace.Core.Services;
using BookMarketPlace.Services.BasketApi.Dtos;
using BookMarketPlace.Services.BasketApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookMarketPlace.Services.BasketApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : CustomBaseController
    {
        private readonly IBasketService _basketService;
        private readonly ISharedIdentityService _sharedIdentityService;

        public BasketController(IBasketService basketService, ISharedIdentityService sharedIdentityService)
        {
            _basketService = basketService;
            _sharedIdentityService = sharedIdentityService;
        }
        [HttpGet]
        public async Task<IActionResult> Get() {

            var response=await _basketService.GetBasket(_sharedIdentityService.getUserId);
            return CreateActionResult(response);
        }
        [HttpPost]
        public async Task<IActionResult> SaveOrUpdate(BasketDto basketDto)
        {
            var response = await _basketService.SaveOrUpdate(basketDto);
            return CreateActionResult(response);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete()
        {
            var response = await _basketService.DeleteBasket(_sharedIdentityService.getUserId);
            return CreateActionResult(response);
        }
    }
}
