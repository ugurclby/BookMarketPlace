using BookMarketPlace.Core.CustomResponse;
using BookMarketPlace.Services.Order.Application.Dtos;
using MediatR;

namespace BookMarketPlace.Services.Order.Application.Commands
{
    public class CreateOrderCommand : IRequest<Response<CreatedOrderDto>>
    {
        public string BuyerId { get; set; }
        public AdressDto Adress { get; set; }
        public List<OrderItemDto> OrderItems { get; set; }
    }
}
