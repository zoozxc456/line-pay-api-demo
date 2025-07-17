namespace LinePayDemo.User.Domain;

public class User
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Account { get; private set; }
    public string Password { get; private set; }
    public decimal CurrentPointBalance { get; private set; }

    private User()
    {
    }

    public User(string name, string account, string password)
    {
        Id = Guid.NewGuid();
        Name = name;
        Account = account;
        Password = password;
        CurrentPointBalance = 0;
    }

    public User(Guid id, string name, string account, string password)
    {
        Id = id;
        Name = name;
        Account = account;
        Password = password;
        CurrentPointBalance = 0;
    }

    public bool IsAuthenticated(string password)
    {
        return Password == password;
    }


    public void DepositPoint(decimal amount)
    {
        CurrentPointBalance += amount;
    }

    public void PurchasePoint(decimal amount)
    {
        if (CurrentPointBalance < amount) throw new InvalidOperationException("There is not enough point to purchase.");

        CurrentPointBalance -= amount;
    }
}