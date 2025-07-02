using LinePayDemo.Transaction.Enums;

namespace LinePayDemo.Transaction.Models;

public class LinePayTransaction
{
    public Guid Id { get; set; }
    public Guid OrderId { get; set; }
    public long LinePayTransactionId { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; }
    public TransactionStatus Status { get; set; }
    public Guid UserId { get; set; }
    public DateTime RequestDateTime { get; set; }
}