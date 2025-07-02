using LinePayDemo.Product.Models;

namespace LinePayDemo.Product.Services;

public interface IProductService
{
    Task<IEnumerable<ProductItem>> GetAllProductsAsync();
    Task<ProductItem?> GetProductByIdAsync(Guid productId);
}