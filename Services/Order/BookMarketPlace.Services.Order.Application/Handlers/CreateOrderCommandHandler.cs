using BookMarketPlace.Core.CustomResponse;
using BookMarketPlace.Services.Order.Application.Commands;
using BookMarketPlace.Services.Order.Application.Dtos;
using BookMarketPlace.Services.Order.Domain.OrderAggragate;
using BookMarketPlace.Services.Order.Infrastructure;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMarketPlace.Services.Order.Application.Handlers
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Response<CreatedOrderDto>>
    {
        private readonly OrderDbContext _orderDbContext;

        public CreateOrderCommandHandler(OrderDbContext orderDbContext)
        {
            _orderDbContext = orderDbContext;
        }

        public async Task<Response<CreatedOrderDto>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            Adress adress = new Adress(request.Adress.Province, request.Adress.District, request.Adress.Street, request.Adress.ZipCode, request.Adress.Line);
            

            Domain.OrderAggragate.Order order = new Domain.OrderAggragate.Order(request.BuyerId, adress);

            request.OrderItems.ForEach(x => order.AddOrderItem(x.ProductId, x.ProductName, x.PictureUrl, x.Price));
            
            await _orderDbContext.Orders.AddAsync (order);
            await _orderDbContext.SaveChangesAsync();

            return Response<CreatedOrderDto>.Success(new CreatedOrderDto() { OrderId = order.Id },200);

        }
    }
}
