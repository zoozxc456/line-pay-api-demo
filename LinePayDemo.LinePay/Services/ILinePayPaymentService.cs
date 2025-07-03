namespace LinePayDemo.LinePay.Services;

public interface ILinePayPaymentService
{
    Task<LinePayPaymentResult> InitiateDepositAsync(decimal amount, Guid userId, string confirmUrl, string cancelUrl);
    Task<LinePayPaymentResult> ConfirmDepositAsync(Guid orderId, long transactionId);
    Task<LinePayPaymentResult> CancelDepositAsync(Guid orderId);

    Task<LinePayPaymentResult> RefundDepositAsync(Guid orderId, Guid userId, long transactionId,
        decimal? refundAmount = null);
}