namespace WMS.Models.Orders.v1.Shared;

public class OrderItemModel
{
    public string ProductName { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}