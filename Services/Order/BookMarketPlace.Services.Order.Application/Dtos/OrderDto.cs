namespace BookMarketPlace.Services.Order.Application.Dtos
{
    public class OrderDto
    {
        public int Id{ get; set; }
        public DateTime CreatedDate { get; set; }
        public AdressDto Adress { get; set; }
        public string BuyerId { get; set; }
        private List<OrderItemDto> OrderItem { get; set; } 
    }
}
