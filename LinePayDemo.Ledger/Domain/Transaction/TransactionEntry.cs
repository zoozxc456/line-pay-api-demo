namespace LinePayDemo.Ledger.Domain.Transaction;

public class TransactionEntry
{
    public Guid Id { get; set; }
    public Guid TransactionId { get; set; }
    public Transaction Transaction { get; set; } = null!;
    public TransactionEntryData Data { get; set; }

    private TransactionEntry()
    {
    }

    public TransactionEntry(Guid transactionId, Guid accountId, decimal debit, decimal credit)
    {
        Id = Guid.NewGuid();
        TransactionId = transactionId;
        Data = new TransactionEntryData(accountId, debit, credit);
    }
}