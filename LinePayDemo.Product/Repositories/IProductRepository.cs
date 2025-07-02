using LinePayDemo.Product.Models;

namespace LinePayDemo.Product.Repositories;

public interface IProductRepository
{
    Task<IEnumerable<ProductItem>> GetAllAsync();
    Task<ProductItem?> GetByIdAsync(Guid id);
}