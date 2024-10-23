using AutoMapper;
using BookMarketPlace.Services.Order.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMarketPlace.Services.Order.Application.Mapping
{
    public class CustomMapping:Profile
    {
        public CustomMapping() { 
        
            CreateMap<Domain.OrderAggragate.Order,OrderDto>().ReverseMap();
            CreateMap<Domain.OrderAggragate.Adress, AdressDto>().ReverseMap();
            CreateMap<Domain.OrderAggragate.OrderItem, OrderItemDto>().ReverseMap(); 
        }
    }
}
