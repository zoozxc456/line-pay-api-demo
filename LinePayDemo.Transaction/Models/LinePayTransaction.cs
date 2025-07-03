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

    private List<LinePayRefundTransaction> _refundTransactions { get; set; } = [];
    public IReadOnlyList<LinePayRefundTransaction> RefundTransactions => _refundTransactions;

    public void AddRefundTransaction(LinePayRefundTransaction refundTransaction)
    {
        _refundTransactions.Add(refundTransaction);
    }

    public decimal GetRefundableAmount()
    {
        var refundedAmount = RefundTransactions.Sum(x => x.RefundAmount);
        return Amount - refundedAmount;
    }

    public void MarkAsRefunded()
    {
        Status = TransactionStatus.Refunded;
    }

    public void MarkAsPartialRefunded()
    {
        Status = TransactionStatus.PartiallyRefunded;
    }

    public void MarkRefundTransactionAsConfirmed(Guid refundId)
    {
        var refundTransaction = _refundTransactions.FirstOrDefault(x => x.Id == refundId);
        if (refundTransaction == null) throw new Exception();

        refundTransaction.Status = RefundTransactionStatus.Confirmed;
    }
}