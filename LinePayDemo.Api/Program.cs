using LinePayDemo.LinePay.Clients;
using LinePayDemo.LinePay.Services;
using LinePayDemo.LinePay.Settings;
using LinePayDemo.Order.Repositories;
using LinePayDemo.Order.Services;
using LinePayDemo.Product.Repositories;
using LinePayDemo.Product.Services;
using LinePayDemo.Transaction.Repositories;
using LinePayDemo.Transaction.Services;
using LinePayDemo.User.Repositories;
using LinePayDemo.User.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddSingleton<IOrderRepository, InMemoryOrderRepository>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddSingleton<ILinePayTransactionRepository, InMemoryLinePayTransactionRepository>();
builder.Services.AddSingleton<IUserBalanceRepository, UserBalanceRepository>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddSingleton<IProductRepository, InMemoryProductRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddSingleton<IUserRepository, InMemoryUserRepository>();
builder.Services.AddScoped<ILinePayPaymentService, LinePayPaymentService>();
builder.Services.AddScoped<ILinePayApiHttpClient, LinePayApiHttpClient>();
builder.Services.Configure<LinePaySettings>(builder.Configuration.GetSection("LinePaySettings"));
builder.Services.AddHttpClient();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();