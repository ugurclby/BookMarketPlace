using BookMarketPlace.Core.Messages;
using BookMarketPlace.Core.Services;
using BookMarketPlace.Services.BasketApi.Services; 
using MassTransit; 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMarketPlace.Services.BasketApi.Consumers
{
    public class BookNameChangedEventConsumer : IConsumer<BookNameChangedEvent>
    {
        private readonly IBasketService _basketService;
        private readonly ISharedIdentityService _sharedIdentityService;

        public BookNameChangedEventConsumer(IBasketService basketService, ISharedIdentityService sharedIdentityService)
        {
            _basketService = basketService;
            _sharedIdentityService = sharedIdentityService;
        }

        public async Task Consume(ConsumeContext<BookNameChangedEvent> context)
        {
            var existsBasket2 = await _basketService.GetAllBasket();

            existsBasket2.Data.ForEach(y=>
            {
                y.BasketItems.ForEach(x =>
                {
                    if (x.BookId == context.Message.BookId)
                    {
                        x.BookName = context.Message.NewName;
                        x.Quantity = x.Quantity;
                        x.Price = x.Price;  
                        x.BookId = context.Message.BookId;
                    }
                     _basketService.SaveOrUpdate(y);
                }); 
            }); 
        }
    }
}
