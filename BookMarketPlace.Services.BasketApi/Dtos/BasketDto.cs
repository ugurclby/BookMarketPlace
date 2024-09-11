namespace BookMarketPlace.Services.BasketApi.Dtos
{
    public class BasketDto
    {
        public string UserId { get; set; }
        public List<BasketItemDto> BasketItems { get; set; }
        public int DiscountCode { get; set; }
        public decimal TotalPrice
        {
            get => BasketItems.Sum(x => x.Price * x.Quantity);
        }
    }
}
