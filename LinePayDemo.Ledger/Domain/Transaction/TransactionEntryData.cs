namespace LinePayDemo.Ledger.Domain.Transaction;

public class TransactionEntryData 
{
    public Guid AccountId { get; init; }
    public decimal Debit { get; init; }
    public decimal Credit { get; init; }

    public TransactionEntryData(Guid accountId, decimal debit, decimal credit)
    {
        // 在值物件層面進行數據驗證，確保其內部的數據始終有效
        if (debit < 0 || credit < 0)
        {
            throw new ArgumentException("借方和貸方金額不能為負數。", nameof(debit));
        }

        if (debit > 0 && credit > 0)
        {
            throw new ArgumentException("分錄不能同時有借方和貸方金額。應僅為借方或貸方。");
        }

        if (debit == 0 && credit == 0)
        {
            throw new ArgumentException("分錄的借方和貸方金額不能都為零。");
        }

        AccountId = accountId;
        Debit = debit;
        Credit = credit;
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as TransactionEntryData);
    }

    public bool Equals(TransactionEntryData? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return AccountId == other.AccountId &&
               Debit == other.Debit &&
               Credit == other.Credit;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(AccountId, Debit, Credit);
    }

    public static bool operator ==(TransactionEntryData? left, TransactionEntryData? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(TransactionEntryData? left, TransactionEntryData? right)
    {
        return !Equals(left, right);
    }
}