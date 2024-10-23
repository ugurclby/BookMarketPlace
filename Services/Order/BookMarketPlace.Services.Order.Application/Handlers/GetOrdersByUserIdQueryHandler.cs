using BookMarketPlace.Core.CustomResponse;
using BookMarketPlace.Services.Order.Application.Dtos;
using BookMarketPlace.Services.Order.Application.Mapping;
using BookMarketPlace.Services.Order.Application.Queries;
using BookMarketPlace.Services.Order.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookMarketPlace.Services.Order.Application.Handlers
{
    public class GetOrdersByUserIdQueryHandler : IRequestHandler<GetOrdersByUserIdQuery, ICustomResponse<List<OrderDto>>>
    {
        private readonly OrderDbContext _context;
        public GetOrdersByUserIdQueryHandler(OrderDbContext context)
        {
            _context = context;
        }

        public async Task<ICustomResponse<List<OrderDto>>> Handle(GetOrdersByUserIdQuery request, CancellationToken cancellationToken)
        {
            var orders= await _context.Orders.Include(x => x.OrderItem).Where(x => x.BuyerId == request.UserId).ToListAsync();

            if (!orders.Any())
            {
                return Response<List<OrderDto>>.Success(new List<OrderDto>(), 200);
            }

            var listOrder= ObjectMapping.Mapper.Map<List<OrderDto>>(orders);

            return Response<List<OrderDto>>.Success(listOrder, 200);
        }
    }
}
