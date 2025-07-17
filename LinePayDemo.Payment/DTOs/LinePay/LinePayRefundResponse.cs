namespace LinePayDemo.Payment.DTOs.LinePay;

public class LinePayRefundResponse
{
    public string ReturnCode { get; set; } = null!;
    public string ReturnMessage { get; set; } = null!;
    public LinePayRefundInfo? Info { get; set; }
}

public class LinePayRefundInfo
{
    public long RefundTransactionId { get; set; }
    public DateTime RefundTransactionDate { get; set; }
}