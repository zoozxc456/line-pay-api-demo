using LinePayDemo.Transaction.Enums;

namespace LinePayDemo.Transaction.Models;

public class LinePayRefundTransaction
{
    public Guid Id { get; set; }
    public Guid OrderId { get; set; }
    public Guid OriginalLinePayTransactionId { get; set; }
    public Guid RefundRequestId { get; set; }
    public decimal RefundAmount { get; set; }
    public string Currency { get; set; } = "TWD";
    public long LinePayRefundTransactionId { get; set; }
    public RefundTransactionStatus Status { get; set; }
    public DateTime RequestDateTime { get; set; } // 申請退款時間
    public Guid UserId { get; set; }
}