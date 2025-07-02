using LinePayDemo.Product.Models;

namespace LinePayDemo.Product.Services;

public interface IProductService
{
    Task<List<ProductItem>> GetAllProductsAsync();
    Task<ProductItem?> GetProductByIdAsync(Guid productId);
}