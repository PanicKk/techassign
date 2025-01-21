using System.Text.Json.Serialization;
using MediatR;

namespace WMS.Models.Orders.v1.Commands.DeleteOrder;

public class DeleteOrderCommand : IRequest<DeleteOrderResponse>
{
    [JsonIgnore] 
    public Guid Id { get; set; }
}