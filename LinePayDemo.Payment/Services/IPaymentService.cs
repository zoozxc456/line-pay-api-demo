using LinePayDemo.Payment.DTOs.Common;

namespace LinePayDemo.Payment.Services;

public interface IPaymentService
{
    Task<CreatePaymentResult> CreateDepositPaymentAsync(Guid userId, Guid orderId, string confirmUrl,
        string cancelUrl);

    Task<bool> HandleDepositPaymentAsync(Guid orderId, long transactionId);
    Task HandleCancelDepositPaymentAsync(Guid orderId, long transactionId);
    Task RefundDepositAsync(Guid orderId, Guid userId, long transactionId);
    Task PurchasingProductAsync(Guid userId, Guid orderId);
}