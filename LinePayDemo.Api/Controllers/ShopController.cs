using LinePayDemo.Api.Helpers;
using LinePayDemo.Order.Services;
using LinePayDemo.Transaction.Services;
using Microsoft.AspNetCore.Mvc;

namespace LinePayDemo.Api.Controllers;

public class PurchaseRequest
{
    public Guid UserId { get; set; }
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}

[ApiController]
[Route("api/[controller]")]
public class ShopController(IOrderService orderService, ITransactionService transactionService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Purchase([FromBody] PurchaseRequest request)
    {
        if (request.Quantity <= 0)
        {
            return BadRequest(new ResponseModel
            {
                Message = "數量必須大於 0。"
            });
        }

        // 在系統中建立訂單
        var orderResult = await orderService.CreateOrderAsync(request.UserId, request.ProductId, request.Quantity);

        if (!orderResult.IsSuccess)
        {
            return BadRequest(new ResponseModel
            {
                Message = orderResult.Message
            });
        }

        // 扣除點數
        var deductionResult = await transactionService.DeductPointsAsync(request.UserId, orderResult.Amount);

        if (deductionResult.IsSuccess)
        {
            // 標記訂單為已支付
            await orderService.MarkOrderAsPaidAsync(orderResult.OrderId);
            return Ok(new ResponseModel
            {
                Message = $"成功購買！剩餘點數：{deductionResult.RemainingBalance}"
            });
        }

        await orderService.CancelOrderAsync(orderResult.OrderId); // 你可能需要不同的狀態，例如「支付失敗」
        return Ok(new ResponseModel
        {
            Message = $"點數扣除失敗：{deductionResult.Message}"
        });
    }
}