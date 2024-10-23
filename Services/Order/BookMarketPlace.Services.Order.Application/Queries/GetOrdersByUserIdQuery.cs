using BookMarketPlace.Core.CustomResponse;
using BookMarketPlace.Services.Order.Application.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMarketPlace.Services.Order.Application.Queries
{
    public class GetOrdersByUserIdQuery :IRequest<ICustomResponse<List<OrderDto>>>
    {
        public string UserId { get; set; }
    }
}
