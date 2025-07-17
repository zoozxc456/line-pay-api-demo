using LinePayDemo.Payment.DTOs.LinePay;

namespace LinePayDemo.LinePay.Clients;

public interface ILinePayApiHttpClient
{
    Task<LinePayRequestResponse> RequestPayment(LinePayRequest request);
    Task<LinePayConfirmResponse> ConfirmPayment(long transactionId, LinePayConfirmRequest request);
    Task<LinePayRefundResponse> RefundPayment(long transactionId, LinePayRefundRequest request);
}