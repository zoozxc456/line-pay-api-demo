using LinePayDemo.Infrastructure.Persistence.Contexts;
using LinePayDemo.Ledger.Domain.Account;
using Microsoft.EntityFrameworkCore;

namespace LinePayDemo.Infrastructure.Persistence.Seeds;

public class ShoppingMallSeedData(ShoppingMallContext context)
{
    public async Task SeedAsync()
    {
        await SeedUsersAsync();
        await SeedProductsAsync();
        await SeedLedgerAccount();
    }

    private async Task SeedUsersAsync()
    {
        if (!context.Users.Any())
        {
            var id = Guid.Parse("f1f875dc-0d5e-4f91-9694-a5948027f7ef");
            const string account = "demo";
            const string name = "demo";
            const string password = "demo123";

            var toBeCreatedUser = new User.Domain.User(id, name, account, password);

            await context.Users.AddAsync(toBeCreatedUser);
            await context.SaveChangesAsync();
        }
    }

    private async Task SeedProductsAsync()
    {
        if (!await context.Products.AnyAsync())
        {
            var toBeCreatedProducts = new List<Product.Domain.Product>
            {
                new(Guid.Parse("37e63690-4829-4848-9ba2-8bf442c1a92d"), "魔法棒", 100m, "一支給有抱負巫師的魔法棒"),
                new(Guid.Parse("50186453-2f49-4ebf-92ba-193f77efdc52"), "生命藥水", 50m, "恢復你的能量"),
                new(Guid.Parse("04135adc-9042-443c-9e44-0871633e1244"), "神秘卷軸", 200m, "包含古老的知識"),
                new(Guid.Parse("cf618096-39db-4a9d-8f67-0141cac8c8d7"), "點數儲值", 0m, "會員點數儲值")
            };

            await context.Products.AddRangeAsync(toBeCreatedProducts);
            await context.SaveChangesAsync();
        }
    }

    private async Task SeedLedgerAccount()
    {
        if (!await context.Accounts.AnyAsync())
        {
            var accounts = new List<Account>
            {
                new(Guid.Parse("0ae2a152-32b8-4a88-a069-67fc95b55d07"), "其他應收款 - LINE Pay", 0m),
                new(Guid.Parse("69fc640b-9b70-4ffc-9296-41b7519f6e4e"), "預收會員儲值金", 0m),
                new(Guid.Parse("a562a724-c1e8-4e19-ae7f-41adea50c3f7"), "銷貨收入", 0m)
            };

            await context.Accounts.AddRangeAsync(accounts);
            await context.SaveChangesAsync();
        }
    }
}