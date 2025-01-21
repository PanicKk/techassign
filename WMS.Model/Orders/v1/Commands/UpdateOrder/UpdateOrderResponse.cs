using WMS.Models.Orders.v1.Shared;

namespace WMS.Models.Orders.v1.Commands.UpdateOrder;

public class UpdateOrderResponse
{
    public Guid Id { get; set; }
    public bool OrderUpdated { get; set; }
    public List<OrderItemModel> Items { get; set; }
    public decimal TotalAmount { get; set; }
}