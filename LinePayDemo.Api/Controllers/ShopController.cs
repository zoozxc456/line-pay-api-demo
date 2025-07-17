using LinePayDemo.Api.Contracts.Request;
using LinePayDemo.Api.Helpers;
using LinePayDemo.Order.Services;
using LinePayDemo.Payment.Services;
using Microsoft.AspNetCore.Mvc;

namespace LinePayDemo.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ShopController(IOrderService orderService, IPaymentService paymentService) : ControllerBase
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
        try
        {
            await paymentService.PurchasingProductAsync(request.UserId, orderResult.OrderId);
            await orderService.MarkOrderAsPaidAsync(orderResult.OrderId);
            return Ok(new ResponseModel
            {
                Message = "成功購買"
            });
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            await orderService.CancelOrderAsync(orderResult.OrderId); // 你可能需要不同的狀態，例如「支付失敗」
            return Ok(new ResponseModel
            {
                Message = $"點數扣除失敗"
            });
        }
    }
}