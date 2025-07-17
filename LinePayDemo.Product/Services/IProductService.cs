namespace LinePayDemo.Product.Services;

public interface IProductService
{
    Task<List<Domain.Product>> GetAllProductsAsync();
    Task<Domain.Product?> GetProductByIdAsync(Guid productId);
}