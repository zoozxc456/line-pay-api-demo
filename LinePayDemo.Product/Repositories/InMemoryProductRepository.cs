using LinePayDemo.Product.Models;

namespace LinePayDemo.Product.Repositories;

public class InMemoryProductRepository : IProductRepository
{
    private readonly List<ProductItem> _products =
    [
        new()
        {
            Id = Guid.Parse("37e63690-4829-4848-9ba2-8bf442c1a92d"), Name = "魔法棒", Price = 100,
            Description = "一支給有抱負巫師的魔法棒。"
        },

        new()
        {
            Id = Guid.Parse("50186453-2f49-4ebf-92ba-193f77efdc52"), Name = "生命藥水", Price = 50, Description = "恢復你的能量。"
        },

        new()
        {
            Id = Guid.Parse("04135adc-9042-443c-9e44-0871633e1244"), Name = "神秘卷軸", Price = 200,
            Description = "包含古老的知識。"
        }
    ];

    public Task<List<ProductItem>> GetAllAsync()
    {
        return Task.FromResult(_products);
    }

    public Task<ProductItem?> GetByIdAsync(Guid id)
    {
        return Task.FromResult(_products.FirstOrDefault(p => p.Id == id));
    }
}