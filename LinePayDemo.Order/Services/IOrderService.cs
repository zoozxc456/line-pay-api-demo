namespace LinePayDemo.Order.Services;

public interface IOrderService
{
    Task<OrderCreationResult> CreateOrderAsync(Guid userId, Guid productId, int quantity);
    Task<bool> MarkOrderAsPaidAsync(Guid orderId);
    Task<bool> CancelOrderAsync(Guid orderId);
}

public class OrderCreationResult
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; }
    public Guid OrderId { get; set; }
    public decimal Amount { get; set; } // 訂單總金額

    public static OrderCreationResult Success(Guid orderId, decimal amount) =>
        new() { IsSuccess = true, OrderId = orderId, Amount = amount };

    public static OrderCreationResult Fail(string message) => new() { IsSuccess = false, Message = message };
}