using BookMarketPlace.Services.Order.DomainCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMarketPlace.Services.Order.Domain.OrderAggragate
{
    public class Order : Entity, IAggregateRoot
    {
        public DateTime CreatedDate { get; private set; }
        public Adress Adress { get; private set; }
        public string BuyerId { get; private set; }
        private readonly List<OrderItem> _orderItem;
        public IReadOnlyCollection<OrderItem> OrderItem => _orderItem;
        public Order()
        {
                
        }
        public Order(string buyerId, Adress adress)
        {
            _orderItem = new List<OrderItem>();
            CreatedDate = DateTime.Now;
            BuyerId = buyerId;
            Adress = adress;
        }

        public void AddOrderItem(string productId,string productName,string pictureUrl,decimal price)
        {
            var existsOrder = _orderItem.Any(x => x.ProductId == productId);
            if (!existsOrder)
            {
                var orderItem = new OrderItem(productId, productName, pictureUrl, price);
                _orderItem.Add(orderItem);
            }
        }

        public decimal GetTotalPrice =>  _orderItem.Sum(x => x.Price);
    }
}
