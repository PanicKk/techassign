using WMS.Models.Entities.Shared;

namespace WMS.Models.Entities;

public class OrderItem : BaseEntity
{
    public Guid OrderId { get; set; }
    public string ProductName { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    
    public Order Order { get; set; }
}