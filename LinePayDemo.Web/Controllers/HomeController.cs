using System.Diagnostics;
using LinePayDemo.LinePay.Services;
using LinePayDemo.Order.Services;
using LinePayDemo.Product.Services;
using LinePayDemo.Transaction.Services;
using Microsoft.AspNetCore.Mvc;
using LinePayDemo.Web.Models;

namespace LinePayDemo.Web.Controllers;

public class HomeController(
    ILogger<HomeController> logger,
    ILinePayPaymentService linePayPaymentService,
    IProductService productService,
    IOrderService orderService,
    ITransactionService transactionService) : Controller
{
    public IActionResult Deposit()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> InitiateLinePayDeposit(decimal amount)
    {
        if (amount <= 0)
        {
            ViewBag.ErrorMessage = "儲值金額必須大於 0。";
            return View("Deposit");
        }

        var userId = Guid.Parse("d6c542b1-400b-4647-8d2f-212e20993b35");
        var confirmUrl =
            Url.Action("ConfirmLinePayDeposit", "Home", new { orderId = "PLACEHOLDER" }, Request.Scheme) ?? "";
        var cancelUrl =
            Url.Action("CancelLinePayDeposit", "Home", new { orderId = "PLACEHOLDER" }, Request.Scheme) ?? "";

        var result = await linePayPaymentService.InitiateDepositAsync(amount, userId, confirmUrl, cancelUrl);

        if (result is { IsSuccess: true, PaymentUrl: not null } && !string.IsNullOrEmpty(result.PaymentUrl.Web))
        {
            return Redirect(result.PaymentUrl.Web);
        }
        else
        {
            ViewBag.ErrorMessage = result.Message;
            return View("Deposit");
        }
    }

    public async Task<IActionResult> ConfirmLinePayDeposit(Guid orderId, long transactionId)
    {
        var result = await linePayPaymentService.ConfirmDepositAsync(orderId, transactionId);

        if (result.IsSuccess)
        {
            ViewBag.Message = result.Message;
        }
        else
        {
            ViewBag.Message = result.Message;
        }

        return View("DepositResult");
    }

    public async Task<IActionResult> CancelLinePayDeposit(Guid orderId)
    {
        var result = await linePayPaymentService.CancelDepositAsync(orderId);

        if (result.IsSuccess)
        {
            ViewBag.Message = result.Message;
        }
        else
        {
            ViewBag.Message = result.Message;
        }

        return View("DepositResult");
    }

    // --- 商品購買區塊 ---

    public async Task<IActionResult> Products()
    {
        var userId = Guid.Parse("d6c542b1-400b-4647-8d2f-212e20993b35");
        var products = await productService.GetAllProductsAsync();
        var userBalance = await transactionService.GetUserBalanceAsync(userId);
        ViewBag.UserBalance = userBalance;
        return View(products);
    }

    [HttpPost]
    public async Task<IActionResult> Purchase(Guid productId, int quantity)
    {
        if (quantity <= 0)
        {
            ViewBag.ErrorMessage = "數量必須大於 0。";
            return RedirectToAction("Products");
        }

        var userId = Guid.Parse("d6c542b1-400b-4647-8d2f-212e20993b35");

        // 在系統中建立訂單
        var orderResult = await orderService.CreateOrderAsync(userId, productId, quantity);

        if (!orderResult.IsSuccess)
        {
            ViewBag.ErrorMessage = orderResult.Message;
            return RedirectToAction("Products");
        }

        // 扣除點數
        var deductionResult = await transactionService.DeductPointsAsync(userId, orderResult.Amount);

        if (deductionResult.IsSuccess)
        {
            // 標記訂單為已支付
            await orderService.MarkOrderAsPaidAsync(orderResult.OrderId);
            ViewBag.SuccessMessage = $"成功購買！剩餘點數：{deductionResult.RemainingBalance}";
        }
        else
        {
            // 如果點數扣除失敗，則取消訂單建立
            await orderService.CancelOrderAsync(orderResult.OrderId); // 你可能需要不同的狀態，例如「支付失敗」
            ViewBag.ErrorMessage = $"點數扣除失敗：{deductionResult.Message}";
        }

        return RedirectToAction("Products");
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}