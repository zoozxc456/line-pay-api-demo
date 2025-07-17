namespace LinePayDemo.Order.Domain;

public class OrderDetail
{
    public Guid Id { get; private set; }
    public Guid OrderId { get; private set; }
    public decimal Subtotal { get; private set; }
    public OrderItemData OrderItemData { get; private set; } = null!;
    public Order Order { get; private set; } = null!;

    private OrderDetail()
    {
    }

    public OrderDetail(Guid orderId, Guid productId, int quantity, decimal unitPrice)
    {
        Id = Guid.NewGuid();
        OrderId = orderId;
        OrderItemData = new OrderItemData(productId, quantity, unitPrice);
        Subtotal = quantity * unitPrice;
    }
}