namespace LinePayDemo.Product.Repositories;

public interface IProductRepository
{
    Task<List<Domain.Product>> GetAllAsync();
    Task<Domain.Product?> GetByIdAsync(Guid id);
}