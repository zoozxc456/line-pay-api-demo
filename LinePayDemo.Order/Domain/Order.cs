using LinePayDemo.Order.Enums;
using LinePayDemo.Ledger.Domain.Transaction;

namespace LinePayDemo.Order.Domain;

public class Order
{
    public Guid OrderId { get; private set; }
    public Guid BuyerId { get; private set; }
    public decimal TotalAmount { get; private set; }
    public DateTime OrderDate { get; private set; }
    public OrderStatus Status { get; private set; }
    private List<OrderDetail> _OrderDetails { get; set; } = [];
    public IReadOnlyList<OrderDetail> OrderDetails => _OrderDetails;
    public Guid? LedgerTransactionId { get; set; }
    public Transaction? LedgerTransaction { get; set; }

    public Order()
    {
    }

    public static Order Create(Guid buyerId, List<OrderItemData> items)
    {
        if (!items.Any())
        {
            throw new ArgumentException("Order must have at least one item.");
        }

        if (buyerId.Equals(Guid.Empty))
        {
            throw new ArgumentException("Buyer ID cannot be empty.");
        }

        var order = new Order
        {
            OrderId = Guid.NewGuid(),
            BuyerId = buyerId,
            TotalAmount = decimal.Zero,
            OrderDate = DateTime.UtcNow,
            Status = OrderStatus.Pending
        };

        foreach (var item in items)
        {
            order.AddOrderDetail(item.ProductId, item.Quantity, item.UnitPrice);
        }

        order.CalculateTotalAmount();

        return order;
    }

    private void AddOrderDetail(Guid productId, int quantity, decimal unitPrice)
    {
        if (quantity <= 0)
        {
            throw new ArgumentException("商品數量必須為正數。", nameof(quantity));
        }

        if (unitPrice < 0)
        {
            throw new ArgumentException("商品單價不能為負數。", nameof(unitPrice));
        }

        _OrderDetails.Add(new OrderDetail(OrderId, productId, quantity, unitPrice));
    }

    private void CalculateTotalAmount()
    {
        var subtotalFromDetails = OrderDetails.Sum(od => od.Subtotal);
        TotalAmount = subtotalFromDetails;

        if (TotalAmount < 0) TotalAmount = 0;
    }

    public void AttachLedgerTransaction(Guid transactionId)
    {
        LedgerTransactionId = transactionId;
    }

    public void MarkAsPaid()
    {
        if (Status != OrderStatus.Pending && Status != OrderStatus.Failed)
        {
            throw new InvalidOperationException($"訂單狀態為 {Status}，無法標記為已支付。");
        }

        Status = OrderStatus.Paid;
    }

    public void MarkAsCancelled()
    {
        if (Status is OrderStatus.Paid or OrderStatus.Refunded)
        {
            throw new InvalidOperationException($"訂單狀態為 {Status}，無法取消。");
        }

        Status = OrderStatus.Cancelled;
    }

    public void MarkAsFailed()
    {
        if (Status is OrderStatus.Paid or OrderStatus.Refunded)
        {
            throw new InvalidOperationException($"訂單狀態為 {Status}，無法標記為失敗。");
        }

        Status = OrderStatus.Failed;
    }

    public void MarkAsRefunded()
    {
        if (Status != OrderStatus.Paid)
        {
            throw new InvalidOperationException($"訂單狀態為 {Status}，無法標記為已退款。");
        }

        Status = OrderStatus.Refunded;
    }
}