namespace LinePayDemo.Payment.DTOs.LinePay;

public class LinePayRequestResponse
{
    public string ReturnCode { get; set; }
    public string ReturnMessage { get; set; }
    public LinePayRequestInfo Info { get; set; }

    public class LinePayRequestInfo
    {
        public required string PaymentAccessToken { get; set; }
        public required long TransactionId { get; set; }
        public required LinePayPaymentUrl PaymentUrl { get; set; }
    }

    public class LinePayPaymentUrl
    {
        public required string Web { get; set; }
        public required string App { get; set; }
    }
}