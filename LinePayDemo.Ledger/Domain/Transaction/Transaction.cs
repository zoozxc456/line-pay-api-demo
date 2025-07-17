namespace LinePayDemo.Ledger.Domain.Transaction;

public class Transaction
{
    public Guid Id { get; set; }
    public DateTime TransactionDate { get; set; } = DateTime.UtcNow;
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public List<TransactionEntry> Entries { get; set; } = [];

    protected Transaction()
    {
    }

    public Transaction(decimal amount, string description)
    {
        Id = Guid.NewGuid();
        TransactionDate = DateTime.UtcNow;
        Amount = amount;
        Description = description;
    }
}