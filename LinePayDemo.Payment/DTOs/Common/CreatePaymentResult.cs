namespace LinePayDemo.Payment.DTOs.Common;

public class CreatePaymentResult
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; }
    public LinePayPaymentCallbackUrl? PaymentUrl { get; set; }
    public decimal? Amount { get; set; } // 儲值點數金額
    public Guid OrderId { get; set; }
    public long? TransactionId { get; set; }
}

public abstract class PaymentCallbackUrl
{
    
}

public class LinePayPaymentCallbackUrl : PaymentCallbackUrl
{
    public required string Web { get; set; }
    public required string App { get; set; }
}