using LinePayDemo.Infrastructure.Clients;
using LinePayDemo.Infrastructure.Persistence.Contexts;
using LinePayDemo.Infrastructure.Persistence.Repositories;
using LinePayDemo.Infrastructure.Persistence.Seeds;
using LinePayDemo.Infrastructure.Settings;
using LinePayDemo.Ledger.Repositories;
using LinePayDemo.Ledger.Services;
using LinePayDemo.Order.Repositories;
using LinePayDemo.Order.Services;
using LinePayDemo.Payment.Interfaces.Clients;
using LinePayDemo.Payment.Interfaces.Repositories;
using LinePayDemo.Payment.Services;
using LinePayDemo.Product.Repositories;
using LinePayDemo.Product.Services;
using LinePayDemo.User.Repositories;
using LinePayDemo.User.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ShoppingMallSeedData>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<ILinePayTransactionRepository, LinePayTransactionRepository>();
builder.Services.AddScoped<IUserPointTransactionRepository, UserPointTransactionRepository>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
// builder.Services.AddScoped<ILinePayPaymentService, LinePayPaymentService>();
builder.Services.AddScoped<ILinePayApiHttpClient, LinePayApiHttpClient>();
builder.Services.Configure<LinePaySettings>(builder.Configuration.GetSection("LinePaySettings"));
builder.Services.AddDbContext<ShoppingMallContext>(options =>
{
    options.UseSnakeCaseNamingConvention();
    options.UseSqlServer(
        "Server=dev-db.minjie.demo,14333;database=shopping_mall;User ID=sa;Password=MSSQL@Password@2024;TrustServerCertificate=True;MultipleActiveResultSets=True");
});
builder.Services.AddHttpClient();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    var scope = app.Services.CreateAsyncScope();
    var seeder = scope.ServiceProvider.GetRequiredService<ShoppingMallSeedData>();
    seeder.SeedAsync().GetAwaiter();
}

// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();