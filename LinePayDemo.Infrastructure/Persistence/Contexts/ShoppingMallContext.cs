using Microsoft.EntityFrameworkCore;

namespace LinePayDemo.Infrastructure.Persistence.Contexts;

public class ShoppingMallContext(DbContextOptions<ShoppingMallContext> options) : DbContext(options)
{
    public DbSet<User.Models.User> Users { get; set; }
    public DbSet<Product.Models.ProductItem> Products { get; set; }
    public DbSet<Order.Models.Order> Orders { get; set; }
    public DbSet<Transaction.Models.LinePayTransaction> LinePayTransactions { get; set; }
    public DbSet<Transaction.Models.UserBalance> UserBalances { get; set; }
    public DbSet<Transaction.Models.LinePayRefundTransaction> LinePayRefundTransactions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User.Models.User>(builder => builder.HasKey(u => u.Id));
        modelBuilder.Entity<Product.Models.ProductItem>(builder => builder.HasKey(u => u.Id));
        modelBuilder.Entity<Order.Models.Order>(builder => builder.HasKey(u => u.OrderId));
        modelBuilder.Entity<Transaction.Models.LinePayTransaction>(builder =>
        {
            builder.HasKey(u => u.Id); 

            builder.HasMany(t => t.RefundTransactions)
                .WithOne()
                .HasForeignKey(r => r.OriginalLinePayTransactionId) 
                .OnDelete(DeleteBehavior.Restrict); 
        });
        modelBuilder.Entity<Transaction.Models.UserBalance>(builder => builder.HasKey(u => u.UserId));
        modelBuilder.Entity<Transaction.Models.LinePayRefundTransaction>(builder =>
        {
            builder.HasKey(u => u.Id); // Id 作為主鍵
            builder.HasIndex(x => x.OriginalLinePayTransactionId);
        });
    }
}