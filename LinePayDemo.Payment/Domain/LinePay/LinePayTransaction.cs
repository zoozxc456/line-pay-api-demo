using LinePayDemo.Payment.Enums;
using LinePayDemo.Ledger.Domain.Transaction;

namespace LinePayDemo.Payment.Domain.LinePay;

public class LinePayTransaction
{
    public Guid Id { get; private set; }
    public long? LinePayTransactionId { get; private set; }
    public Guid OrderId { get; private set; }
    public decimal Amount { get; private set; }
    public string Currency { get; private set; } = "TWD";
    public LinePayTransactionStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? PendingAt { get; private set; }
    public DateTime? ConfirmedAt { get; private set; }
    public DateTime? CancelledAt { get; private set; }
    public Guid? LedgerTransactionId { get; private set; }
    public Transaction? LedgerTransaction { get; private set; }

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
        Status = LinePayTransactionStatus.Created;
        CreatedAt = DateTime.UtcNow;
    }

    public void MarkAsPending(long linePayTransactionId)
    {
        if (Status != LinePayTransactionStatus.Created)
        {
            throw new Exception("Only created transaction can be marked as pending.");
        }

        Status = LinePayTransactionStatus.Pending;
        PendingAt = DateTime.UtcNow;
        LinePayTransactionId = linePayTransactionId;
    }

    public void MarkAsConfirmed()
    {
        if (Status != LinePayTransactionStatus.Pending)
        {
            throw new Exception("Only pending transaction can be marked as confirmed.");
        }

        Status = LinePayTransactionStatus.Confirmed;
        ConfirmedAt = DateTime.UtcNow;
    }

    public void MarkAsCancelled()
    {
        if (Status != LinePayTransactionStatus.Pending)
        {
            throw new Exception("Only pending transaction can be marked as cancelled.");
        }

        Status = LinePayTransactionStatus.Cancelled;
        CancelledAt = DateTime.UtcNow;
    }

    public void MarkAsFailed()
    {
        if (Status is LinePayTransactionStatus.Confirmed
            or LinePayTransactionStatus.Refunded
            or LinePayTransactionStatus.Cancelled)
        {
            throw new Exception("Transaction has been confirmed or refunded or cancelled.");
        }

        Status = LinePayTransactionStatus.Failed;
    }

    public void MarkAsFailed(long? linePayTransactionId)
    {
        if (Status is LinePayTransactionStatus.Confirmed
            or LinePayTransactionStatus.Refunded
            or LinePayTransactionStatus.Cancelled)
        {
            throw new Exception("Transaction has been confirmed or refunded or cancelled.");
        }

        Status = LinePayTransactionStatus.Failed;
        LinePayTransactionId = linePayTransactionId;
    }

    public void MarkAsRefunded(Guid refundId, long linePayRefundTransactionId)
    {
        if (Status != LinePayTransactionStatus.Confirmed)
        {
            throw new Exception("Only confirmed transaction can be marked as refunded.");
        }

        var refundTransaction = _refundTransactions.FirstOrDefault(x => x.Id == refundId);
        if (refundTransaction == null) throw new Exception("Refund transaction not found.");

        refundTransaction.MarkAsConfirmed(linePayRefundTransactionId);
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