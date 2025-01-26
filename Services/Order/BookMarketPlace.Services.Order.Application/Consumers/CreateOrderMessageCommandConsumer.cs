using BookMarketPlace.Core.Messages;
using BookMarketPlace.Services.Order.Infrastructure;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMarketPlace.Services.Order.Application.Consumers
{
    public class CreateOrderMessageCommandConsumer : IConsumer<CreateOrderMessageCommand>
    {
        private readonly OrderDbContext _orderDbContext;

        public CreateOrderMessageCommandConsumer(OrderDbContext orderDbContext)
        {
            _orderDbContext = orderDbContext;
        }

        public async Task Consume(ConsumeContext<CreateOrderMessageCommand> context)
        {
            var adress = new Domain.OrderAggragate.Adress(context.Message.Province, context.Message.District, context.Message.Street, context.Message.ZipCode, context.Message.Line);

            Domain.OrderAggragate.Order order = new Domain.OrderAggragate.Order(context.Message.BuyerId, adress);

            foreach (var item in context.Message.OrderItems)
            {
                order.AddOrderItem(item.ProductId, item.ProductName, item.PictureUrl, item.Price);
            }

            await _orderDbContext.AddAsync(order);

            _orderDbContext.SaveChanges(); 

        }
    }
}
