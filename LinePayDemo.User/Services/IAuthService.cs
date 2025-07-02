namespace LinePayDemo.User.Services;

public interface IAuthService
{
    Task<Guid> AuthenticateAsync(string account, string password);
}