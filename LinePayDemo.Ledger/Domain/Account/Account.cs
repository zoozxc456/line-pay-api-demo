namespace LinePayDemo.Ledger.Domain.Account;

public class Account
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public decimal Balance { get; set; }

    private Account()
    {
    }

    public Account(Guid id, string name, decimal balance)
    {
        Id = id;
        Name = name;
        Balance = balance;
    }

    public Account(string name, decimal balance)
    {
        Id = Guid.NewGuid();
        Name = name;
        Balance = balance;
    }
}