using LinePayDemo.Infrastructure.Persistence.Contexts;
using LinePayDemo.Product.Models;
using LinePayDemo.Product.Repositories;

namespace LinePayDemo.Infrastructure.Persistence.Repositories;

public class ProductRepository(ShoppingMallContext context)
    : GenericRepository<Product.Domain.Product>(context), IProductRepository
{
    public Task<List<Product.Domain.Product>> GetAllAsync()
    {
        return FindAllAsync();
    }

    public Task<Product.Domain.Product?> GetByIdAsync(Guid id)
    {
        return FindAsync(x => x.Id == id);
    }
}