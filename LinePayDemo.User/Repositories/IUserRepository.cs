namespace LinePayDemo.User.Repositories;

public interface IUserRepository
{
    Task<Models.User?> GetByAccountAsync(string account);
    Task AddAsync(Models.User user);
    Task UpdateAsync(Models.User user);
    Task<Models.User?> GetByIdAsync(Guid id);
    Task<IEnumerable<Models.User>> GetAllAsync();
}