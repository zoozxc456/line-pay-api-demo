using LinePayDemo.Api.Contracts.Request;
using LinePayDemo.Api.Helpers;
using LinePayDemo.Order.Services;
using LinePayDemo.Payment.Services;
using Microsoft.AspNetCore.Mvc;

namespace LinePayDemo.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LinePayController(IPaymentService paymentService, IOrderService orderService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateLinePayDepositAsync([FromBody] InitiateLinePayDepositRequest request)
    {
        if (request.Amount <= 0)
        {
            return BadRequest(new ResponseModel { Message = "儲值金額必須大於 0。" });
        }

        var userId = request.UserId;
        string confirmUrl, cancelUrl;

        if (request.Platform == "mobile")
        {
            confirmUrl = "sylvie://linepay/confirm";
            cancelUrl = "sylvie://linepay/cancel";
        }
        else
        {
            confirmUrl = Url.Action("ConfirmLinePayDeposit", "LinePay", "",Request.Scheme) ?? "";
            cancelUrl = Url.Action("CancelLinePayDeposit", "LinePay","", Request.Scheme) ?? "";
        }

        var orderResult = await orderService.CreateTopUpOrderAsync(request.UserId, request.Amount);
        var result = await paymentService.CreateDepositPaymentAsync(request.UserId, orderResult.OrderId,
            confirmUrl, cancelUrl);

        if (result.IsSuccess)
        {
            return Ok(new ResponseModel { Message = result.Message, Data = result });
        }

        return BadRequest(new ResponseModel { Message = result.Message });
    }

    [HttpGet("confirm")]
    public async Task<IActionResult> ConfirmLinePayDeposit(Guid orderId, long transactionId)
    {
        var result = await paymentService.HandleDepositPaymentAsync(orderId, transactionId);

        if (result)
        {
            return Ok(new ResponseModel { Message = "支付成功" });
        }

        return BadRequest(new ResponseModel { Message = "支付失敗" });
    }

    [HttpGet("cancel")]
    public async Task<IActionResult> CancelLinePayDeposit(Guid orderId, long transactionId)
    {
        try
        {
            await paymentService.HandleCancelDepositPaymentAsync(orderId, transactionId);
            return Ok(new ResponseModel { Message = "取消成功" });
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return BadRequest(new ResponseModel { Message = "取消失敗" });
        }
    }

    [HttpPost("refund")]
    public async Task<IActionResult> RefundLinePayDeposit([FromBody] RefundLinePayDepositRequest request)
    {
        try
        {
            await paymentService.RefundDepositAsync(request.OrderId, request.UserId, request.TransactionId);
            return Ok(new ResponseModel { Message = "退款成功" });
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return BadRequest(new ResponseModel { Message = "退款失敗" });
        }
    }
}