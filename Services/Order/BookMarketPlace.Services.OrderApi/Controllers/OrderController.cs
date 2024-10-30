using BookMarketPlace.Core.CustomController;
using BookMarketPlace.Core.Services;
using BookMarketPlace.Services.Order.Application.Commands;
using BookMarketPlace.Services.Order.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BookMarketPlace.Services.OrderApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : CustomBaseController
    {
        private readonly IMediator _mediator;
        private readonly ISharedIdentityService _sharedIdentityService;

        public OrderController(IMediator mediator, ISharedIdentityService sharedIdentityService)
        {
            _mediator = mediator;
            _sharedIdentityService = sharedIdentityService;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var response = await _mediator.Send(new GetOrdersByUserIdQuery() { UserId= _sharedIdentityService.getUserId});
            return CreateActionResult(response);
        }

        [HttpPost]
        public async Task<IActionResult> SaveOrder(CreateOrderCommand orderCommand)
        {
            var response = await _mediator.Send(orderCommand);
            return CreateActionResult(response);
        }
    }
}
