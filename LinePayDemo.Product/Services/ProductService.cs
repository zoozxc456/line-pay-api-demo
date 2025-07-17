using LinePayDemo.Product.Repositories;

namespace LinePayDemo.Product.Services;

public class ProductService(IProductRepository productRepository) : IProductService
{
    public Task<List<Domain.Product>> GetAllProductsAsync()
    {
        return productRepository.GetAllAsync();
    }

    public Task<Domain.Product?> GetProductByIdAsync(Guid productId)
    {
        return productRepository.GetByIdAsync(productId);
    }
}