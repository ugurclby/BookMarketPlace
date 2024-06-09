using BookMarketPlace.Core.CustomResponse;
using Microsoft.AspNetCore.Mvc;

namespace BookMarketPlace.Core.CustomController
{
    public class CustomBaseController: ControllerBase
    {
        public IActionResult CreateActionResult<T>(ICustomResponse<T> response)
        {
            return new ObjectResult(response)
            {
                StatusCode = response.StatusCode
            };
        }
    }
}
