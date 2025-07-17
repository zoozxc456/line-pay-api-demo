namespace LinePayDemo.Product.Domain;

public class Product
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public decimal UnitPrice { get; private set; }
    public string Description { get; private set; }

    private Product()
    {
    }

    public Product(Guid id, string name, decimal unitPrice, string description = "")
    {
        Id = id;
        Name = name;
        UnitPrice = unitPrice;
        Description = description;
    }
}