using BookMarketPlace.Core.CustomController;
using BookMarketPlace.Core.CustomResponse;
using BookMarketPlace.Core.Messages;
using BookMarketPlace.Services.FakePaymentApi.Models;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookMarketPlace.Services.FakePaymentApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FakePaymentController : CustomBaseController
    {
        private readonly ISendEndpointProvider _sendEndPoint;

        public FakePaymentController(ISendEndpointProvider sendEndPoint)
        {
            _sendEndPoint = sendEndPoint;
        }

        [HttpPost]
        public async Task<IActionResult> ReceivePayment(PaymentDto paymentDto)
        {
            var sendEndPoint = await _sendEndPoint.GetSendEndpoint(new Uri("queue:create-order-service"));

            var createOrderMessage = new CreateOrderMessageCommand
            {
                BuyerId = paymentDto.Order.BuyerId,
                Province = paymentDto.Order.Adress.Province,
                District = paymentDto.Order.Adress.District,
                Line = paymentDto.Order.Adress.Line,
                Street = paymentDto.Order.Adress.Street,
                ZipCode = paymentDto.Order.Adress.ZipCode,
                OrderItems = paymentDto.Order.OrderItems.Select(x => new OrderItem
                {
                    PictureUrl = x.PictureUrl,
                    Price = x.Price,
                    ProductId = x.ProductId,
                    ProductName = x.ProductName 
                }).ToList() 
            };

            await sendEndPoint.Send(createOrderMessage);    

            return CreateActionResult(ResponseNoContent<object>.Success(200));
        }
    }
}