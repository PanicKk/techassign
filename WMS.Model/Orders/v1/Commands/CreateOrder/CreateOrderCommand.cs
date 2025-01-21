using MediatR;
using WMS.Models.Orders.v1.Shared;

namespace WMS.Models.Orders.v1.Commands.CreateOrder;

public class CreateOrderCommand : IRequest<OrderModel>
{
    public string CustomerName { get; set; }
    public List<OrderItemModel> Items { get; set; }
}