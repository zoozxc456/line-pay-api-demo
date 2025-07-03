using LinePayDemo.Api.Helpers;
using LinePayDemo.LinePay.Services;
using Microsoft.AspNetCore.Mvc;

namespace LinePayDemo.Api.Controllers;

public class InitiateLinePayDepositRequest
{
    public Guid UserId { get; set; }
    public decimal Amount { get; set; }
    public string Platform { get; set; }
}

public class RefundLinePayDepositRequest
{
    public Guid UserId { get; set; }
    public Guid OrderId { get; set; }
    public long TransactionId { get; set; }
    public decimal? RefundAmount { get; set; }
}

[ApiController]
[Route("api/[controller]")]
public class LinePayController(ILinePayPaymentService linePayPaymentService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> InitiateLinePayDeposit([FromBody] InitiateLinePayDepositRequest request)
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
            confirmUrl =
                Url.Action("ConfirmLinePayDeposit", "LinePay", new { orderId = "PLACEHOLDER" }, Request.Scheme) ?? "";
            cancelUrl =
                Url.Action("CancelLinePayDeposit", "LinePay", new { orderId = "PLACEHOLDER" }, Request.Scheme) ?? "";
        }


        var result = await linePayPaymentService.InitiateDepositAsync(request.Amount, userId, confirmUrl, cancelUrl);

        if (result.IsSuccess)
        {
            return Ok(new ResponseModel { Message = result.Message, Data = result });
        }

        return BadRequest(new ResponseModel { Message = result.Message });
    }

    [HttpGet("confirm")]
    public async Task<IActionResult> ConfirmLinePayDeposit(Guid orderId, long transactionId)
    {
        var result = await linePayPaymentService.ConfirmDepositAsync(orderId, transactionId);

        if (result.IsSuccess)
        {
            return Ok(new ResponseModel { Message = result.Message });
        }

        return BadRequest(new ResponseModel { Message = result.Message });
    }

    [HttpGet("cancel")]
    public async Task<IActionResult> CancelLinePayDeposit(Guid orderId)
    {
        var result = await linePayPaymentService.CancelDepositAsync(orderId);

        if (result.IsSuccess)
        {
            return Ok(new ResponseModel { Message = result.Message });
        }

        return BadRequest(new ResponseModel { Message = result.Message });
    }

    [HttpPost("refund")]
    public async Task<IActionResult> RefundLinePayDeposit([FromBody] RefundLinePayDepositRequest request)
    {
        var result = await linePayPaymentService.RefundDepositAsync(request.OrderId, request.UserId,
            request.TransactionId, request.RefundAmount);

        if (result.IsSuccess)
        {
            return Ok(new ResponseModel { Message = result.Message });
        }

        return BadRequest(new ResponseModel { Message = result.Message });
    }
}