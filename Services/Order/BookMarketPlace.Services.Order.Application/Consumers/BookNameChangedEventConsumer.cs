using BookMarketPlace.Core.Messages;
using BookMarketPlace.Services.Order.Infrastructure;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMarketPlace.Services.Order.Application.Consumers
{
    public class BookNameChangedEventConsumer : IConsumer<BookNameChangedEvent>
    {private readonly OrderDbContext _orderDbContext;

        public BookNameChangedEventConsumer(OrderDbContext orderDbContext)
        {
            _orderDbContext = orderDbContext;
        }

        public async Task Consume(ConsumeContext<BookNameChangedEvent> context)
        {
            var orderItems = await _orderDbContext.OrderItems.Where(x => x.ProductId == context.Message.BookId).ToListAsync();

            orderItems.ForEach(x =>
            {
                x.UpdateOrderItem(context.Message.NewName, x.PictureUrl,x.Price );
            });

            await _orderDbContext.SaveChangesAsync();   
        }
    }
}
