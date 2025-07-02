using LinePayDemo.Infrastructure.Persistence.Contexts;
using LinePayDemo.Product.Models;
using Microsoft.EntityFrameworkCore;

namespace LinePayDemo.Infrastructure.Persistence.Seeds;

public class ShoppingMallSeedData(ShoppingMallContext context)
{
    public Task SeedAsync()
    {
        return Task.WhenAll(SeedUsersAsync(), SeedProductsAsync());
    }

    private async Task SeedUsersAsync()
    {
        if (!context.Users.Any())
        {
            var toBeCreatedUser = new User.Models.User
            {
                Id = Guid.Parse("f1f875dc-0d5e-4f91-9694-a5948027f7ef"),
                Account = "demo",
                Name = "demo",
                Password = "demo123"
            };

            await context.Users.AddAsync(toBeCreatedUser);
            await context.SaveChangesAsync();
        }
    }

    private async Task SeedProductsAsync()
    {
        if (!await context.Products.AnyAsync())
        {
            var toBeCreatedProducts = new List<ProductItem>
            {
                new()
                {
                    Id = Guid.Parse("37e63690-4829-4848-9ba2-8bf442c1a92d"), Name = "魔法棒", Price = 100,
                    Description = "一支給有抱負巫師的魔法棒。"
                },
                new()
                {
                    Id = Guid.Parse("50186453-2f49-4ebf-92ba-193f77efdc52"), Name = "生命藥水", Price = 50,
                    Description = "恢復你的能量。"
                },
                new()
                {
                    Id = Guid.Parse("04135adc-9042-443c-9e44-0871633e1244"), Name = "神秘卷軸", Price = 200,
                    Description = "包含古老的知識。"
                }
            };

            await context.Products.AddRangeAsync(toBeCreatedProducts);
            await context.SaveChangesAsync();
        }
    }
}