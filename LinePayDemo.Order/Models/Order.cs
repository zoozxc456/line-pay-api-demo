using LinePayDemo.Order.Enums;

namespace LinePayDemo.Order.Models;

public class Order
{
    public Guid OrderId { get; set; } // 內部訂單 ID
    public Guid UserId { get; set; }
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal TotalAmount { get; set; } // 總點數金額
    public DateTime OrderDate { get; set; }
    public OrderStatus Status { get; set; } // Pending, Paid, Cancelled, Failed
}