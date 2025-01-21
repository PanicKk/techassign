using WMS.Models.Enums;

namespace WMS.Models.Orders.v1.Queries.GetOrders;

public class OrderListModel
{
    public Guid Id { get; set; }
    public string CustomerName { get; set; }
    public int Items {get; set; }
    public decimal TotalAmount { get; set; }
    public OrderStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
}