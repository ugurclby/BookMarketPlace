using BookMarketPlace.Core.CustomController;
using BookMarketPlace.Core.Services;
using BookMarketPlace.Services.DiscountApi.Models;
using BookMarketPlace.Services.DiscountApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookMarketPlace.Services.DiscountApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountController : CustomBaseController
    {
        private readonly ISharedIdentityService _sharedIdentityService;
        private readonly IDiscountService _discountService;

        public DiscountController(ISharedIdentityService sharedIdentityService, IDiscountService discountService)
        {
            _sharedIdentityService = sharedIdentityService;
            _discountService = discountService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _discountService.GetAll();
            return CreateActionResult(response);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById(int Id)
        {
            var response = await _discountService.GetById(Id);
            return CreateActionResult(response);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]/{code}")]
        public async Task<IActionResult> GetByCodeAndUserId(string code)
        {
            var userId = _sharedIdentityService.getUserId;
            var response = await _discountService.GetByCodeAndUserId(code, userId);
            return CreateActionResult(response);
        }

        [HttpPost]
        public async Task<IActionResult> Save(Discount discount)
        {
            var result=await _discountService.Save(discount);
            return CreateActionResult(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update(Discount discount)
        {
            var result = await _discountService.Update(discount);
            return CreateActionResult(result);
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteById(int Id)
        {
            var result = await _discountService.DeleteById(Id);
            return CreateActionResult(result);
        } 

    }
}
