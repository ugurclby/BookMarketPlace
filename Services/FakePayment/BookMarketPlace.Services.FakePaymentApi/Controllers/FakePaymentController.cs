using BookMarketPlace.Core.CustomController;
using BookMarketPlace.Core.CustomResponse;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookMarketPlace.Services.FakePaymentApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FakePaymentController : CustomBaseController
    {
        [HttpPost]
        public IActionResult ReceivePayment()
        {
            return CreateActionResult(ResponseNoContent<object>.Success(200));
        }
    }
}