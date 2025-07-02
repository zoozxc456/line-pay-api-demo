using LinePayDemo.Order.Enums;
using LinePayDemo.Order.Repositories;
using LinePayDemo.Product.Services;
using Microsoft.Extensions.Logging;

namespace LinePayDemo.Order.Services;

public class OrderService(
    IOrderRepository orderRepository,
    IProductService productService,
    ILogger<OrderService> logger)
    : IOrderService
{
    public async Task<OrderCreationResult> CreateOrderAsync(Guid userId, Guid productId, int quantity)
    {
        var product = await productService.GetProductByIdAsync(productId);
        if (product == null)
        {
            return OrderCreationResult.Fail("找不到商品。");
        }

        var totalAmount = product.Price * quantity;
        var orderId = Guid.NewGuid();

        var newOrder = new Models.Order
        {
            OrderId = orderId,
            UserId = userId,
            ProductId = productId,
            Quantity = quantity,
            TotalAmount = totalAmount,
            OrderDate = DateTime.Now,
            Status = OrderStatus.Pending
        };

        await orderRepository.AddAsync(newOrder);
        logger.LogInformation($"訂單 {orderId} 為使用者 {userId} 建立，總計 {totalAmount} 點。");
        return OrderCreationResult.Success(orderId, totalAmount);
    }

    public async Task<bool> MarkOrderAsPaidAsync(Guid orderId)
    {
        var order = await orderRepository.GetByOrderIdAsync(orderId);
        if (order == null)
        {
            logger.LogWarning($"嘗試將未知訂單 {orderId} 標記為已支付。");
            return false;
        }

        order.Status = OrderStatus.Paid;
        await orderRepository.UpdateAsync(order);
        logger.LogInformation($"訂單 {orderId} 標記為已支付。");
        return true;
    }

    public async Task<bool> CancelOrderAsync(Guid orderId)
    {
        var order = await orderRepository.GetByOrderIdAsync(orderId);
        if (order == null)
        {
            logger.LogWarning($"嘗試取消未知訂單 {orderId}。");
            return false;
        }

        order.Status = OrderStatus.Cancelled;
        await orderRepository.UpdateAsync(order);
        logger.LogInformation($"訂單 {orderId} 標記為已取消。");
        return true;
    }
}