using LinePayPaymentUrl = LinePayDemo.LinePay.Models.LinePayRequestResponse.LinePayPaymentUrl;

namespace LinePayDemo.LinePay.Services;

public class LinePayPaymentResult
{
    public bool IsSuccess { get; private set; }
    public string Message { get; private set; }
    public LinePayPaymentUrl? PaymentUrl { get; private set; }
    public decimal? Amount { get; private set; } // 儲值點數金額
    public Guid OrderId { get; private set; }
    public long TransactionId { get; private set; }

    private LinePayPaymentResult(bool success, string message, LinePayPaymentUrl? paymentUrl = null,
        decimal? amount = null, Guid? orderId = null, long? transactionId = null)
    {
        IsSuccess = success;
        Message = message;
        PaymentUrl = paymentUrl;
        Amount = amount;
        OrderId = orderId ?? Guid.NewGuid();
        TransactionId = transactionId ?? 0;
    }

    public static LinePayPaymentResult Success(string message, decimal? amount = null) =>
        new(true, message, null, amount);

    public static LinePayPaymentResult Success(string message, LinePayPaymentUrl paymentUrl) =>
        new(true, message, paymentUrl);

    public static LinePayPaymentResult Success(string message, LinePayPaymentUrl paymentUrl, Guid orderId,
        long transactionId) =>
        new(true, message, paymentUrl, orderId: orderId, transactionId: transactionId);

    public static LinePayPaymentResult Fail(string errorMessage) => new(false, errorMessage);
}