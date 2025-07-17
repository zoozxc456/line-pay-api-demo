namespace LinePayDemo.User.Repositories;

public interface IUserRepository
{
    Task<Domain.User?> GetByAccountAsync(string account);
    Task AddAsync(Domain.User user);
    Task UpdateAsync(Domain.User user);
    Task<Domain.User?> GetByIdAsync(Guid id);
    Task<List<Domain.User>> GetAllAsync();
}