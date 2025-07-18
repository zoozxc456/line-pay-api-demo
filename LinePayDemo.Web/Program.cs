using LinePayDemo.Infrastructure.Persistence.Repositories;
using LinePayDemo.LinePay.Clients;
using LinePayDemo.LinePay.Services;
using LinePayDemo.LinePay.Settings;
using LinePayDemo.Order.Repositories;
using LinePayDemo.Order.Services;
using LinePayDemo.Product.Repositories;
using LinePayDemo.Product.Services;
using LinePayDemo.Transaction.Repositories;
using LinePayDemo.Transaction.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddSingleton<IOrderRepository, InMemoryOrderRepository>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddSingleton<ILinePayTransactionRepository, InMemoryLinePayTransactionRepository>();
builder.Services.AddSingleton<IUserBalanceRepository, InMemoryUserBalanceRepository>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddSingleton<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ILinePayPaymentService, LinePayPaymentService>();
builder.Services.AddScoped<ILinePayApiHttpClient, LinePayApiHttpClient>();
builder.Services.Configure<LinePaySettings>(builder.Configuration.GetSection("LinePaySettings"));
builder.Services.AddHttpClient();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Deposit}/{id?}");

app.Run();