using LinePayDemo.Infrastructure.Persistence.Contexts;
using LinePayDemo.Product.Models;
using LinePayDemo.Product.Repositories;

namespace LinePayDemo.Infrastructure.Persistence.Repositories;

public class ProductRepository(ShoppingMallContext context)
    : GenericRepository<ProductItem>(context), IProductRepository
{
    public Task<List<ProductItem>> GetAllAsync()
    {
        return FindAllAsync();
    }

    public Task<ProductItem?> GetByIdAsync(Guid id)
    {
        return FindAsync(x => x.Id == id);
    }
}