namespace LinePayDemo.Product.Models;

public class ProductItem
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; }
}