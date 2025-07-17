namespace LinePayDemo.Order.Domain;

public class OrderItemData
{
    public Guid ProductId { get; private set; }
    public int Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }
    public Product.Domain.Product Product { get; private set; } = null!;

    private OrderItemData()
    {
    }

    public OrderItemData(Guid productId, int quantity, decimal unitPrice)
    {
        ProductId = productId;
        Quantity = quantity;
        UnitPrice = unitPrice;
    }
}