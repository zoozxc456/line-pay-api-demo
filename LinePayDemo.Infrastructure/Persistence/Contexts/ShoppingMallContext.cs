using LinePayDemo.Ledger.Domain.Account;
using LinePayDemo.Ledger.Domain.Transaction;
using LinePayDemo.Order.Domain;
using LinePayDemo.Payment.Domain.LinePay;
using LinePayDemo.Payment.Domain.UserPoint;
using Microsoft.EntityFrameworkCore;

namespace LinePayDemo.Infrastructure.Persistence.Contexts;

public class ShoppingMallContext(DbContextOptions<ShoppingMallContext> options) : DbContext(options)
{
    public DbSet<User.Domain.User> Users { get; set; }
    public DbSet<Product.Domain.Product> Products { get; set; }
    public DbSet<Order.Domain.Order> Orders { get; set; }
    public DbSet<OrderDetail> OrderDetails { get; set; }
    public DbSet<Account> Accounts { get; init; }
    public DbSet<Transaction> Transactions { get; init; }
    public DbSet<TransactionEntry> TransactionEntries { get; init; }
    public DbSet<LinePayTransaction> LinePayTransactions { get; init; }
    public DbSet<LinePayRefundTransaction> LinePayRefundTransactions { get; init; }
    public DbSet<UserPointTransaction> UserPointTransactions { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User.Domain.User>(builder => { builder.HasKey(u => u.Id); });
        modelBuilder.Entity<Product.Domain.Product>(builder =>
        {
            builder.HasKey(u => u.Id);
            builder.Property(u => u.UnitPrice).HasPrecision(18, 2);
        });
        modelBuilder.Entity<Order.Domain.Order>(builder =>
        {
            builder.HasKey(u => u.OrderId);

            builder.HasMany(u => u.OrderDetails)
                .WithOne(x => x.Order)
                .HasForeignKey(x => x.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(u => u.LedgerTransaction)
                .WithOne()
                .HasForeignKey<Order.Domain.Order>(u => u.LedgerTransactionId)
                .OnDelete(DeleteBehavior.Restrict);
        });
        modelBuilder.Entity<OrderDetail>(builder =>
        {
            builder.HasKey(x => x.Id);

            builder.OwnsOne(x => x.OrderItemData, navigationBuilder =>
            {
                navigationBuilder.Property(x => x.UnitPrice)
                    .HasPrecision(18, 2)
                    .IsRequired();

                navigationBuilder.Property(x => x.ProductId)
                    .IsRequired();

                navigationBuilder.Property(x => x.Quantity)
                    .IsRequired();

                navigationBuilder.HasOne(x => x.Product)
                    .WithMany()
                    .HasForeignKey(x => x.ProductId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        });

        modelBuilder.Entity<Account>(builder =>
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Balance).HasPrecision(18, 2);
        });

        modelBuilder.Entity<Transaction>(builder =>
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Amount).HasPrecision(18, 2);
            builder.HasMany(x => x.Entries)
                .WithOne(u => u.Transaction)
                .HasForeignKey(x => x.TransactionId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<TransactionEntry>(builder =>
        {
            builder.HasKey(x => x.Id);
            builder.OwnsOne(x => x.Data, navigationBuilder =>
            {
                navigationBuilder.Property(x => x.AccountId);
                navigationBuilder.Property(x => x.Credit).HasPrecision(18, 2);
                navigationBuilder.Property(x => x.Debit).HasPrecision(18, 2);

                navigationBuilder.HasOne<Account>()
                    .WithMany()
                    .HasForeignKey(x => x.AccountId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        });

        modelBuilder.Entity<LinePayTransaction>(builder =>
        {
            builder.HasKey(x => x.Id);
            builder.HasMany(x => x.RefundTransactions)
                .WithOne()
                .HasForeignKey(x => x.LinePayTransactionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.LedgerTransaction)
                .WithOne()
                .HasForeignKey<LinePayTransaction>(x => x.LedgerTransactionId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<LinePayRefundTransaction>(builder =>
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.LedgerTransaction)
                .WithOne()
                .HasForeignKey<LinePayRefundTransaction>(x => x.LedgerTransactionId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<UserPointTransaction>(builder =>
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.LedgerTransaction)
                .WithOne()
                .HasForeignKey<UserPointTransaction>(x => x.LedgerTransactionId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // modelBuilder.Entity<Transaction.Models.LinePayTransaction>(builder =>
        // {
        //     builder.HasKey(u => u.Id); 
        //
        //     builder.HasMany(t => t.RefundTransactions)
        //         .WithOne()
        //         .HasForeignKey(r => r.OriginalLinePayTransactionId) 
        //         .OnDelete(DeleteBehavior.Restrict); 
        // });
        // modelBuilder.Entity<Transaction.Models.UserBalance>(builder => builder.HasKey(u => u.UserId));
        // modelBuilder.Entity<Transaction.Models.LinePayRefundTransaction>(builder =>
        // {
        //     builder.HasKey(u => u.Id); // Id 作為主鍵
        //     builder.HasIndex(x => x.OriginalLinePayTransactionId);
        // });
    }
}