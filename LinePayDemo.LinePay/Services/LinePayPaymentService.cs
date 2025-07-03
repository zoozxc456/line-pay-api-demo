using LinePayDemo.LinePay.Clients;
using LinePayDemo.LinePay.Models;
using LinePayDemo.Transaction.Enums;
using LinePayDemo.Transaction.Services;
using Microsoft.Extensions.Logging;

namespace LinePayDemo.LinePay.Services;

public class LinePayPaymentService(
    ILinePayApiHttpClient linePayApiHttpClient,
    ITransactionService transactionService,
    ILogger<LinePayPaymentService> logger)
    : ILinePayPaymentService
{
    public async Task<LinePayPaymentResult> InitiateDepositAsync(decimal amount, Guid userId, string confirmUrl,
        string cancelUrl)
    {
        if (amount <= 0)
        {
            return LinePayPaymentResult.Fail("儲值金額必須大於 0。");
        }

        Transaction.Models.LinePayTransaction linePayTx = null; // 使用 Transaction 專案的模型
        try
        {
            // 在你的系統中建立一個待處理的交易記錄
            linePayTx = await transactionService.CreateLinePayTransactionAsync(userId, amount,
                TransactionStatus.Pending);

            // 將 URL 中的佔位符替換為實際的 OrderId
            var finalConfirmUrl = confirmUrl.Replace("PLACEHOLDER", linePayTx.OrderId.ToString());
            var finalCancelUrl = cancelUrl.Replace("PLACEHOLDER", linePayTx.OrderId.ToString());

            var request = new LinePayRequest
            {
                Amount = (int)linePayTx.Amount,
                Currency = "TWD",
                OrderId = linePayTx.OrderId,
                Packages =
                [
                    new LinePayRequest.LinePayRequestPackage
                    {
                        Id = linePayTx.OrderId,
                        Amount = (int)linePayTx.Amount,
                        Products =
                        [
                            new LinePayRequest.LinePayRequestProduct
                            {
                                Id = Guid.Parse("6d6609d2-cac7-412d-ac9e-e3b3ccda3173"),
                                Quantity = 1,
                                Price = (int)linePayTx.Amount,
                                Name = "儲值",
                                ImageUrl = ""
                            }
                        ]
                    }
                ],
                RedirectUrls = new LinePayRequest.LinePayRequestRedirectUrls
                {
                    ConfirmUrl = finalConfirmUrl,
                    CancelUrl = finalCancelUrl
                }
            };

            var linePayResponse = await linePayApiHttpClient.RequestPayment(request);

            if (linePayResponse.ReturnCode == "0000")
            {
                // 使用 LinePay 詳細資料更新交易
                await transactionService.UpdateLinePayTransactionDetailsAsync(
                    linePayTx.OrderId,
                    linePayResponse.Info.TransactionId
                );
                logger.LogInformation(
                    $"LINE Pay 請求成功。交易ID：{linePayResponse.Info.TransactionId}，訂單ID：{linePayTx.OrderId}");
                return LinePayPaymentResult.Success("導向 Line Pay",
                    linePayResponse.Info.PaymentUrl,
                    linePayTx.OrderId,
                    linePayResponse.Info.TransactionId);
            }
            else
            {
                // 將交易標記為失敗
                await transactionService.UpdateLinePayTransactionStatusAsync(linePayTx.OrderId,
                    TransactionStatus.Failed);

                logger.LogError($"LINE Pay 請求失敗。代碼：{linePayResponse.ReturnCode}，訊息：{linePayResponse.ReturnMessage}");
                return LinePayPaymentResult.Fail($"LINE Pay 請求失敗：{linePayResponse.ReturnMessage}");
            }
        }
        catch (HttpRequestException ex)
        {
            if (linePayTx != null)
                await transactionService.UpdateLinePayTransactionStatusAsync(linePayTx.OrderId,
                    TransactionStatus.Failed);

            logger.LogError(ex, "向 LINE Pay 發送 HTTP 請求失敗。");
            return LinePayPaymentResult.Fail($"與 LINE Pay 服務通訊失敗：{ex.Message}");
        }
        catch (Exception ex)
        {
            if (linePayTx != null)
                await transactionService.UpdateLinePayTransactionStatusAsync(linePayTx.OrderId,
                    TransactionStatus.Failed);

            logger.LogError(ex, "啟動 LINE Pay 請求時發生錯誤。");
            return LinePayPaymentResult.Fail($"發生錯誤：{ex.Message}");
        }
    }

    public async Task<LinePayPaymentResult> ConfirmDepositAsync(Guid orderId, long transactionId)
    {
        var linePayTx = await transactionService.GetLinePayTransactionByOrderIdAsync(orderId);

        if (linePayTx == null || linePayTx.LinePayTransactionId != transactionId)
        {
            logger.LogError($"確認呼叫了未知/不匹配的訂單ID：{orderId} 或交易ID：{transactionId}");
            return LinePayPaymentResult.Fail("支付確認失敗：找不到交易記錄或 ID 不匹配。");
        }

        if (linePayTx.Status == TransactionStatus.Completed)
        {
            logger.LogWarning($"嘗試確認已確認的交易。訂單ID：{orderId}");
            return LinePayPaymentResult.Success($"交易已確認。點數：{linePayTx.Amount}", linePayTx.Amount);
        }

        if (linePayTx.Status != TransactionStatus.Pending)
        {
            logger.LogWarning($"嘗試確認非待處理的交易。訂單ID：{orderId}，目前狀態：{linePayTx.Status}");
            return LinePayPaymentResult.Fail($"此交易狀態為 '{linePayTx.Status}'，無法確認。");
        }

        try
        {
            var confirmRequest = new LinePayConfirmRequest
            {
                Amount = (int)linePayTx.Amount,
                Currency = linePayTx.Currency
            };

            var confirmResponse = await linePayApiHttpClient.ConfirmPayment(transactionId, confirmRequest);

            if (confirmResponse.ReturnCode == "0000")
            {
                // 將交易狀態更新為 Confirmed
                await transactionService.UpdateLinePayTransactionStatusAsync(linePayTx.OrderId,
                    TransactionStatus.Completed);

                // 將點數新增到使用者餘額
                await transactionService.AddPointsToUserBalanceAsync(linePayTx.UserId, linePayTx.Amount);

                logger.LogInformation(
                    $"LINE Pay 確認成功。交易ID：{confirmResponse.Info.TransactionId}，訂單ID：{confirmResponse.Info.OrderId}");
                return LinePayPaymentResult.Success($"恭喜！{linePayTx.Amount} 點已成功存入您的帳戶。", linePayTx.Amount);
            }

            // 在確認期間將交易標記為失敗
            await transactionService.UpdateLinePayTransactionStatusAsync(linePayTx.OrderId,
                TransactionStatus.Failed);

            logger.LogError($"LINE Pay 確認失敗。代碼：{confirmResponse.ReturnCode}，訊息：{confirmResponse.ReturnMessage}");
            return LinePayPaymentResult.Fail(
                $"LINE Pay 儲值失敗：{confirmResponse.ReturnCode} - {confirmResponse.ReturnMessage}");
        }
        catch (HttpRequestException ex)
        {
            await transactionService.UpdateLinePayTransactionStatusAsync(
                linePayTx.OrderId, TransactionStatus.Failed);
            logger.LogError(ex, "向 LINE Pay 確認發送 HTTP 請求失敗。");
            return LinePayPaymentResult.Fail($"與 LINE Pay 服務通訊失敗：{ex.Message}");
        }
        catch (Exception ex)
        {
            await transactionService.UpdateLinePayTransactionStatusAsync(
                linePayTx.OrderId, TransactionStatus.Failed);
            logger.LogError(ex, "確認 LINE Pay 時發生錯誤。");
            return LinePayPaymentResult.Fail($"發生錯誤：{ex.Message}");
        }
    }

    public async Task<LinePayPaymentResult> CancelDepositAsync(Guid orderId)
    {
        var linePayTx = await transactionService.GetLinePayTransactionByOrderIdAsync(orderId);

        if (linePayTx == null)
        {
            logger.LogError($"取消呼叫了未知訂單ID：{orderId}");
            return LinePayPaymentResult.Fail("取消失敗：找不到交易記錄。");
        }

        if (linePayTx.Status == TransactionStatus.Pending)
        {
            await transactionService.UpdateLinePayTransactionStatusAsync(
                linePayTx.OrderId, TransactionStatus.Cancelled);
            logger.LogInformation($"LINE Pay 儲值已由使用者取消。訂單ID：{orderId}");
            return LinePayPaymentResult.Success("您已取消 LINE Pay 儲值。");
        }

        if (linePayTx.Status == TransactionStatus.Confirmed)
        {
            logger.LogWarning($"取消呼叫了已確認的交易。訂單ID：{orderId}");
            return LinePayPaymentResult.Fail("此交易已確認且無法取消。");
        }

        logger.LogInformation($"取消呼叫了狀態為 {linePayTx.Status} 的交易。訂單ID：{orderId}");
        return LinePayPaymentResult.Success($"交易狀態為 '{linePayTx.Status}'，無需操作。");
    }

    public async Task<LinePayPaymentResult> RefundDepositAsync(Guid orderId, Guid userId, long transactionId,
        decimal? refundAmount = null)
    {
        var linePayTx = await transactionService.GetLinePayTransactionByOrderIdAsync(orderId);

        if (linePayTx == null)
        {
            return LinePayPaymentResult.Fail("退款失敗：找不到本地交易記錄。");
        }

        if (linePayTx.LinePayTransactionId.Equals(0))
        {
            LinePayPaymentResult.Fail("退款失敗：本地交易記錄中缺少 LINE Pay 交易 ID。");
        }

        if (linePayTx.Status != TransactionStatus.Completed &&
            linePayTx.Status != TransactionStatus.PartiallyRefunded) // 只有已確認或部分退款的才能再退款
        {
            return LinePayPaymentResult.Fail($"退款失敗：交易狀態為 '{linePayTx.Status}'，無法進行退款。");
        }

        if (refundAmount is <= 0)
        {
            return LinePayPaymentResult.Fail("退款金額必須大於 0。");
        }

        if (refundAmount.HasValue && refundAmount.Value > linePayTx.GetRefundableAmount())
        {
            return LinePayPaymentResult.Fail($"退款金額 {refundAmount.Value} 超出可退款金額 {linePayTx.GetRefundableAmount()}。");
        }

        if (!refundAmount.HasValue && linePayTx.GetRefundableAmount() <= 0)
        {
            return LinePayPaymentResult.Fail($"退款金額 {linePayTx.Amount} 超出可退款金額 {linePayTx.GetRefundableAmount()}。");
        }

        try
        {
            var refundTransaction = await transactionService.CreateRefundTransactionAsync(linePayTx.OrderId,
                linePayTx.Id,
                refundAmount.GetValueOrDefault(0),
                linePayTx.UserId);

            var refundRequest = new LinePayRefundRequest
            {
                RefundAmount = refundAmount
            };

            var refundResponse =
                await linePayApiHttpClient.RefundPayment(linePayTx.LinePayTransactionId, refundRequest);

            if (refundResponse is { ReturnCode: "0000", Info: not null })
            {
                var newStatus = TransactionStatus.Refunded;
                if (refundAmount.HasValue && refundAmount.Value < linePayTx.Amount)
                {
                    await transactionService.ConfirmPartialRefundTransactionStatusAsync(linePayTx.Id, refundTransaction.Id);
                }
                else if (!refundAmount.HasValue) // 全額退款
                {
                    await transactionService.ConfirmRefundTransactionStatusAsync(linePayTx.Id, refundTransaction.Id);
                }
          
                await transactionService.DeductPointsAsync(linePayTx.UserId,
                    refundAmount.GetValueOrDefault(refundAmount ?? linePayTx.Amount));

                logger.LogInformation(
                    $"LINE Pay Refund successful for OrderId: {orderId}. Refund Transaction ID: {refundResponse.Info.RefundTransactionId}");
                return LinePayPaymentResult.Success(
                    $"退款成功。狀態：{newStatus}。退款交易ID：{refundResponse.Info.RefundTransactionId}");
            }

            if (refundResponse.ReturnCode == "1165")
            {
                logger.LogInformation(
                    $"LINE Pay Had Refunded already for OrderId: {orderId}. LinePay Transaction ID: {transactionId}");

                await transactionService.UpdateLinePayTransactionStatusAsync(
                    linePayTx.OrderId,
                    TransactionStatus.Refunded
                );

                return LinePayPaymentResult.Fail("此交易已退款完成，無法再次退款。");
            }

            logger.LogError(
                $"LINE Pay Refund failed for OrderId: {orderId}. Code: {refundResponse.ReturnCode}, Message: {refundResponse.ReturnMessage}");
            return LinePayPaymentResult.Fail($"LINE Pay 退款失敗：{refundResponse.ReturnMessage}");
        }
        catch (HttpRequestException ex)
        {
            logger.LogError(ex, $"HTTP Request to LINE Pay Refund failed for OrderId: {orderId}.");
            return LinePayPaymentResult.Fail($"與 LINE Pay 服務通訊失敗：{ex.Message}");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"An error occurred during LINE Pay refund for OrderId: {orderId}.");
            return LinePayPaymentResult.Fail($"發生錯誤：{ex.Message}");
        }
    }
}