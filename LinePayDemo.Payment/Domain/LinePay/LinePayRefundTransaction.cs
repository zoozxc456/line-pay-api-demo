using LinePayDemo.Ledger.Domain.Transaction;
using LinePayDemo.Payment.Enums;

namespace LinePayDemo.Payment.Domain.LinePay;

public class LinePayRefundTransaction
{
    public Guid Id { get; private set; }
    public Guid LinePayTransactionId { get; private set; }
    public decimal RefundAmount { get; private set; }
    public string Currency { get; private set; } = "TWD";
    public long? LinePayRefundTransactionId { get; private set; }
    public LinePayRefundTransactionStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? ConfirmedAt { get; private set; }
    public DateTime? CancelledAt { get; private set; }
    public Guid? LedgerTransactionId { get; private set; }
    public Transaction? LedgerTransaction { get; private set; } = null!;

    private LinePayRefundTransaction()
    {
    }

    public LinePayRefundTransaction(Guid linePayTransactionId, decimal refundAmount, string currency)
    {
        Id = Guid.NewGuid();
        LinePayTransactionId = linePayTransactionId;
        RefundAmount = refundAmount;
        Currency = currency;
        Status = LinePayRefundTransactionStatus.Pending;
        CreatedAt = DateTime.UtcNow;
    }

    public void MarkAsConfirmed(long linePayRefundTransactionId)
    {
        Status = LinePayRefundTransactionStatus.Confirmed;
        LinePayRefundTransactionId = linePayRefundTransactionId;
        ConfirmedAt = DateTime.UtcNow;
    }

    public void MarkAsCancelled(long linePayRefundTransactionId)
    {
        Status = LinePayRefundTransactionStatus.Cancelled;
        LinePayRefundTransactionId = linePayRefundTransactionId;
        CancelledAt = DateTime.UtcNow;
    }

    public void MarkAsFailed(long? linePayRefundTransactionId)
    {
        Status = LinePayRefundTransactionStatus.Failed;
        LinePayRefundTransactionId = linePayRefundTransactionId;
    }

    public void AttachLedgerTransaction(Guid ledgerTransactionId)
    {
        LedgerTransactionId = ledgerTransactionId;
    }
}