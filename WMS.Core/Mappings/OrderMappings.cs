using AutoMapper;
using WMS.Models.Entities;
using WMS.Models.Orders.v1.Commands.CreateOrder;
using WMS.Models.Orders.v1.Queries.GetOrders;
using WMS.Models.Orders.v1.Shared;

namespace WMS.Core.Mappings;

public class OrderMappings : Profile
{
    public OrderMappings()
    {
        CreateMap<CreateOrderCommand, Order>()
            .ForMember(dest => dest.TotalAmount, opt => opt.Ignore())
            .ForMember(dest => dest.Status, opt => opt.Ignore()) 
            .ReverseMap();

        CreateMap<Order, OrderModel>()
            .ReverseMap();
        
        CreateMap<OrderItem, OrderItemModel>()
            .ReverseMap()
            .ForMember(dest => dest.Order, opt => opt.Ignore());

        CreateMap<Order, OrderListModel>()
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items.Count))
            .ReverseMap();
    }
}