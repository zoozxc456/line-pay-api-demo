using LinePayDemo.LinePay.Enums;

namespace LinePayDemo.LinePay.Domain;

public class LinePayTransaction
{
    public Guid Id { get; private set; }
    public long? LinePayTransactionId { get; private set; }
    public Guid OrderId { get; private set; }
    public decimal Amount { get; private set; }
    public string Currency { get; private set; } = "TWD";
    public LinePayTransactionStatus Status { get; private set; }
    public DateTime RequestedAt { get; private set; }
    public DateTime? ConfirmedAt { get; private set; }
    public DateTime? CancelledAt { get; private set; }
    public Guid? LedgerTransactionId { get; private set; }
    public Transaction.Domain.Transaction? LedgerTransaction { get; private set; }

    private readonly List<LinePayRefundTransaction> _refundTransactions = [];
    public IReadOnlyList<LinePayRefundTransaction> RefundTransactions => _refundTransactions;

    private LinePayTransaction()
    {
    }

    public LinePayTransaction(Guid orderId, decimal amount)
    {
        Id = Guid.NewGuid();
        OrderId = orderId;
        Amount = amount;
        Currency = "TWD";
        Status = LinePayTransactionStatus.Pending;
    }

    public void MarkAsConfirmed(long linePayTransactionId)
    {
        Status = LinePayTransactionStatus.Confirmed;
        LinePayTransactionId = linePayTransactionId;
        ConfirmedAt = DateTime.UtcNow;
    }

    public void MarkAsCancelled(long linePayTransactionId)
    {
        Status = LinePayTransactionStatus.Cancelled;
        LinePayTransactionId = linePayTransactionId;
        CancelledAt = DateTime.UtcNow;
    }

    public void MarkAsFailed(long linePayTransactionId)
    {
        Status = LinePayTransactionStatus.Failed;
        LinePayTransactionId = linePayTransactionId;
    }

    public void AddRefundTransaction(LinePayRefundTransaction refundTransaction)
    {
        _refundTransactions.Add(refundTransaction);
        Status = LinePayTransactionStatus.Refunded;
    }

    public void AttachLedgerTransaction(Guid ledgerTransactionId)
    {
        LedgerTransactionId = ledgerTransactionId;
    }
}