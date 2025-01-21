using WMS.Models.Orders.v1.Shared;

namespace WMS.Models.Orders.v1.Commands.DeleteOrder;

public class DeleteOrderResponse
{
    public bool Deleted { get; set; }
    public OrderModel Order { get; set; }
}