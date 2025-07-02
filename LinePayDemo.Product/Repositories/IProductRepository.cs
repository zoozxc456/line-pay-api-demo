using LinePayDemo.Product.Models;

namespace LinePayDemo.Product.Repositories;

public interface IProductRepository
{
    Task<List<ProductItem>> GetAllAsync();
    Task<ProductItem?> GetByIdAsync(Guid id);
}