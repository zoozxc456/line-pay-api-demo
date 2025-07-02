using LinePayDemo.User.Repositories;

namespace LinePayDemo.User.Services;

public class AuthService(IUserRepository userRepository) : IAuthService
{
    public async Task<Guid> AuthenticateAsync(string account, string password)
    {
        var user = await userRepository.GetByAccountAsync(account);
        if (user is null) throw new KeyNotFoundException("This user is not exist.");

        return user.Password.Equals(password) ? user.Id : throw new UnauthorizedAccessException();
    }
}