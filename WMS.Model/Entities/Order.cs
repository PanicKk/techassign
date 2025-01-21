using WMS.Models.Entities.Shared;
using WMS.Models.Enums;

namespace WMS.Models.Entities;

public class Order : BaseEntity
{
    public string CustomerName { get; set; }
    public List<OrderItem> Items { get; set; }
    public decimal TotalAmount { get; set; }
    public OrderStatus Status { get; set; }
}