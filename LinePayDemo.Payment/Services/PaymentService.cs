using LinePayDemo.Ledger.DTOs;
using LinePayDemo.Ledger.Services;
using LinePayDemo.Order.Repositories;
using LinePayDemo.Payment.Domain.LinePay;
using LinePayDemo.Payment.Domain.UserPoint;
using LinePayDemo.Payment.DTOs.Common;
using LinePayDemo.Payment.DTOs.LinePay;
using LinePayDemo.Payment.Enums;
using LinePayDemo.Payment.Interfaces.Clients;
using LinePayDemo.Payment.Interfaces.Repositories;
using LinePayDemo.User.Repositories;
using Microsoft.Extensions.Logging;

namespace LinePayDemo.Payment.Services;

public class PaymentService(
    ILogger<PaymentService> logger,
    IOrderRepository orderRepository,
    ITransactionService transactionService,
    ILinePayTransactionRepository linePayTransactionRepository,
    IUserPointTransactionRepository userPointTransactionRepository,
    IUserRepository userRepository,
    ILinePayApiHttpClient linePayApiHttpClient)
    : IPaymentService
{
    public async Task<CreatePaymentResult> CreateDepositPaymentAsync(Guid userId, Guid orderId, string confirmUrl,
        string cancelUrl)
    {
        var order = await orderRepository.GetByIdAsync(orderId);
        if (order is null) throw new Exception("Order not found.");

        var linePayTransaction = new LinePayTransaction(orderId, order.TotalAmount);
        await linePayTransactionRepository.AddAsync(linePayTransaction);

        var linePayRequestResponse = await linePayApiHttpClient.RequestPayment(new LinePayRequest
        {
            OrderId = orderId,
            Amount = linePayTransaction.Amount,
            Currency = linePayTransaction.Currency,
            Packages = ConvertToLinePayRequestPackages(order),
            RedirectUrls = new LinePayRequest.LinePayRequestRedirectUrls
            {
                CancelUrl = cancelUrl,
                ConfirmUrl = confirmUrl
            }
        });

        if (linePayRequestResponse.ReturnCode == "0000")
        {
            linePayTransaction.MarkAsPending(linePayRequestResponse.Info.TransactionId);
            await linePayTransactionRepository.UpdateAsync(linePayTransaction);
            return new CreatePaymentResult
            {
                IsSuccess = true,
                Message = "支付請求成功",
                PaymentUrl = new LinePayPaymentCallbackUrl
                {
                    App = linePayRequestResponse.Info.PaymentUrl.App,
                    Web = linePayRequestResponse.Info.PaymentUrl.Web,
                },
                Amount = order.TotalAmount,
                OrderId = orderId,
                TransactionId = linePayRequestResponse.Info.TransactionId
            };
        }

        linePayTransaction.MarkAsFailed(linePayRequestResponse.Info?.TransactionId);
        await linePayTransactionRepository.UpdateAsync(linePayTransaction);

        return new CreatePaymentResult
        {
            IsSuccess = true,
            Message = "支付請求成功",
            PaymentUrl = null,
            Amount = order.TotalAmount,
            OrderId = orderId,
            TransactionId = linePayRequestResponse.Info?.TransactionId
        };
    }


    public async Task<bool> HandleDepositPaymentAsync(Guid orderId, long transactionId)
    {
        var linePayTransaction = await linePayTransactionRepository.GetByLinePayTransactionIdAsync(transactionId);
        if (linePayTransaction == null) throw new Exception("Line Pay Transaction not found.");

        var response = await linePayApiHttpClient.ConfirmPayment(transactionId, new LinePayConfirmRequest
        {
            Amount = linePayTransaction.Amount,
            Currency = linePayTransaction.Currency
        });

        if (response.ReturnCode == "0000")
        {
            var order = await orderRepository.GetByIdAsync(orderId);
            linePayTransaction.MarkAsConfirmed();
            await linePayTransactionRepository.UpdateAsync(linePayTransaction);
            var ledgerTransactionId = await PostDepositLedgerTransactionAsync(order!.BuyerId, linePayTransaction.Amount,
                "Deposit Payment");

            linePayTransaction.AttachLedgerTransaction(ledgerTransactionId);
            await linePayTransactionRepository.UpdateAsync(linePayTransaction);

            var userPointTransaction = new UserPointTransaction(order.BuyerId,
                UserPointTransactionType.Deposit,
                order.TotalAmount);

            userPointTransaction.AttachLedgerTransaction(ledgerTransactionId);
            await userPointTransactionRepository.AddAsync(userPointTransaction);

            var user = await userRepository.GetByIdAsync(userPointTransaction.UserId);
            if (user is null) throw new Exception("Not Found User");

            user.DepositPoint(linePayTransaction.Amount);
            await userRepository.UpdateAsync(user);
            return true;
        }

        linePayTransaction.MarkAsFailed();
        await linePayTransactionRepository.UpdateAsync(linePayTransaction);
        return false;
    }

    public async Task HandleCancelDepositPaymentAsync(Guid orderId, long transactionId)
    {
        var linePayTransaction = await linePayTransactionRepository.GetByLinePayTransactionIdAsync(transactionId);
        if (linePayTransaction == null) throw new Exception("Line Pay Transaction not found.");

        linePayTransaction.MarkAsCancelled();
        await linePayTransactionRepository.UpdateAsync(linePayTransaction);

        var order = await orderRepository.GetByIdAsync(orderId);
        if (order is null) throw new Exception("Order not found.");

        order.MarkAsCancelled();
        await orderRepository.UpdateAsync(order);
    }

    public async Task RefundDepositAsync(Guid orderId, Guid userId, long transactionId)
    {
        var order = await orderRepository.GetByIdAsync(orderId);
        if (order is null) throw new Exception("Order not found.");

        var user = await userRepository.GetByIdAsync(order.BuyerId);
        if (user is null) throw new Exception("Not Found User");

        var linePayTransaction = await linePayTransactionRepository.GetByLinePayTransactionIdAsync(transactionId);

        if (linePayTransaction == null)
        {
            throw new Exception("退款失敗：找不到本地交易記錄。");
        }

        if (linePayTransaction.LinePayTransactionId is null)
        {
            throw new Exception("退款失敗：本地交易記錄中缺少 LINE Pay 交易 ID。");
        }

        if (linePayTransaction.Status != LinePayTransactionStatus.Confirmed) // 只有已確認或部分退款的才能再退款
        {
            throw new Exception($"退款失敗：交易狀態為 '{linePayTransaction.Status}'，無法進行退款。");
        }

        if (user.CurrentPointBalance < linePayTransaction.Amount)
        {
            throw new Exception("Not Enough Point");
        }

        try
        {
            var linePayRefundTransaction = new LinePayRefundTransaction(
                linePayTransaction.Id,
                linePayTransaction.Amount,
                linePayTransaction.Currency
            );

            linePayTransaction.AddRefundTransaction(linePayRefundTransaction);
            await linePayTransactionRepository.UpdateAsync(linePayTransaction);

            var refundResponse = await linePayApiHttpClient.RefundPayment(
                linePayTransaction.LinePayTransactionId.Value,
                new LinePayRefundRequest { RefundAmount = linePayTransaction.Amount }
            );

            if (refundResponse is { ReturnCode: "0000", Info: not null })
            {
                linePayTransaction.MarkAsRefunded(linePayRefundTransaction.Id, refundResponse.Info.RefundTransactionId);
                await linePayTransactionRepository.UpdateAsync(linePayTransaction);


                var ledgerTransactionId = await PostRefundLedgerTransactionAsync(order.BuyerId,
                    linePayTransaction.Amount,
                    "Deposit Refunded");

                var userPointTransaction = new UserPointTransaction(order.BuyerId,
                    UserPointTransactionType.Refund, order.TotalAmount);

                userPointTransaction.AttachLedgerTransaction(ledgerTransactionId);
                await userPointTransactionRepository.AddAsync(userPointTransaction);

                user.PurchasePoint(linePayTransaction.Amount);
                await userRepository.UpdateAsync(user);

                logger.LogInformation(
                    $"LINE Pay Refund successful for OrderId: {orderId}. Refund Transaction ID: {refundResponse.Info.RefundTransactionId}");
            }

            if (refundResponse.ReturnCode == "1165")
            {
                logger.LogInformation(
                    $"LINE Pay Had Refunded already for OrderId: {orderId}. LinePay Transaction ID: {transactionId}");

                throw new Exception("This transaction had been refunded already.");
            }

            logger.LogError(
                $"LINE Pay Refund failed for OrderId: {orderId}. Code: {refundResponse.ReturnCode}, Message: {refundResponse.ReturnMessage}");
            throw new Exception("Error occurred during LINE Pay refund.");
        }
        catch (HttpRequestException ex)
        {
            logger.LogError(ex, $"HTTP Request to LINE Pay Refund failed for OrderId: {orderId}.");
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"An error occurred during LINE Pay refund for OrderId: {orderId}.");
            throw;
        }
    }

    public async Task PurchasingProductAsync(Guid userId, Guid orderId)
    {
        var order = await orderRepository.GetByIdAsync(orderId);
        if (order is null) throw new Exception("Order not found.");

        var user = await userRepository.GetByIdAsync(order.BuyerId);
        if (user is null) throw new Exception("Not Found User");

        if (user.CurrentPointBalance < order.TotalAmount)
        {
            throw new Exception("Not Enough Point");
        }

        var ledgerTransactionId = await PostPurchaseLedgerTransactionAsync(order.BuyerId,
            order.TotalAmount, "Purchase Product");

        var userPointTransaction = new UserPointTransaction(order.BuyerId,
            UserPointTransactionType.Refund, order.TotalAmount);

        userPointTransaction.AttachLedgerTransaction(ledgerTransactionId);
        await userPointTransactionRepository.AddAsync(userPointTransaction);

        user.PurchasePoint(order.TotalAmount);
        await userRepository.UpdateAsync(user);
    }

    private List<LinePayRequest.LinePayRequestPackage> ConvertToLinePayRequestPackages(Order.Domain.Order order)
    {
        return
        [
            new LinePayRequest.LinePayRequestPackage
            {
                Amount = order.TotalAmount,
                Id = order.OrderId,
                Products = order.OrderDetails.Select(detail => new LinePayRequest.LinePayRequestProduct
                {
                    Id = detail.OrderItemData.ProductId,
                    ImageUrl = "",
                    Name = detail.OrderItemData.Product.Name,
                    Price = detail.OrderItemData.UnitPrice,
                    Quantity = detail.OrderItemData.Quantity
                }).ToList()
            }
        ];
    }

    private async Task<Guid> PostDepositLedgerTransactionAsync(Guid userId, decimal amount, string description)
    {
        var linePayAccountId = Guid.Parse("0ae2a152-32b8-4a88-a069-67fc95b55d07");
        var depositAccountId = Guid.Parse("69fc640b-9b70-4ffc-9296-41b7519f6e4e");

        var transactionEntryDtos = new List<TransactionEntryDto>
        {
            new(linePayAccountId, amount, decimal.Zero),
            new(depositAccountId, decimal.Zero, amount)
        };

        var transaction = await transactionService.PostTransactionAsync(description, transactionEntryDtos);
        return transaction.Id;
    }

    private async Task<Guid> PostRefundLedgerTransactionAsync(Guid userId, decimal amount, string description)
    {
        var linePayAccountId = Guid.Parse("0ae2a152-32b8-4a88-a069-67fc95b55d07");
        var depositAccountId = Guid.Parse("69fc640b-9b70-4ffc-9296-41b7519f6e4e");

        var transactionEntryDtos = new List<TransactionEntryDto>
        {
            new(linePayAccountId, decimal.Zero, amount),
            new(depositAccountId, amount, decimal.Zero)
        };

        var transaction = await transactionService.PostTransactionAsync(description, transactionEntryDtos);
        return transaction.Id;
    }

    private async Task<Guid> PostPurchaseLedgerTransactionAsync(Guid userId, decimal amount, string description)
    {
        var depositAccountId = Guid.Parse("69fc640b-9b70-4ffc-9296-41b7519f6e4e");
        var incomeAccountId = Guid.Parse("a562a724-c1e8-4e19-ae7f-41adea50c3f7");

        var transactionEntryDtos = new List<TransactionEntryDto>
        {
            new(depositAccountId, amount, decimal.Zero),
            new(incomeAccountId, decimal.Zero, amount)
        };

        var transaction = await transactionService.PostTransactionAsync(description, transactionEntryDtos);
        return transaction.Id;
    }
}