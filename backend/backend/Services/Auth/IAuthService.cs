using backend.Models;

public interface IAuthService
{
    Task<User?> LoginAsync(string username, string password);
    Task RegisterAsync(string email, string username, string password);

}