using System.Text.Json.Serialization;
using MediatR;
using WMS.Models.Entities;
using WMS.Models.Orders.v1.Shared;

namespace WMS.Models.Orders.v1.Queries.GetOrder;

public class GetOrderQuery : IRequest<OrderModel>
{
    [JsonIgnore] 
    public Guid Id { get; set; }
}