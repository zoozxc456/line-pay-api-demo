using LinePayDemo.Order.Domain;
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

        var totalAmount = product.UnitPrice * quantity;
        var orderId = Guid.NewGuid();

        var newOrder = Domain.Order.Create(userId, [new OrderItemData(productId, quantity, product.UnitPrice)]);

        await orderRepository.AddAsync(newOrder);
        logger.LogInformation($"訂單 {orderId} 為使用者 {userId} 建立，總計 {totalAmount} 點。");
        return OrderCreationResult.Success(newOrder.OrderId, totalAmount);
    }

    public async Task<OrderCreationResult> CreateTopUpOrderAsync(Guid userId, decimal amount)
    {
        var product = await productService.GetProductByIdAsync(Guid.Parse("cf618096-39db-4a9d-8f67-0141cac8c8d7"));
        if (product == null)
        {
            return OrderCreationResult.Fail("找不到商品。");
        }

        var newOrder = Domain.Order.Create(userId, [new OrderItemData(Guid.Parse("cf618096-39db-4a9d-8f67-0141cac8c8d7"), 1, amount)]);
        await orderRepository.AddAsync(newOrder);
        logger.LogInformation($"訂單 {newOrder.OrderId} 為使用者 {userId} 建立，總計 {amount} 點。");
        return OrderCreationResult.Success(newOrder.OrderId, amount);
    }

    public async Task<bool> MarkOrderAsPaidAsync(Guid orderId)
    {
        var order = await orderRepository.GetByOrderIdAsync(orderId);
        if (order == null)
        {
            logger.LogWarning($"嘗試將未知訂單 {orderId} 標記為已支付。");
            return false;
        }

        order.MarkAsPaid();

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

        order.MarkAsCancelled();

        await orderRepository.UpdateAsync(order);
        logger.LogInformation($"訂單 {orderId} 標記為已取消。");
        return true;
    }
}