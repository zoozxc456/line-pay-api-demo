using LinePayDemo.Product.Models;
using LinePayDemo.Product.Repositories;

namespace LinePayDemo.Product.Services;

public class ProductService(IProductRepository productRepository) : IProductService
{
    public Task<List<ProductItem>> GetAllProductsAsync()
    {
        return productRepository.GetAllAsync();
    }

    public Task<ProductItem?> GetProductByIdAsync(Guid productId)
    {
        return productRepository.GetByIdAsync(productId);
    }
}