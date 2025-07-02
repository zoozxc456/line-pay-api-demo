using LinePayDemo.LinePay.Models;

namespace LinePayDemo.LinePay.Clients;

public interface ILinePayApiHttpClient
{
    Task<LinePayRequestResponse> RequestPayment(LinePayRequest request);
    Task<LinePayConfirmResponse> ConfirmPayment(long transactionId, LinePayConfirmRequest request);
}