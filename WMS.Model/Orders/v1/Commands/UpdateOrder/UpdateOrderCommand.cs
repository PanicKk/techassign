using System.Text.Json.Serialization;
using MediatR;
using WMS.Models.Orders.v1.Shared;

namespace WMS.Models.Orders.v1.Commands.UpdateOrder;

public class UpdateOrderCommand : IRequest<UpdateOrderResponse>
{
    [JsonIgnore]
    public Guid Id { get; set; }
    public List<OrderItemModel> Items { get; set; }
}