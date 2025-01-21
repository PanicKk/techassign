using WMS.Models.Enums;

namespace WMS.Models.Orders.v1.Shared;

public class OrderModel
{
    public Guid Id { get; set; }
    public string CustomerName { get; set; }
    public List<OrderItemModel> Items { get; set; }
    public decimal TotalAmount { get; set; }
    public OrderStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
}