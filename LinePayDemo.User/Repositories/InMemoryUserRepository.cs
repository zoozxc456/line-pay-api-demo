namespace LinePayDemo.User.Repositories;

public class InMemoryUserRepository : IUserRepository
{
    private static readonly Dictionary<Guid, Models.User> Users = new()
    {
        {
            Guid.Parse("37e63690-4829-4848-9ba2-8bf442c1a92d"),
            new Models.User
            {
                Account = "demo",
                Id = Guid.Parse("37e63690-4829-4848-9ba2-8bf442c1a92d"),
                Name = "demo",
                Password = "demo123"
            }
        }
    };

    public Task<Models.User?> GetByAccountAsync(string account)
    {
        var user = Users.Values.FirstOrDefault(x => x.Account == account);

        return Task.FromResult(user);
    }

    public Task AddAsync(Models.User user)
    {
        Users.TryGetValue(user.Id, out var existUser);

        if (existUser != null)
        {
            throw new InvalidOperationException($"User {user.Id} already exists.");
        }

        Users[user.Id] = user;
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Models.User user)
    {
        if (Users.ContainsKey(user.Id))
        {
            Users[user.Id] = user;
            return Task.CompletedTask;
        }

        throw new InvalidOperationException($"User {user.Id} not found.");
    }

    public Task<Models.User?> GetByIdAsync(Guid id)
    {
        Users.TryGetValue(id, out var user);
        return Task.FromResult(user);
    }

    public Task<IEnumerable<Models.User>> GetAllAsync()
    {
        return Task.FromResult<IEnumerable<Models.User>>(Users.Values.ToList());
    }
}